using System;
namespace AppDomainToolkit
{
    /// <summary>
    /// This is a basic wrapper around reflection or CCI metadata based assemblies. It, however, allows referencing
    /// the assembly without loading it into memory, i.e. by it's file name. Although trivial, it's key to enabling 
    /// remoting across application domains for assembly loading and processing in any context.
    /// </summary>
    public interface IAssemblyTarget
    {
        #region Properties

        /// <summary>
        /// Gets the location of the code base on disk.
        /// </summary>
        Uri CodeBase { get; }

        /// <summary>
        /// Gets the location of the assembly, if applicable.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Gets the full name of the assembly target.
        /// </summary>
        string FullName { get; }

        #endregion
    }
}
