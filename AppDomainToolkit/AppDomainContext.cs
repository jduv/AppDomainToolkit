namespace AppDomainToolkit
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

    /// <summary>
    /// Loads assemblies into the contained application domain.
    /// </summary>
    public sealed class AppDomainContext : IAppDomainContext
    {
        #region Fields & Constants

        private readonly DisposableAppDomain wrappedDomain;
        private readonly Remote<AssemblyTargetLoader> loaderProxy;
        private readonly Remote<PathBasedAssemblyResolver> resolverProxy;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the AppDomainAssemblyLoader class. The assembly environment will create
        /// a new application domain with the location of the currently executing assembly as the application base. It
        /// will also add that root directory to the assembly resolver's path in order to properly load a remotable
        /// AssemblyLoader object into context. From here, add whatever assembly probe paths you wish in order to
        /// resolve remote proxies, or extend this class if you desire more specific behavior.
        /// </summary>
        /// <param name="setupInfo">
        /// The setup information.
        /// </param>
        private AppDomainContext(AppDomainSetup setupInfo)
        {
            this.UniqueId = Guid.NewGuid();
            this.LocalResolver = new PathBasedAssemblyResolver();

            // Add some root directories to resolve some required assemblies
            this.LocalResolver.AddProbePath(setupInfo.ApplicationBase);
            this.LocalResolver.AddProbePath(setupInfo.PrivateBinPath);
            this.LocalResolver.AddProbePath(setupInfo.PrivateBinPath);

            // Create the new domain and wrap it for disposal.
            this.wrappedDomain = new DisposableAppDomain(
                AppDomain.CreateDomain(
                    this.UniqueId.ToString(),
                    null,
                    setupInfo));

            AppDomain.CurrentDomain.AssemblyResolve += this.LocalResolver.Resolve;

            // Create remotes
            this.loaderProxy = Remote<AssemblyTargetLoader>.CreateProxy(this.wrappedDomain);
            this.resolverProxy = Remote<PathBasedAssemblyResolver>.CreateProxy(this.wrappedDomain);

            // Create a resolver in the other domain.
            RemoteAction.Invoke(
                this.wrappedDomain.Domain,
                this.resolverProxy.RemoteObject,
                (resolver) =>
                {
                    AppDomain.CurrentDomain.AssemblyResolve += resolver.Resolve;
                });

            // Assign proper paths to the remote resolver
            this.resolverProxy.RemoteObject.AddProbePath(setupInfo.ApplicationBase);
            this.resolverProxy.RemoteObject.AddProbePath(setupInfo.PrivateBinPath);
            this.resolverProxy.RemoteObject.AddProbePath(setupInfo.PrivateBinPathProbe);

            this.IsDisposed = false;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public AppDomain Domain
        {
            get
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException("The AppDomain has been unloaded or disposed!");
                }

                return this.wrappedDomain.Domain;
            }
        }

        /// <summary>
        /// Gets a unique ID assigned to the environment. Useful for dictionary keys.
        /// </summary>
        public Guid UniqueId { get; private set; }

        /// <inheritdoc />
        public IAssemblyResolver LocalResolver { get; private set; }

        /// <inheritdoc />
        /// <remarks>
        /// This property hits the remote AppDomain each time you ask for it, so don't call this in a
        /// tight loop unless you like slow code.
        /// </remarks>
        public IEnumerable<IAssemblyTarget> LoadedAssemblies
        {
            get
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException("The AppDomain has been unloaded or disposed!");
                }

                return this.loaderProxy.RemoteObject.GetAssemblies();
            }
        }

        /// <inheritdoc />
        public bool IsDisposed { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <returns>
        /// A new AppDomainContext.
        /// </returns>
        public static AppDomainContext Create()
        {
            var guid = Guid.NewGuid();
            var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var setupInfo = new AppDomainSetup()
            {
                ApplicationName = "Temp-Domain-" + guid,
                ApplicationBase = rootDir,
                PrivateBinPath = rootDir
            };

            return new AppDomainContext(setupInfo) { UniqueId = guid };
        }

        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <param name="setupInfo">
        /// The setup info.
        /// </param>
        /// <returns>
        /// A new AppDomainContext.
        /// </returns>
        public static AppDomainContext Create(AppDomainSetup setupInfo)
        {
            if (setupInfo == null)
            {
                throw new ArgumentNullException("setupInfo");
            }

            var guid = Guid.NewGuid();
            setupInfo.ApplicationName = string.IsNullOrEmpty(setupInfo.ApplicationName) ?
                "Temp-Domain-" + guid.ToString() :
                setupInfo.ApplicationName;

            return new AppDomainContext(setupInfo) { UniqueId = guid };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                if (!this.wrappedDomain.IsDisposed)
                {
                    this.wrappedDomain.Dispose();
                }

                if (!this.loaderProxy.IsDisposed)
                {
                    this.loaderProxy.Dispose();
                }

                this.IsDisposed = true;
            }

            // No subclasses exist, no need to suppress finalizers.
            //GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public IAssemblyTarget FindByCodeBase(Uri codebaseUri)
        {
            if (codebaseUri == null)
            {
                throw new ArgumentNullException("codebaseUri");
            }

            return this.LoadedAssemblies.FirstOrDefault(x => x.CodeBase.Equals(codebaseUri));
        }

        /// <inheritdoc />
        public IAssemblyTarget FindByLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("Location cannot be null or empty");
            }

            return this.LoadedAssemblies.FirstOrDefault(x => x.Location.Equals(location));
        }

        /// <inheritdoc />
        public IAssemblyTarget FindByFullName(string fullname)
        {
            if (string.IsNullOrEmpty(fullname))
            {
                throw new ArgumentException("Full name cannot be null or empty!");
            }

            return this.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(fullname));
        }

        /// <inheritdoc />
        public IAssemblyTarget LoadTarget(LoadMethod loadMethod, IAssemblyTarget target)
        {
            return this.LoadAssembly(loadMethod, target.CodeBase.LocalPath);
        }

        /// <inheritdoc />
        public IEnumerable<IAssemblyTarget> LoadTargetWithReferences(LoadMethod loadMethod, IAssemblyTarget target)
        {
            return this.LoadAssemblyWithReferences(loadMethod, target.CodeBase.LocalPath);
        }

        /// <inheritdoc/>
        public IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string path, string pdbPath = null)
        {
            return this.loaderProxy.RemoteObject.LoadAssembly(loadMethod, path, pdbPath);
        }

        /// <inheritdoc />
        public IEnumerable<IAssemblyTarget> LoadAssemblyWithReferences(LoadMethod loadMethod, string path)
        {
            return this.loaderProxy.RemoteObject.LoadAssemblyWithReferences(loadMethod, path);
        }

        #endregion
    }
}
