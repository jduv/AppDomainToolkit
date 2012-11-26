namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyTargetUnitTests
    {
        #region Test Methods

        #region FromAssembly

        [TestMethod]
        public void FromAssembly_CurrentAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var target = AssemblyTarget.FromAssembly(assembly);

            Assert.IsNotNull(target);
            Assert.AreEqual(assembly.CodeBase, target.CodeBase.ToString());
            Assert.AreEqual(assembly.Location, target.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromAssembly_NullArgument()
        {
            var target = AssemblyTarget.FromAssembly(null);
        }

        #endregion

        #region FromPath

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromPath_NullArguments()
        {
            var target = AssemblyTarget.FromPath(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromPath_NonExistingCodeBase()
        {
            var location = Path.GetFullPath(Guid.NewGuid().ToString() + "/" + Path.GetRandomFileName());
            var target = AssemblyTarget.FromPath(new Uri(location), null, null);
        }

        [TestMethod]
        public void FromPath_CurrentAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var target = AssemblyTarget.FromPath(new Uri(assembly.CodeBase), assembly.Location, null);

            Assert.IsNotNull(target);
            Assert.AreEqual(assembly.CodeBase, target.CodeBase.ToString());
            Assert.AreEqual(assembly.Location, target.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromPath_NonExistingLocationExistingCodeBase()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var location = Guid.NewGuid().ToString() + "/" + Path.GetRandomFileName();
            var target = AssemblyTarget.FromPath(new Uri(assembly.CodeBase), location, null);
        }

        #endregion

        #endregion
    }
}
