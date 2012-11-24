namespace AppDomainToolkit
{
    using System;

    /// <summary>
    /// This class exists to prevent DLL hell. Assemblies must be loaded into specific application domains
    /// without crossing those boundaries. We cannot simply remote an AssemblyLoader into a remote 
    /// domain and load assemblies to use in the current domain. Instead, we introduct a tiny, serializable
    /// implementation of the AssemblyTarget class that handles comunication between the foreign app
    /// domain and the default one. This class is simply a wrapper around an assembly loader that translates
    /// Assembly to AssemblyTarget instances before shipping them back to the parent domain.
    /// </summary>
    public class AssemblyTargetLoader : MarshalByRefObject, IAssemblyTargetLoader
    {
        #region Fields & Constants

        private readonly IAssemblyLoader loader;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemotableAssemblyLoader class. This parameterless ctor is
        /// required for remoting.
        /// </summary>
        public AssemblyTargetLoader()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RemotableAssemblyLoader class.
        /// </summary>
        /// <param name="loader">
        /// The AssemblyLoader to use when importing assemblies.
        /// </param>
        public AssemblyTargetLoader(IAssemblyLoader loader)
        {
            this.loader = loader == null ? new AssemblyLoader() : loader;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string assemblyPath, string pdbPath = null)
        {
            return AssemblyTarget.FromAssembly(this.loader.LoadAssembly(loadMethod, assemblyPath, pdbPath));
        }

        /// <inheritdoc/>
        public IAssemblyTarget LoadAssemblyWithReferences(LoadMethod loadMethod, string assemblyPath)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
