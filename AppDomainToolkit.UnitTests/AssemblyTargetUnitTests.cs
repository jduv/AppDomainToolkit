namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;

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
        public void FromPath_CurrentAssembly()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var target = AssemblyTarget.FromPath(assemblyPath);

            Assert.IsNotNull(target);
            Assert.AreEqual(assemblyPath, target.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromPath_NullArgument()
        {
            var target = AssemblyTarget.FromPath(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromPath_NonExistingPath()
        {
            var assemblyPath = Guid.NewGuid().ToString() + "/" + Path.GetRandomFileName();
            var target = AssemblyTarget.FromPath(assemblyPath);
        }

        #endregion

        #endregion
    }
}
