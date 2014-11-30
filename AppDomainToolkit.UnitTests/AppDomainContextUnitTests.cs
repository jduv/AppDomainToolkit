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

        private static readonly string NoRefsAssemblyName = @"TestWithNoReferences";

        private static readonly string NoRefsAssemblyFileName = NoRefsAssemblyName + @".dll";

        private static readonly string NoRefsAssemblyPath = Path.Combine(TestAssemblyDir, NoRefsAssemblyFileName);

        private static readonly string InternalRefsAssemblyDir = Path.Combine(TestAssemblyDir, "test-with-internal-references");

        private static readonly string InternalRefsAssemblyName = @"TestWithInternalReferences";

        private static readonly string InternalRefsAssemblyFileName = InternalRefsAssemblyName + @".dll";

        private static readonly string InternalRefsAssemblyPath = Path.Combine(InternalRefsAssemblyDir, InternalRefsAssemblyFileName);

        private static readonly string AssemblyAName = "AssemblyA";

        private static readonly string AssemblyAFileName = AssemblyAName + ".dll";

        private static readonly string AssemblyBName = "AssemblyB";

        private static readonly string AssemblyBFileName = AssemblyBName + ".dll";

        #endregion

        #region Test Methods

        #region Create

        [TestMethod]
        public void Create_NoArgs_ValidContext()
        {
            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create();

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.RemoteResolver);
            Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullAppDomainSetupInfo()
        {
            var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create(null);
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

            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create(setupInfo);

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.RemoteResolver);
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

            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create(setupInfo);

            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Domain);
            Assert.IsNotNull(target.UniqueId);
            Assert.IsNotNull(target.RemoteResolver);
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
            AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver> target;
            using (target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                Assert.IsNotNull(target);
                Assert.IsNotNull(target.Domain);
                Assert.IsNotNull(target.UniqueId);
                Assert.IsNotNull(target.RemoteResolver);
                Assert.AreNotEqual(AppDomain.CurrentDomain, target.Domain);
                Assert.IsFalse(target.IsDisposed);
            }

            Assert.IsTrue(target.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_DomainProperty()
        {
            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create();
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
            var domain = target.Domain;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_LoadedAssembliesProperty()
        {
            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create();
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
            var assemblies = target.LoadedAssemblies;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_RemoteResolverPropery()
        {
            var target = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create();
            target.Dispose();

            Assert.IsTrue(target.IsDisposed);
            var resolver = target.RemoteResolver;
        }

        #endregion

        #region FindByCodeBase

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindByCodeBase_NullArgument()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                context.FindByCodeBase(null);
            }
        }

        [TestMethod]
        public void FindByCodeBase_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
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
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                context.FindByFullName(null);
            }
        }

        [TestMethod]
        public void FindByFullName_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
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
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                context.FindByLocation(null);
            }
        }

        [TestMethod]
        public void FindByLocation_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var target = context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.IsNotNull(context.FindByLocation(target.Location));
            }
        }


        #endregion

        #region LoadTarget

        [TestMethod]
        public void LoadTarget_NoRefAssembly_LoadFile()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                context.LoadTarget(LoadMethod.LoadFile, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));

                Assert.IsNotNull(actual);
                Assert.AreEqual(target.FullName, actual.FullName);
                Assert.AreEqual(target.Location, actual.Location);
                Assert.AreEqual(target.CodeBase, target.CodeBase);
            }
        }

        [TestMethod]
        public void LoadTarget_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                context.LoadTarget(LoadMethod.LoadFrom, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));

                Assert.IsNotNull(actual);
                Assert.AreEqual(target.FullName, actual.FullName);
                Assert.AreEqual(target.Location, actual.Location);
                Assert.AreEqual(target.CodeBase, target.CodeBase);
            }
        }

        [TestMethod]
        public void LoadTarget_NoRefAssembly_LoadBits()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                context.LoadTarget(LoadMethod.LoadBits, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));

                Assert.IsNotNull(actual);
                Assert.AreEqual(target.FullName, actual.FullName);
                Assert.AreEqual(string.Empty, actual.Location);
                Assert.AreEqual(target.CodeBase, target.CodeBase);
            }
        }


        #endregion

        #region LoadTargetWithReferences

        [TestMethod]
        public void LoadTargetWithReferences_InternalReferences_LoadFile()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                var targets = context.LoadTargetWithReferences(LoadMethod.LoadFile, target);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        [TestMethod]
        public void LoadTargetWithReferences_InternalReferences_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                var targets = context.LoadTargetWithReferences(LoadMethod.LoadFrom, target);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        [TestMethod]
        public void LoadTargetWithReferences_InternalReferences_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);

                var targets = context.LoadTargetWithReferences(LoadMethod.LoadBits, target);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        #endregion

        #region LoadAssembly

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadFile()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
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
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
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
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
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
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath, Path.ChangeExtension(targetPath, "pdb"));
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }

        [TestMethod]
        public void LoadAssembly_NoRefAssembly_LoadBitsWrongPdbPathSpecified()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var pdbPath = Path.ChangeExtension(Path.Combine(TestAssemblyDir, Guid.NewGuid().ToString(), NoRefsAssemblyFileName), "pdb");
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath, Path.GetFullPath(pdbPath));
                Assert.IsTrue(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }

        #endregion

        #region LoadAssemblyWithReferences

        [TestMethod]
        public void LoadAssemblyWithReferences_InternalReferences_LoadFile()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadFile, targetPath);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        [TestMethod]
        public void LoadAssemblyWithReferences_InternalReferences_LoadFrom()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadFrom, targetPath);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        [TestMethod]
        public void LoadAssemblyWithReferences_InternalReferences_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();

                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadBits, targetPath);

                Assert.IsTrue(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.IsTrue(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }

        #endregion

        #endregion
    }
}
