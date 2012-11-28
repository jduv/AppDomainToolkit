namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DisposableAppDomainUnitTests
    {
        #region Test Methods

        #region Ctor

        [TestMethod]
        public void Ctor_CurrentApplicationDomain()
        {
            var target = new DisposableAppDomain(AppDomain.CurrentDomain);

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsFalse(target.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullAppDomain()
        {
            var target = new DisposableAppDomain(null);
        }

        #endregion

        #region Dispose

        [TestMethod]
        public void Dispose_CurrentAppDomain()
        {
            // The current app domain should NOT be unloaded, but the object should be disposed.
            var target = new DisposableAppDomain(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
        }

        [TestMethod]
        public void Dispose_ValidAppDomain()
        {
            var target = new DisposableAppDomain(AppDomain.CreateDomain("My domain"));
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_DomainProp()
        {
            // The current app domain should NOT be unloaded, but the object should be disposed.
            var target = new DisposableAppDomain(AppDomain.CreateDomain("My domain"));
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);

            var domain = target.Domain;
        }

        #endregion

        #endregion
    }
}
