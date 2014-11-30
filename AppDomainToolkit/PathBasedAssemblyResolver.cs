namespace AppDomainToolkit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Handles resolving assemblies in application domains. This class is helpful when attempting to load a
    /// particular assembly into an application domain and the assembly you're looking for doesn't exist in the
    /// main application bin path. This 'feature' of the .NET framework makes assembly loading very, very
    /// irritating, but this little helper class should alleviate much of the pains here. Note that it extends 
    /// MarshalByRefObject, so it can be remoted into another application domain. Paths to directories containing
    /// assembly files that you wish to load should be added to an instance of this class, and then the Resolve
    /// method should be assigned to the AssemblyResolve event on the target application domain.
    /// </summary>
    public class PathBasedAssemblyResolver : MarshalByRefObject, IAssemblyResolver
    {
        #region Fields & Constants

        private readonly HashSet<string> probePaths;
        private readonly IAssemblyLoader loader;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the PathBasedAssemblyResolver class. Exists for MarshalByRefObject
        /// remoting into app domains.
        /// </summary>
        public PathBasedAssemblyResolver()
            : this(null, LoadMethod.LoadFrom)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssemblyResolver class. A default instance of this class will resolve
        /// assemblies into the LoadFrom context.
        /// </summary>
        /// <param name="loader">
        /// The loader to use when loading assemblies. Default is null, which will create and use an instance
        /// of the RemotableAssemblyLoader class.
        /// </param>
        /// <param name="loadMethod">
        /// The load method to use when loading assemblies. Defaults to LoadMethod.LoadFrom.
        /// </param>
        public PathBasedAssemblyResolver(
            IAssemblyLoader loader = null,
            LoadMethod loadMethod = LoadMethod.LoadFrom)
        {
            this.probePaths = new HashSet<string>();
            this.loader = loader == null ? new AssemblyLoader() : loader;
            this.LoadMethod = loadMethod;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public LoadMethod LoadMethod { get; set; }

        /// <inheritdoc />
        private string _applicationBase;

        public string ApplicationBase
        {
            get { return _applicationBase; }
            set
            {
                _applicationBase = value;
                this.AddProbePath(value);
            }
        }

        /// <inheritdoc />
        private string _privateBinPath;

        public string PrivateBinPath
        {
            get { return _privateBinPath; }
            set
            {
                _privateBinPath = value;
                AddProbePath(value);
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void AddProbePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (path.Contains(";"))
            {
                var paths = path.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                this.AddProbePaths(paths);
            }
            else
            {
                this.AddProbePaths(path);
            }
        }

        /// <inheritdoc />
        public void AddProbePaths(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                var dir = new DirectoryInfo(path);
                if (!this.probePaths.Contains(dir.FullName))
                {
                    this.probePaths.Add(dir.FullName);
                }
            }
        }

        /// <inheritdoc />
        public Assembly Resolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            foreach (var path in this.probePaths)
            {
                var dllPath = Path.Combine(path, string.Format("{0}.dll", name.Name));
                if (File.Exists(dllPath))
                {
                    return this.loader.LoadAssembly(this.LoadMethod, dllPath);
                }

                var exePath = Path.ChangeExtension(dllPath, "exe");
                if (File.Exists(exePath))
                {
                    return this.loader.LoadAssembly(this.LoadMethod, exePath);
                }
            }

            // Not found.
            return null;
        }

        #endregion


    }
}
