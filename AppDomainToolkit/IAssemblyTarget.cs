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
        /// Gets the location of the assembly on disk. Assembly targets should never be dynamic, or
        /// in-memory assemblies.
        /// </summary>
        string Location { get; }

        #endregion
    }
}
