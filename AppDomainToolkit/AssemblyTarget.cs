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
        public Uri CodeBase { get; private set; }

        /// <inheritdoc />
        public string Location { get; private set; }

        /// <inheritdoc />
        public string FullName { get; private set; }

        /// <inheritdoc />
        public bool IsDynamic { get; private set; }

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

            if (assembly.IsDynamic)
            {
                return FromDynamic(assembly.FullName);
            }
            else
            {
                var uri = new Uri(assembly.CodeBase);
                return FromPath(uri, assembly.Location, assembly.FullName);
            }
        }

        /// <summary>
        /// Creates a new assembly target for the given location. The only required parameter here is the codebase.
        /// </summary>
        /// <param name="codebase">
        /// The URI to the code base.
        /// </param>
        /// <param name="location">
        /// The location. Must be a valid path and an existing file if supplied--defaults to null.
        /// </param>
        /// <param name="fullname">
        /// The full name of the assembly. Defaults to null.
        /// </param>
        /// <returns>
        /// An AssemblyTarget.
        /// </returns>
        public static IAssemblyTarget FromPath(Uri codebase, string location = null, string fullname = null)
        {
            if (codebase == null)
            {
                throw new ArgumentNullException("codebase", "Codebase URI cannot be null!");
            }

            if (!File.Exists(codebase.LocalPath))
            {
                throw new FileNotFoundException("The target location must be an existing file!");
            }

            if (!string.IsNullOrEmpty(location) && !File.Exists(location))
            {
                throw new FileNotFoundException("The target location must be an existing file!");
            }

            return new AssemblyTarget()
            {
                CodeBase = codebase,
                Location = location,
                FullName = fullname,
                IsDynamic = false,
            };
        }

        /// <summary>
        /// Creates a new assembly target for the given dynamic assembly.
        /// </summary>
        /// <param name="fullName">
        /// The full name of the assembly.
        /// </param>
        /// <returns>
        /// An AssemblyTarget.
        /// </returns>
        public static IAssemblyTarget FromDynamic(string fullName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName", "FullName cannot be null!");
            }

            return new AssemblyTarget()
            {
                CodeBase = null,
                Location = null,
                FullName = fullName,
                IsDynamic = true
            };
        }

        #endregion
    }
}
