namespace AppDomainToolkit.UnitTests
{
    using System;
    using Xunit;


    public class RemoteUnitTests
    {
        #region Test Method

        #region CreateProxy

        [Fact]
        public void CreateProxy_NullDisposableDomain()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
               {
                   var target = Remote<TestProxy>.CreateProxy((DisposableAppDomain)null);
               });
        }

        [Fact]
        public void CreateProxy_NullDomain()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
               {
                   var target = Remote<TestProxy>.CreateProxy((AppDomain)null);
               });
        }

        [Fact]
        public void CreateProxy_CurrentAppDomain()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);

            Assert.NotNull(target.Domain);
            Assert.Equal(AppDomain.CurrentDomain, target.Domain);
            Assert.NotNull(target.RemoteObject);
            Assert.Equal(TestProxy.MundaneMessage, target.RemoteObject.Message);
        }

        [Fact]
        public void CreateProxy_CtorArguments()
        {
            var message = "Foo";
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain, message);

            Assert.NotNull(target.Domain);
            Assert.Equal(AppDomain.CurrentDomain, target.Domain);
            Assert.NotNull(target.RemoteObject);
            Assert.Equal(message, target.RemoteObject.Message);
        }

        #endregion

        #region Dispose

        [Fact]
        public void Dispose_CurrentAppDomain()
        {
            var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.True(target.IsDisposed);
        }

        [Fact]
        public void Dispose_DomainProperty()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
                  {
                      var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
                      target.Dispose();

                      Assert.True(target.IsDisposed);

                      var domain = target.Domain;
                  });
        }

        [Fact]
        public void Dispose_RemoteObjectProperty()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
                   {
                       var target = Remote<TestProxy>.CreateProxy(AppDomain.CurrentDomain);
                       target.Dispose();

                       Assert.True(target.IsDisposed);

                       var proxy = target.RemoteObject;
                   });
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
