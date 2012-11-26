namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem(@"test-assembly-files\", @"test-assembly-files\")]
    public class AppDomainContextUnitTests
    {
        #region Fields & Constants

        private static readonly string TestAssemblyDir = @"test-assembly-files\";

        private static readonly string NoRefsAssemblyName = @"TestWithNoReferences.dll";

        private static readonly string NoRefsAssemblyPath = TestAssemblyDir + NoRefsAssemblyName;

        #endregion

        #region Test Methods

        #region Create

        [TestMethod]
        public void Create_NoArgs_ValidContext()
        {
            var target = AppDomainContext.Create();

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.LocalResolver);
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
            Assert.IsNotNull(target.LocalResolver);
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
            Assert.IsNotNull(target.LocalResolver);
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
                Assert.IsNotNull(target.LocalResolver);
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

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_LoadedAssembliesProperty()
        {
            var target = AppDomainContext.Create();
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
            var assemblies = target.LoadedAssemblies;
        }

        #endregion

        #region FindByCodeBase

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindByCodeBase_NullArgument()
        {
            using (var context = AppDomainContext.Create())
            {
                context.FindByCodeBase(null);
            }
        }

        [TestMethod]
        public void FindByCodeBase_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.IsNotNull(context.FindByCodeBase(codebaseUri));
            }
        }

        #endregion

        #region FindByFullName

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindByFullName_NullArgument()
        {
            using (var context = AppDomainContext.Create())
            {
                context.FindByFullName(null);
            }
        }

        [TestMethod]
        public void FindByFullName_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var target = context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.IsNotNull(context.FindByFullName(target.FullName));
            }
        }

        #endregion

        #region FindByLocation

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindByLocation_NullArgument()
        {
            using (var context = AppDomainContext.Create())
            {
                context.FindByLocation(null);
            }
        }

        [TestMethod]
        public void FindByLocation_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var target = context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.IsNotNull(context.FindByLocation(target.Location));
            }
        }


        #endregion

        #region LoadTarget



        #endregion

        #region LoadTargetWithReferences



        #endregion

        #region LoadAssembly

        [TestMethod]
        public void LoadAssembly_CurrentlyExecutingAssembly_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var toLoad = Assembly.GetExecutingAssembly().Location;
                context.LoadAssembly(LoadMethod.LoadFile, toLoad);
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.Location.Equals(toLoad)));
            }
        }

        [TestMethod]
        public void LoadAssembly_CurrentlyExecutingAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var toLoad = Assembly.GetExecutingAssembly().Location;
                context.LoadAssembly(LoadMethod.LoadFrom, toLoad);
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.Location.Equals(toLoad)));
            }
        }

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFile, targetPath);
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.CodeBase.Equals(codebaseUri.ToString())));
            }
        }

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.CodeBase.Equals(codebaseUri.ToString())));
            }
        }

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath);
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadBitsPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath, Path.ChangeExtension(targetPath, "pdb"));
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }

        #endregion

        #region LoadAssemblyWithReferences



        #endregion

        #endregion
    }
}
