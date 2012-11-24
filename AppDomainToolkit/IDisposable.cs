namespace AppDomainToolkit
{
    /// <summary>
    /// Basic extension of the System.IDisposable interface that allows the implementation to 
    /// manipulate a flag indicating if the object has been disposed or not.
    /// </summary>
    public interface IDisposable : System.IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this object has been disposed or not.
        /// </summary>
        bool IsDisposed { get; }

        #endregion
    }
}
