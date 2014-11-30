namespace AppDomainToolkit.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RemoteFuncUnitTests
    {
        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_NullDomain()
        {
            RemoteFunc.Invoke(null, () => { return 1; });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_NullFunction()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke<int>(context.Domain, null);
            }
        }

        [TestMethod]
        public void Invoke_EmptyFunction()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(context.Domain, () => { return 1; });
                Assert.AreEqual(1, actual);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceOneType_NullFunction()
        {
            var action = new RemoteFunc<int>();
            action.Invoke(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceTwoTypes_NullFunction()
        {
            var action = new RemoteFunc<int, int>();
            action.Invoke(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceThreeTypes_NullFunction()
        {
            var action = new RemoteFunc<int, int, int>();
            action.Invoke(1, 2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceFourTypes_NullFunction()
        {
            var action = new RemoteFunc<int, int, int, int>();
            action.Invoke(1, 2, 3, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceFiveTypes_NullFunction()
        {
            var action = new RemoteFunc<int, int, int, int, int>();
            action.Invoke(1, 2, 3, 4, null);
        }

        [TestMethod]
        public void Invoke_Serializable_NoArguments()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    () =>
                    {
                        return new Test() { Value1 = 10 };
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
            }
        }

        [TestMethod]
        public void Invoke_Serializable_OneArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (value) =>
                    {
                        return new Test() { Value1 = value };
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
            }
        }

        [TestMethod]
        public void Invoke_Serializable_TwoArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (short)11,
                    (value1, value2) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2 };
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
            }
        }

        [TestMethod]
        public void Invoke_Serializable_ThreeArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (short)11,
                    new Double?(12),
                    (value1, value2, value3) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2, Value3 = value3 };
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
                Assert.AreEqual(12, actual.Value3);
            }
        }

        [TestMethod]
        public void Invoke_Serializable_FourArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (short)11,
                    new Double?(12.0),
                    new Composite() { Value = 13 },
                    (value1, value2, value3, value4) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2, Value3 = value3, Value4 = value4 };
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
                Assert.AreEqual(12, actual.Value3);

                Assert.IsNotNull(actual.Value4);
                Assert.AreEqual(13, actual.Value4.Value);
            }
        }

        #endregion

        #region Inner Classes

        [Serializable]
        class Test
        {
            #region Constructors & Destructors

            public Test()
            {
            }

            #endregion

            #region Properties

            public int Value1 { get; set; }

            public short Value2 { get; set; }

            public Double? Value3 { get; set; }

            public Composite Value4 { get; set; }

            #endregion
        }

        [Serializable]
        class Composite
        {
            #region Constructors & Destructors

            public Composite()
            {
            }

            #endregion

            #region Properties

            public short Value { get; set; }

            #endregion
        }

        #endregion
    }
}
