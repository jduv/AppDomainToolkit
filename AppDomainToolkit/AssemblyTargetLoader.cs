namespace AppDomainToolkit
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

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
        {
            this.loader = new AssemblyLoader();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string assemblyPath, string pdbPath = null)
        {
            IAssemblyTarget target = null;
            var assembly = this.loader.LoadAssembly(loadMethod, assemblyPath, pdbPath);
            if (loadMethod == LoadMethod.LoadBits)
            {
                // Assemlies loaded by bits will have the codebase set to the assembly that loaded it. Set it to the correct path here.
                var codebaseUri = new Uri(assemblyPath);
                target = AssemblyTarget.FromPath(codebaseUri, assembly.Location, assembly.FullName);
            }
            else
            {
                target = AssemblyTarget.FromAssembly(assembly);
            }

            return target;
        }

        /// <inheritdoc/>
        public IList<IAssemblyTarget> LoadAssemblyWithReferences(LoadMethod loadMethod, string assemblyPath)
        {
            return this.loader.LoadAssemblyWithReferences(loadMethod, assemblyPath).Select(x => AssemblyTarget.FromAssembly(x)).ToList();
        }

        /// <inheritdoc />
        public IAssemblyTarget[] GetAssemblies()
        {
            var assemblies = this.loader.GetAssemblies();
            return assemblies.Select(x => AssemblyTarget.FromAssembly(x)).ToArray();
        }

        #endregion
    }
}
