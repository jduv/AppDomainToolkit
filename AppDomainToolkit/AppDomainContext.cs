
namespace AppDomainToolkit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Loads assemblies into the contained application domain.<br/>
    /// No-hassle wrapper for creating default instances of <see cref="AppDomainToolkit.AppDomainContext&lt;TAssemblyTargetLoader, TAssemblyResolver&gt;" />
    /// </summary>
    public sealed class AppDomainContext 
    {
        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <returns>
        /// A new AppDomainContext.
        /// </returns>
        public static AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver> Create() { return AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create<AssemblyTargetLoader, PathBasedAssemblyResolver>(); }
        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <param name="setupInfo">
        /// The setup info.
        /// </param>
        /// <returns>
        /// A new AppDomainContext.
        /// </returns>
        public static AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver> Create(AppDomainSetup setupInfo) { return AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create<AssemblyTargetLoader, PathBasedAssemblyResolver>(setupInfo); }

        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <param name="domain">The domain to wrap in the context</param>
        /// <returns></returns>
        public static AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver> Wrap(AppDomain domain)
        {
            return AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Wrap(domain);
        }
    }

    /// <summary>
    /// Loads assemblies into the contained application domain.
    /// </summary>
    public sealed class AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver> : IAppDomainContext
        where TAssemblyTargetLoader : MarshalByRefObject, IAssemblyTargetLoader, new()
        where TAssemblyResolver : MarshalByRefObject, IAssemblyResolver, new()
    {
        #region Fields & Constants

        private readonly DisposableAppDomain wrappedDomain;
        private readonly Remote<TAssemblyTargetLoader> loaderProxy;
        private readonly Remote<TAssemblyResolver> resolverProxy;

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
            : this(setupInfo, CreateDomain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AppDomainContext class. The new AppDomainContext will wrap the given domain
        /// </summary>
        /// <param name="domain"></param>
        private AppDomainContext(AppDomain domain)
            : this(domain.SetupInformation, (_, __) => domain)
        {
        }

        private AppDomainContext(AppDomainSetup setupInfo, Func<AppDomainSetup, string, AppDomain> createDomain)
        {
            this.UniqueId = Guid.NewGuid();
            this.AssemblyImporter = new TAssemblyResolver 
            {
                ApplicationBase = setupInfo.ApplicationBase,
                PrivateBinPath = setupInfo.PrivateBinPath
            };

            // Add some root directories to resolve some required assemblies
            // Create the new domain and wrap it for disposal.
            this.wrappedDomain = new DisposableAppDomain(createDomain(setupInfo, UniqueId.ToString()));

            AppDomain.CurrentDomain.AssemblyResolve += this.AssemblyImporter.Resolve;

            // Create remotes
            this.loaderProxy = Remote<TAssemblyTargetLoader>.CreateProxy(this.wrappedDomain);
            this.resolverProxy = Remote<TAssemblyResolver>.CreateProxy(this.wrappedDomain);

            // Assign the resolver in the other domain (just to be safe)
            RemoteAction.Invoke(
                this.wrappedDomain.Domain,
                this.resolverProxy.RemoteObject,
                (resolver) =>
                {
                    AppDomain.CurrentDomain.AssemblyResolve += resolver.Resolve;
                });

            // Assign proper paths to the remote resolver
            this.resolverProxy.RemoteObject.ApplicationBase = setupInfo.ApplicationBase;
            this.resolverProxy.RemoteObject.PrivateBinPath = setupInfo.PrivateBinPath;

            this.IsDisposed = false;
        }

        private static AppDomain CreateDomain(AppDomainSetup setup, string name)
        {
            return AppDomain.CreateDomain(name, null, setup);
        }

        /// <summary>
        /// Finalizes an instance of the AppDomainContext class.
        /// </summary>
        ~AppDomainContext()
        {
            this.OnDispose(false);
            GC.SuppressFinalize(this);
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
        public IAssemblyResolver AssemblyImporter { get; private set; }

        /// <inheritdoc />
        public IAssemblyResolver RemoteResolver
        {
            get
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException("The AppDomain has been unloaded or disposed!");
                }

                return this.resolverProxy.RemoteObject;
            }
        }

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

                var rValue = this.loaderProxy.RemoteObject.GetAssemblies();
                return rValue;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This property hits the remote AppDomain each time you ask for it, so don't call this in a
        /// tight loop unless you like slow code.
        /// </remarks>
        public IEnumerable<IAssemblyTarget> ReflectionOnlyLoadedAssemblies
        {
            get
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException("The AppDomain has been unloaded or disposed!");
                }

                var rValue = this.loaderProxy.RemoteObject.ReflectionOnlyGetAssemblies();
                return rValue;
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
        public static AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver> Create<TNewAssemblyTargetLoader, TNewAssemblyResolver>()
            where TNewAssemblyTargetLoader : MarshalByRefObject, TAssemblyTargetLoader, IAssemblyTargetLoader, new()
            where TNewAssemblyResolver : MarshalByRefObject, TAssemblyResolver, IAssemblyResolver, new()
        {
            var guid = Guid.NewGuid();

            //string rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string rootDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            var setupInfo = new AppDomainSetup()
            {
                ApplicationName = "Temp-Domain-" + guid,
                ApplicationBase = rootDir,
                PrivateBinPath = rootDir
            };

            return new AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver>(setupInfo) { UniqueId = guid };
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
        public static AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver> Create<TNewAssemblyTargetLoader, TNewAssemblyResolver>(AppDomainSetup setupInfo)
            where TNewAssemblyTargetLoader : MarshalByRefObject, TAssemblyTargetLoader, IAssemblyTargetLoader, new()
            where TNewAssemblyResolver : MarshalByRefObject, TAssemblyResolver, IAssemblyResolver, new()
        {
            if (setupInfo == null)
            {
                throw new ArgumentNullException("setupInfo");
            }

            var guid = Guid.NewGuid();
            setupInfo.ApplicationName = string.IsNullOrEmpty(setupInfo.ApplicationName) ?
                "Temp-Domain-" + guid.ToString() :
                setupInfo.ApplicationName;

            return new AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver>(setupInfo) { UniqueId = guid };
        }

        /// <summary>
        /// Creates a new instance of the AppDomainContext class.
        /// </summary>
        /// <param name="domain">The appdomain to wrap.</param>
        /// <returns>A new AppDomainContext.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver> Wrap(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            return new AppDomainContext<TAssemblyTargetLoader, TAssemblyResolver>(domain);
        }
        /// <inheritdoc />
        public void Dispose()
        {
            this.OnDispose(true);
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
        /// <remarks>
        /// In order to ensure that the assembly is loaded the way the caller expects, the LoadMethod property of
        /// the remote domain assembly resolver will be temporarily set to the value of <paramref name="loadMethod"/>.
        /// It will be reset to the original value after the load is complete.
        /// </remarks>
        public IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string path, string pdbPath = null)
        {
            var previousLoadMethod = this.resolverProxy.RemoteObject.LoadMethod;
            this.resolverProxy.RemoteObject.LoadMethod = loadMethod;
            var target = this.loaderProxy.RemoteObject.LoadAssembly(loadMethod, path, pdbPath);
            this.resolverProxy.RemoteObject.LoadMethod = previousLoadMethod;
            return target;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// In order to ensure that the assembly is loaded the way the caller expects, the LoadMethod property of
        /// the remote domain assembly resolver will be temporarily set to the value of <paramref name="loadMethod"/>.
        /// It will be reset to the original value after the load is complete.
        /// </remarks>
        public IAssemblyTarget ReflectionOnlyLoadAssembly(LoadMethod loadMethod, string path)
        {
            var previousLoadMethod = this.resolverProxy.RemoteObject.LoadMethod;
            this.resolverProxy.RemoteObject.LoadMethod = loadMethod;
            var target = this.loaderProxy.RemoteObject.ReflectionOnlyLoadAssembly(loadMethod, path);
            this.resolverProxy.RemoteObject.LoadMethod = previousLoadMethod;
            return target;
        }

        /// <inheritdoc />
        /// <remarks>
        /// In order to ensure that the assembly is loaded the way the caller expects, the LoadMethod property of
        /// the remote domain assembly resolver will be temporarily set to the value of <paramref name="loadMethod"/>.
        /// It will be reset to the original value after the load is complete.
        /// </remarks>
        public IEnumerable<IAssemblyTarget> LoadAssemblyWithReferences(LoadMethod loadMethod, string path)
        {
            var previousLoadMethod = this.resolverProxy.RemoteObject.LoadMethod;
            this.resolverProxy.RemoteObject.LoadMethod = loadMethod;
            var targets = this.loaderProxy.RemoteObject.LoadAssemblyWithReferences(loadMethod, path);
            this.resolverProxy.RemoteObject.LoadMethod = previousLoadMethod;
            return targets;
        }

        #endregion

        #region Private Methods

        private void OnDispose(bool disposing)
        {
            if (disposing)
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

                    this.AssemblyImporter = null;
                }
            }

            this.IsDisposed = true;
        }

        #endregion
    }
}
