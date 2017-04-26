namespace AppDomainToolkit
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines behavior for an instance class that should act similar to the static methods on the
    /// Assembly class. The reason for this is to support hot-swappable loading implemenations and
    /// facilitate better testing of encironments by stubbing out how each load method executes.
    /// </summary>
    public interface IAssemblyLoader
    {
        #region Methods

        /// <summary>
        /// Loads the target assembly with the indicated load method into the current application domain
        /// and returns it.
        /// </summary>
        /// <param name="loadMethod">
        /// The load method to use.
        /// </param>
        /// <param name="assemblyPath">
        /// The path to the assembly to load.
        /// </param>
        /// <param name="pdbPath">
        /// The path to the PDB file. Defaults to null, which should result in an intelligent search for the correct
        /// PDB file given the assemblies file name.
        /// </param>
        /// <returns>
        /// The target assembly.
        /// </returns>
        Assembly LoadAssembly(LoadMethod loadMethod, string assemblyPath, string pdbPath = null);

        /// <summary>
        /// Loads the target assembly for reflection only with the indicated load method into the current application domain
        /// and returns it.
        /// </summary>
        /// <param name="loadMethod">
        /// The load method to use.
        /// </param>
        /// <param name="assemblyPath">
        /// The path to the assembly to load.
        /// </param>
        /// <returns>
        /// The target assembly.
        /// </returns>
        Assembly ReflectionOnlyLoadAssembly(LoadMethod loadMethod, string assemblyPath);

        /// <summary>
        /// Loads the target assembly along with all of it's references and corresponding PDB files if they exist.
        /// This should perform a best effort guess as to where these assemblies should live, and it should
        /// load these assemblies into the current application domain and return the original assembly at
        /// the target path.
        /// </summary>
        /// <param name="loadMethod">
        /// The load method to use.
        /// </param>
        /// <param name="assemblyPath">
        /// The path to the assembly to load.
        /// </param>
        /// <returns>
        /// The target assembly.
        /// </returns>
        IList<Assembly> LoadAssemblyWithReferences(LoadMethod loadMethod, string assemblyPath);

        /// <summary>
        /// Gets an array of all the assemblies currently loaded into the current application domain.
        /// </summary>
        /// <returns>
        /// An array of assemblies.
        /// </returns>
        Assembly[] GetAssemblies();

        /// <summary>
        /// Gets an array of all the assemblies currently loaded into the current application domain.
        /// </summary>
        /// <returns>
        /// An array of assemblies.
        /// </returns>
        Assembly[] ReflectionOnlyGetAssemblies();

        #endregion
    }
}
