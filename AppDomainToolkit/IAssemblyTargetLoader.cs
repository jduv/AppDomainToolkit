using System.Collections.Generic;
namespace AppDomainToolkit
{
    /// <summary>
    /// Defines behavior for a class that loads assembly target instances instead of raw assemblies.
    /// This is specifically useful when loading assemblies into other application domains, but still
    /// wishing to keep tabs on what file specifically was loaded. This is a mirror of the IAssemblyLoader
    /// interface, except with assembly targets instead of raw reflection assemblies.
    /// </summary>
    public interface IAssemblyTargetLoader
    {
        #region Methods

        /// <summary>
        /// Loads the target assembly with the indicated load method into the current application domain and
        /// returns an assembly target.
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
        /// An assembly target pointing to the target assembly.
        /// </returns>
        IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string assemblyPath, string pdbPath = null);

        /// <summary>
        /// Loads the target assembly for reflection only with the indicated load method 
        /// into the current application domain and returns an assembly target.
        /// </summary>
        /// <param name="loadMethod">
        /// The load method to use. 
        /// </param>
        /// <param name="assemblyPath">
        /// The path to the assembly to load.
        /// </param>
        /// <returns>
        /// An assembly target pointing to the target assembly.
        /// </returns>
        IAssemblyTarget ReflectionOnlyLoadAssembly(LoadMethod loadMethod, string assemblyPath);

        /// <summary>
        /// Lads the target assembly with all of it's referencings and corresponding PDB files if they exist. This
        /// should perform a best effor guess as to where these assemblies should life, and it should load the
        /// assemblies into the current application domain and return an assembly target pointing to the original
        /// assembly at the target path.
        /// </summary>
        /// <param name="loadMethod">
        /// The load method to use.
        /// </param>
        /// <param name="assemblyPath">
        /// The path to the assembly to load.
        /// </param>
        /// <returns>
        /// A list of loaded assemblies.
        /// </returns>
        IList<IAssemblyTarget> LoadAssemblyWithReferences(LoadMethod loadMethod, string assemblyPath);

        /// <summary>
        /// Gets a list of all the assemblies loaded into the current application domain.
        /// </summary>
        /// <returns>
        /// An array of AssemblyTargets.
        /// </returns>
        IAssemblyTarget[] GetAssemblies();

        /// <summary>
        /// Gets a list of all the assemblies loaded into the current application domain.
        /// </summary>
        /// <returns>
        /// An array of AssemblyTargets.
        /// </returns>
        IAssemblyTarget[] ReflectionOnlyGetAssemblies();

        #endregion
    }
}
