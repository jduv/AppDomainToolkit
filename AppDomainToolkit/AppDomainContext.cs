namespace AppDomainToolkit
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Loads assemblies into the contained application domain.
    /// </summary>
    public sealed class AppDomainContext : IAppDomainContext
    {
        #region Fields & Constants

        private readonly DisposableAppDomain wrappedDomain;
        private readonly Remote<AssemblyTargetLoader> loaderProxy;

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
            this.Resolver = new PathBasedAssemblyResolver();

            // Add some root directories to resolve some required assemblies
            this.Resolver.AddProbePath(setupInfo.ApplicationBase);
            this.Resolver.AddProbePath(setupInfo.PrivateBinPath);
            this.Resolver.AddProbePath(setupInfo.PrivateBinPath);

            // Create the new domain and wrap it for disposal.
            this.wrappedDomain = new DisposableAppDomain(
                AppDomain.CreateDomain(
                    this.UniqueId.ToString(),
                    null,
                    setupInfo));

            this.wrappedDomain.Domain.AssemblyResolve += this.Resolver.Resolve;
            AppDomain.CurrentDomain.AssemblyResolve += this.Resolver.Resolve;

            // Create a remote for an assembly loader.
            this.loaderProxy = Remote<AssemblyTargetLoader>.CreateProxy(this.wrappedDomain);
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
        public IAssemblyResolver Resolver { get; private set; }

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
        public IAssemblyTarget LoadTarget(LoadMethod loadMethod, IAssemblyTarget target)
        {
            return this.LoadAssembly(loadMethod, target.Location);
        }

        /// <inheritdoc />
        public IAssemblyTarget LoadTargetWithReferences(LoadMethod loadMethod, IAssemblyTarget target)
        {
            return this.LoadAssemblyWithReferences(loadMethod, target.Location);
        }

        /// <inheritdoc/>
        public IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string assemblyPath, string pdbPath = null)
        {
            return this.loaderProxy.RemoteObject.LoadAssembly(loadMethod, assemblyPath, pdbPath);
        }

        /// <inheritdoc />
        public IAssemblyTarget LoadAssemblyWithReferences(LoadMethod loadMethod, string path)
        {
            // BMK Implement me.
            throw new NotImplementedException();
        }

        #endregion
    }
}
