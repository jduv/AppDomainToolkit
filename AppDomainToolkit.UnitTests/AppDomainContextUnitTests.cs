namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    //[DeploymentItem(@"test-assembly-files\", @"test-assembly-files\")]
    public class AppDomainContextUnitTests
    {
        #region Fields & Constants
        
        private static readonly string TestAssemblyDir = @"test-assembly-files\";
        private static readonly string NoRefsAssemblyName = @"TestWithNoReferences";
        private static readonly string NoRefsAssemblyFileName = string.Format("{0}{1}", NoRefsAssemblyName, @".dll");
        private static readonly string NoRefsAssemblyPath = Path.Combine(TestAssemblyDir, NoRefsAssemblyFileName);
        private static readonly string InternalRefsAssemblyDir = Path.Combine(TestAssemblyDir, "test-with-internal-references");
        private static readonly string InternalRefsAssemblyName = @"TestWithInternalReferences";
        private static readonly string InternalRefsAssemblyFileName = string.Format("{0}{1}", InternalRefsAssemblyName, @".dll");
        private static readonly string InternalRefsAssemblyPath = Path.Combine(InternalRefsAssemblyDir, InternalRefsAssemblyFileName);
        private static readonly string AssemblyAName = "AssemblyA";
        private static readonly string AssemblyAFileName = string.Format("{0}.dll", AssemblyAName);
        private static readonly string AssemblyBName = "AssemblyB";
        private static readonly string AssemblyBFileName = string.Format("{0}.dll", AssemblyBName);
        
        #endregion
        
        #region Test Methods
        
        #region Create
        
        [Fact]
        public void Create_NoArgs_ValidContext()
        {
            var target = AppDomainContext.Create();
            
            Assert.NotNull(target);
            Assert.NotNull(target.Domain);
            Assert.NotNull(target.UniqueId);
            Assert.NotNull(target.RemoteResolver);
            Assert.NotEqual(AppDomain.CurrentDomain, target.Domain);
        }
        
        [Fact]
        public void Create_NullAppDomainSetupInfo()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var context = AppDomainContext.Create(null);
            });
        }
        
        [Fact]
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
            
            Assert.NotNull(target);
            Assert.NotNull(target.Domain);
            Assert.NotNull(target.UniqueId);
            Assert.NotNull(target.RemoteResolver);
            Assert.NotEqual(AppDomain.CurrentDomain, target.Domain);
            
            // Verify the app domain's setup info
            Assert.Equal(setupInfo.ApplicationName, target.Domain.SetupInformation.ApplicationName, true);
            Assert.Equal(setupInfo.ApplicationBase, setupInfo.ApplicationBase);
            Assert.Equal(setupInfo.PrivateBinPath, target.Domain.SetupInformation.PrivateBinPath);
        }
        
        [Fact]
        public void Create_NoApplicationNameSupplied()
        {
            var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var setupInfo = new AppDomainSetup()
            {
                ApplicationBase = workingDir,
                PrivateBinPath = workingDir
            };
            
            var target = AppDomainContext.Create(setupInfo);
            
            Assert.NotNull(target);
            Assert.NotNull(target.Domain);
            Assert.NotNull(target.UniqueId);
            Assert.NotNull(target.RemoteResolver);
            Assert.NotEqual(AppDomain.CurrentDomain, target.Domain);
            
            // Verify the app domain's setup info
            Assert.False(string.IsNullOrEmpty(target.Domain.SetupInformation.ApplicationName));
            Assert.Equal(setupInfo.ApplicationBase, setupInfo.ApplicationBase);
            Assert.Equal(setupInfo.PrivateBinPath, target.Domain.SetupInformation.PrivateBinPath);
        }
        
        [Fact]
        public void Create_NoApplicationNameSupplied_WrappedDomain()
        {
            var target = AppDomainContext.Wrap(AppDomain.CurrentDomain);
            
            Assert.NotNull(target);
            Assert.NotNull(target.Domain);
            Assert.NotNull(target.UniqueId);
            Assert.NotNull(target.RemoteResolver);
            Assert.Equal(AppDomain.CurrentDomain, target.Domain);
            
            // Verify the app domain's setup info
            Assert.False(string.IsNullOrEmpty(target.Domain.SetupInformation.ApplicationName));
        }
        
        #endregion
        
        #region Dispose
        
        [Fact]
        public void Dispose_WithUsingClause()
        {
            AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver> target;
            using (target = AppDomainContext.Create())
            {
                Assert.NotNull(target);
                Assert.NotNull(target.Domain);
                Assert.NotNull(target.UniqueId);
                Assert.NotNull(target.RemoteResolver);
                Assert.NotEqual(AppDomain.CurrentDomain, target.Domain);
                Assert.False(target.IsDisposed);
            }
            
            Assert.True(target.IsDisposed);
        }
        
        [Fact]
        public void Dispose_DomainProperty()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
            {
                var target = AppDomainContext.Create();
                target.Dispose();
                
                Assert.True(target.IsDisposed);
                var domain = target.Domain;
            });
        }
        
        [Fact]
        public void Dispose_LoadedAssembliesProperty()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
            {
                var target = AppDomainContext.Create();
                target.Dispose();
                
                Assert.True(target.IsDisposed);
                var assemblies = target.LoadedAssemblies;
            });
        }
        
        [Fact]
        public void Dispose_RemoteResolverPropery()
        {
            Assert.Throws(typeof(ObjectDisposedException), () =>
            {
                var target = AppDomainContext.Create();
                target.Dispose();
                
                Assert.True(target.IsDisposed);
                var resolver = target.RemoteResolver;
            });
        }
        
        #endregion
        
        #region FindByCodeBase
        
        [Fact]
        public void FindByCodeBase_NullArgument()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    context.FindByCodeBase(null);
                }
            });
        }
        
        [Fact]
        public void FindByCodeBase_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.NotNull(context.FindByCodeBase(codebaseUri));
            }
        }
        
        #endregion
        
        #region FindByFullName
        
        [Fact]
        public void FindByFullName_NullArgument()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    context.FindByFullName(null);
                }
            });
        }
        
        [Fact]
        public void FindByFullName_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var target = context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.NotNull(context.FindByFullName(target.FullName));
            }
        }
        
        #endregion
        
        #region FindByLocation
        
        [Fact]
        public void FindByLocation_NullArgument()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    context.FindByLocation(null);
                }
            });
        }
        
        [Fact]
        public void FindByLocation_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var target = context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.NotNull(context.FindByLocation(target.Location));
            }
        }
        
        #endregion
        
        #region LoadTarget
        
        [Fact]
        public void LoadTarget_NoRefAssembly_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                context.LoadTarget(LoadMethod.LoadFile, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));
                
                Assert.NotNull(actual);
                Assert.Equal(target.FullName, actual.FullName);
                Assert.Equal(target.Location, actual.Location);
                Assert.Equal(target.CodeBase, target.CodeBase);
            }
        }
        
        [Fact]
        public void LoadTarget_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                context.LoadTarget(LoadMethod.LoadFrom, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));
                
                Assert.NotNull(actual);
                Assert.Equal(target.FullName, actual.FullName);
                Assert.Equal(target.Location, actual.Location);
                Assert.Equal(target.CodeBase, target.CodeBase);
            }
        }
        
        [Fact]
        public void LoadTarget_NoRefAssembly_LoadBits()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                context.LoadTarget(LoadMethod.LoadBits, target);
                var actual = context.LoadedAssemblies.FirstOrDefault(x => x.FullName.Equals(target.FullName));
                
                Assert.NotNull(actual);
                Assert.Equal(target.FullName, actual.FullName);
                Assert.Equal(string.Empty, actual.Location);
                Assert.Equal(target.CodeBase, target.CodeBase);
            }
        }
        
        #endregion
        
        #region LoadTargetWithReferences
        
        [Fact]
        public void LoadTargetWithReferences_InternalReferences_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                var targets = context.LoadTargetWithReferences(LoadMethod.LoadFile, target);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        [Fact]
        public void LoadTargetWithReferences_InternalReferences_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                var targets = context.LoadTargetWithReferences(LoadMethod.LoadFrom, target);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        [Fact]
        public void LoadTargetWithReferences_InternalReferences_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var assembly = Assembly.LoadFile(targetPath);
                var target = AssemblyTarget.FromAssembly(assembly);
                
                var targets = context.LoadTargetWithReferences(LoadMethod.LoadBits, target);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        #endregion
        
        #region LoadAssembly
        
        [Fact]
        public void LoadAssembly_NoRefAssembly_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFile, targetPath);
                Assert.True(context.LoadedAssemblies.Any(x => x.CodeBase.Equals(codebaseUri.ToString())));
            }
        }
        
        [Fact]
        public void LoadAssembly_NoRefAssembly_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                context.LoadAssembly(LoadMethod.LoadFrom, targetPath);
                Assert.True(context.LoadedAssemblies.Any(x => x.CodeBase.Equals(codebaseUri.ToString())));
            }
        }
        
        [Fact]
        public void LoadAssembly_NoRefAssembly_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath);
                Assert.True(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }
        
        [Fact]
        public void LoadAssembly_NoRefAssembly_LoadBitsPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath, Path.ChangeExtension(targetPath, "pdb"));
                Assert.True(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }
        
        [Fact]
        public void LoadAssembly_NoRefAssembly_LoadBitsWrongPdbPathSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var targetPath = Path.GetFullPath(NoRefsAssemblyPath);
                var pdbPath = Path.ChangeExtension(Path.Combine(TestAssemblyDir, Guid.NewGuid().ToString(), NoRefsAssemblyFileName), "pdb");
                var codebaseUri = new Uri(targetPath);
                var target = context.LoadAssembly(LoadMethod.LoadBits, targetPath, Path.GetFullPath(pdbPath));
                Assert.True(context.LoadedAssemblies.Any(x => x.FullName.Equals(target.FullName)));
            }
        }
        
        #endregion
        
        #region LoadAssemblyWithReferences
        
        [Fact]
        public void LoadAssemblyWithReferences_InternalReferences_LoadFile()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadFile, targetPath);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        [Fact]
        public void LoadAssemblyWithReferences_InternalReferences_LoadFrom()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadFrom, targetPath);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.Location.Equals(targetPath)));
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        [Fact]
        public void LoadAssemblyWithReferences_InternalReferences_LoadBitsNoPdbSpecified()
        {
            using (var context = AppDomainContext.Create())
            {
                var prevNumAssemblies = context.LoadedAssemblies.Count();
                
                // Add the correct resolver path for the test dir.
                context.RemoteResolver.AddProbePath(Path.GetFullPath(InternalRefsAssemblyDir));
                var targetPath = Path.GetFullPath(InternalRefsAssemblyPath);
                var targets = context.LoadAssemblyWithReferences(LoadMethod.LoadBits, targetPath);
                
                Assert.True(context.LoadedAssemblies.Count() > prevNumAssemblies);
                Assert.True(targets.Any(x => x.FullName.Contains(InternalRefsAssemblyName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyAName)));
                Assert.True(targets.Any(x => x.FullName.Contains(AssemblyBName)));
            }
        }
        
        #endregion
        
        #endregion
    }
}