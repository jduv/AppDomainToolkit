namespace AppDomainToolkit
{
    using System;

    /// <summary>
    /// Defines behavior for implementations that load assemblies into a built-in application domain.
    /// </summary>
    public interface IAppDomainContext : AppDomainToolkit.IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the contained application domain.
        /// </summary>
        AppDomain Domain { get; }

        /// <summary>
        /// Gets the assembly resolver responsible for resolving assemblies in the application domain.
        /// </summary>
        IAssemblyResolver Resolver { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the assembly target into the application domain managed by the context.
        /// </summary>
        /// <param name="loadMethod">
        /// The LoadMethod to use when importing the assembly.
        /// </param>
        /// <param name="target">
        /// The assembly target to load.
        /// </param>
        /// <returns>
        /// An assembly target pointing to the assembly to load.
        /// </returns>
        IAssemblyTarget LoadTarget(LoadMethod loadMethod, IAssemblyTarget target);

        /// <summary>
        /// Loads the assembly target into the application domain managed by the context along with any
        /// reference assemblies
        /// </summary>
        /// <param name="loadMethod">
        /// The LoadMethod to use when importing the assembly.
        /// </param>
        /// <param name="target">
        /// The assembly target to load.
        /// </param>
        /// <returns>
        /// An assembly target pointing to the assembly to load.
        /// </returns>
        IAssemblyTarget LoadTargetWithReferences(LoadMethod loadMethod, IAssemblyTarget target);

        /// <summary>
        /// Loads the assembly at the specified path into the application domain managed by the 
        /// context.
        /// </summary>
        /// <param name="loadMethod">
        /// The LoadMethod to use when importing the assembly.
        /// </param>
        /// <param name="path">
        /// The path to the assembly.
        /// </param>
        /// <param name="pdbPath">
        /// The path to the assembly's PDB file.
        /// </param>
        /// <returns>
        /// An assembly target pointing to the loaded assembly.
        /// </returns>
        IAssemblyTarget LoadAssembly(LoadMethod loadMethod, string path, string pdbPath = null);

        /// <summary>
        /// Loads the assembly target into the application domain managed by the context along with any
        /// reference assemblies
        /// </summary>
        /// <param name="loadMethod">
        /// The LoadMethod to use when importing the assembly.
        /// </param>
        /// <param name="path">
        /// The path to the assembly.
        /// </param>
        /// <returns>
        /// An assembly target pointing to the loaded assembly.
        /// </returns>
        IAssemblyTarget LoadAssemblyWithReferences(LoadMethod loadMethod, string path);

        #endregion
    }
}
