namespace AppDomainToolkit
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Simple class representing an assembly target. This class will be serialized accross application domains
    /// instead of remoted. There's no reason to remote it because it's simply a wrapper around a couple of
    /// strings anyway.
    /// </summary>
    [Serializable]
    public sealed class AssemblyTarget : IAssemblyTarget
    {
        #region Constructors & Destructors

        /// <summary>
        /// Prevents a default instance of the AssemblyTarget class from being created.
        /// </summary>
        private AssemblyTarget()
        {
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public string Location { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new AssemblyTarget from the target assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly to create the target for.
        /// </param>
        /// <returns>
        /// An AssemblyTarget.
        /// </returns>
        public static IAssemblyTarget FromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return Create(assembly.Location);
        }

        /// <summary>
        /// Creates a new assembly target for the given location.
        /// </summary>
        /// <param name="location">
        /// The location. Must be a valid path and an existing file.
        /// </param>
        /// <returns>
        /// An AssemblyTarget.
        /// </returns>
        public static IAssemblyTarget Create(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("Location cannot be null or empty.");
            }

            if (!File.Exists(location))
            {
                throw new ArgumentException("The target location must be an existing file!");
            }

            return new AssemblyTarget()
            {
                Location = location
            };
        }

        #endregion
    }
}
