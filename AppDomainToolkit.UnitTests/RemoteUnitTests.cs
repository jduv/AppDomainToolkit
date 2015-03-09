namespace AppDomainToolkit.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RemoteUnitTests
    {
        #region Test Method

        #region CreateProxy

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateProxy_NullDisposableDomain()
        {
            var target = Remote<TestProxy>.CreateProxy((DisposableAppDomain)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateProxy_NullDomain()
        {
            var target = Remote<TestProxy>.CreateProxy((AppDomain)null);
        }

        [TestMethod]
        public void CreateProxy_CurrentAppDomain()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);

            Assert.IsNotNull(target.Domain);
            Assert.AreEqual(AppDomain.CurrentDomain, target.Domain);
            Assert.IsNotNull(target.RemoteObject);
            Assert.AreEqual(TestProxy.MundaneMessage, target.RemoteObject.Message);
        }

        [TestMethod]
        public void CreateProxy_CtorArguments()
        {
            var message = "Foo";
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain, message);

            Assert.IsNotNull(target.Domain);
            Assert.AreEqual(AppDomain.CurrentDomain, target.Domain);
            Assert.IsNotNull(target.RemoteObject);
            Assert.AreEqual(message, target.RemoteObject.Message);
        }

        #endregion

        #region Dispose

        [TestMethod]
        public void Dispose_CurrentAppDomain()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_DomainProperty()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);

            var domain = target.Domain;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_RemoteObjectProperty()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);

            var proxy = target.RemoteObject;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// This private class exists only to facilitate very basic testing of remoting into another application
    /// domain. It doesn't do anything at all except get instantiated.
    /// </summary>
    internal class TestProxy : MarshalByRefObject
    {
        #region Fields & Constants
        
        internal static readonly string MundaneMessage = "Hello World";
        
        #endregion

        #region Constructors & Destructors

        public TestProxy()
        {
            this.Message = MundaneMessage;
        }

        public TestProxy(string message)
        {
            this.Message = message;
        }

        #endregion

        #region Properties

        public string Message { get; private set; }

        #endregion
    }
}
