namespace AppDomainToolkit
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// Allows for isolated creation of an object of type T imported into another
    /// application domain.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to import. Must be a deriviative of MarshalByRefObject.
    /// </typeparam>
    public sealed class Remote<T> : IDisposable where T : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the Remote class.
        /// </summary>
        private Remote()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wrapped value.
        /// </summary>
        public T RemoteObject { get; private set; }

        /// <summary>
        /// Gets the application domain where the wrapped value lives.
        /// </summary>
        public AppDomain Domain { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new remote.
        /// </summary>
        /// <param name="domain">
        /// The domain for the remote. Default value is null, in which case a new application domain  that 
        /// mirrors the current one will be automatically created.
        /// </param>
        /// <param name="constructorArgs">
        /// A list of constructor arguments to pass to the remote object.
        /// </param>
        /// <returns>
        /// A remote proxy to an object of type T living in the target application domain.
        /// </returns>
        public static Remote<T> CreateProxy(AppDomain domain, params object[] constructorArgs)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            var type = typeof(T);

            var proxy = (T)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                false,
                BindingFlags.CreateInstance,
                null,
                constructorArgs,
                null,
                null);

            return new Remote<T>()
            {
                Domain = domain,
                RemoteObject = proxy
            };
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.Domain != null)
            {
                AppDomain.Unload(this.Domain);

                this.Domain = null;
            }
        }

        #endregion
    }
}
