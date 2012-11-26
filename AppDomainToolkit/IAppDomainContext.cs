namespace AppDomainToolkit
{
    using System;
    using System.Collections.Generic;

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
        IAssemblyResolver LocalResolver { get; }

        /// <summary>
        /// Gets a list of all assemblies loaded into this domain without bleeding them into the current
        /// application domain.
        /// </summary>
        IEnumerable<IAssemblyTarget> LoadedAssemblies { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the first assembly found with the target codebase URI.
        /// </summary>
        /// <param name="codebaseUri">
        /// The URI to look for.
        /// </param>
        /// <returns>
        /// Returns the first assembly found with the target codebase URI, or null if none are found.
        /// </returns>
        IAssemblyTarget FindByCodeBase(Uri codebaseUri);

        /// <summary>
        /// Returns the first assembly found with the target location.
        /// </summary>
        /// <param name="location">
        /// The location to check for.
        /// </param>
        /// <returns>
        /// Returns the first assembly found with the target location, or null if none are found.
        /// </returns>
        IAssemblyTarget FindByLocation(string location);

        /// <summary>
        /// Returns the first assembly found with the target full name.
        /// </summary>
        /// <param name="fullname">
        /// The name of the assembly.
        /// </param>
        /// <returns>
        /// Returns the first assembly found with the target full name, or null if none are found.
        /// </returns>
        IAssemblyTarget FindByFullName(string fullname);

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
        /// A list of assembly targets pointing to the assemblies that were loaded.
        /// </returns>
        IEnumerable<IAssemblyTarget> LoadTargetWithReferences(LoadMethod loadMethod, IAssemblyTarget target);

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
        /// A list of assembly targets pointing to the assemblies that were loaded.
        /// </returns>
        IEnumerable<IAssemblyTarget> LoadAssemblyWithReferences(LoadMethod loadMethod, string path);

        #endregion
    }
}
