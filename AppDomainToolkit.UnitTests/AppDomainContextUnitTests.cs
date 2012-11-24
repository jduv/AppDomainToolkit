namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reflection;
    using System.IO;

    [TestClass]
    public class AppDomainContextUnitTests
    {
        #region Test Methods

        #region Create

        [TestMethod]
        public void Create_NoArgs_ValidContext()
        {
            var target = AppDomainContext.Create();

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.Resolver);
            Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullAppDomainSetupInfo()
        {
            var context = AppDomainContext.Create(null);
        }

        [TestMethod]
        public void Create_ValidSetupInfo()
        {
            var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var setupInfo = new AppDomainSetup()
            {
                ApplicationName = "My app",
                ApplicationBase = workingDir,
                PrivateBinPath = workingDir
            };

            var target = AppDomainContext.Create(setupInfo);

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.Resolver);
            Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);

            // Verify the app domain's setup info
            Assert.AreEqual(setupInfo.ApplicationName, target.Domain.SetupInformation.ApplicationName, true);
            Assert.AreEqual(setupInfo.ApplicationBase, setupInfo.ApplicationBase);
            Assert.AreEqual(setupInfo.PrivateBinPath, target.Domain.SetupInformation.PrivateBinPath);
        }

        [TestMethod]
        public void Create_NoApplicationNameSupplied()
        {
            var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var setupInfo = new AppDomainSetup()
            {
                ApplicationBase = workingDir,
                PrivateBinPath = workingDir
            };

            var target = AppDomainContext.Create(setupInfo);

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.Resolver);
            Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);

            // Verify the app domain's setup info
            Assert.IsFalse(string.IsNullOrEmpty(target.Domain.SetupInformation.ApplicationName));
            Assert.AreEqual(setupInfo.ApplicationBase, setupInfo.ApplicationBase);
            Assert.AreEqual(setupInfo.PrivateBinPath, target.Domain.SetupInformation.PrivateBinPath);
        }

        #endregion

        #region Dispose

        [TestMethod]
        public void Dispose_WithUsingClause()
        {
            AppDomainContext target;
            using (target = AppDomainContext.Create())
            {
                Assert.IsNotNull(target);
                Assert.IsNotNull(target.Domain);
                Assert.IsNotNull(target.UniqueId);
                Assert.IsNotNull(target.Resolver);
                Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);
                Assert.IsFalse(target.IsDisposed);
            }

            Assert.IsTrue(target.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_DomainProperty()
        {
            var target = AppDomainContext.Create();
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
            var domain = target.Domain;
        }

        #endregion

        #region LoadAssembly


        #endregion

        #region LoadTarget



        #endregion

        #endregion
    }
}
