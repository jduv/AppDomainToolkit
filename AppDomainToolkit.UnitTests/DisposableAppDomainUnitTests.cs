namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.Linq;
    using Xunit;

    public class DisposableAppDomainUnitTests
    {
        #region Test Methods

        #region Ctor

        [Fact]
        public void Ctor_CurrentApplicationDomain()
        {
            var target = new DisposableAppDomain(AppDomain.CurrentDomain);

            Assert.NotNull(target);
            Assert.NotNull(target.Domain);
            Assert.False(target.IsDisposed);
        }

        [Fact]
        public void Ctor_NullAppDomain()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var target = new DisposableAppDomain(null);
            });
        }

        #endregion

        #region Dispose

        [Fact]
        public void Dispose_CurrentAppDomain()
        {
            // The current app domain should NOT be unloaded, but the object should be disposed.
            var target = new DisposableAppDomain(AppDomain.CurrentDomain);
            target.Dispose();

            Assert.True(target.IsDisposed);
        }

        [Fact]
        public void Dispose_ValidAppDomain()
        {
            var target = new DisposableAppDomain(AppDomain.CreateDomain("My domain"));
            target.Dispose();

            Assert.True(target.IsDisposed);
        }

        [Fact]
        public void Dispose_DomainProp()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
            {
                // The current app domain should NOT be unloaded, but the object should be disposed.
                var target = new DisposableAppDomain(AppDomain.CreateDomain("My domain"));
                target.Dispose();

                Assert.True(target.IsDisposed);

                var domain = target.Domain;
            });
        }

        #endregion

        #endregion
    }
}