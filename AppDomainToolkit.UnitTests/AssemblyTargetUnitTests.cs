namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Xunit;

    public class AssemblyTargetUnitTests
    {
        #region Test Methods

        #region FromAssembly

        [Fact]
        public void FromAssembly_CurrentAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var target = AssemblyTarget.FromAssembly(assembly);

            Assert.NotNull(target);
            Assert.Equal(assembly.CodeBase, target.CodeBase.ToString());
            Assert.Equal(assembly.Location, target.Location);
            Assert.Equal(assembly.FullName, target.FullName);
        }

        [Fact]
        public void FromAssembly_NullArgument()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var target = AssemblyTarget.FromAssembly(null);
            });
        }

        #endregion

        #region FromPath

        [Fact]
        public void FromPath_NullCodebase()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var target = AssemblyTarget.FromPath(null);
            });
        }

        [Fact]
        public void FromPath_NonExistingLocationExistingCodeBase()
        {
            Assert.Throws(typeof(FileNotFoundException), () =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var location = string.Format("{0}/{1}", Guid.NewGuid().ToString(), Path.GetRandomFileName());
                var target = AssemblyTarget.FromPath(new Uri(assembly.CodeBase), location);
            });
        }

        [Fact]
        public void FromPath_NonExistingCodeBase()
        {
            Assert.Throws(typeof(FileNotFoundException), () =>
            {
                var location = Path.GetFullPath(string.Format("{0}/{1}", Guid.NewGuid().ToString(), Path.GetRandomFileName()));
                var target = AssemblyTarget.FromPath(new Uri(location));
            });
        }

        [Fact]
        public void FromPath_CurrentAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var target = AssemblyTarget.FromPath(new Uri(assembly.CodeBase), assembly.Location, assembly.FullName);

            Assert.NotNull(target);
            Assert.Equal(assembly.CodeBase, target.CodeBase.ToString());
            Assert.Equal(assembly.Location, target.Location);
            Assert.Equal(assembly.FullName, target.FullName);
        }

        #endregion

        #endregion
    }
}