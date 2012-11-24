namespace AppDomainToolkit
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Defines behavior for an object that is able to resolve assemblies for a given application domain.
    /// </summary>
    public interface IAssemblyResolver
    {
        #region Properties

        /// <summary>
        /// Gets or sets the load method for the resolver.
        /// </summary>
        LoadMethod LoadMethod { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a probe path to the assembly loader. This is a directory that will be searched while assembly
        /// resolver events are being processed.
        /// </summary>
        /// <param name="path">
        /// The path to probe.
        /// </param>
        void AddProbePath(string path);

        /// <summary>
        /// Resolves assemblies. This enables all assembly loaders to be able to handle the resolve event in their specified
        /// application domain.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly Resolve(object sender, ResolveEventArgs args);

        #endregion
    }
}
