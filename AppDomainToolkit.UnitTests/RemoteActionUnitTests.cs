namespace AppDomainToolkit.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RemoteActionUnitTests
    {
        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_NullDomain()
        {
            RemoteAction.Invoke(null, () => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_NullFunction()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                RemoteAction.Invoke(context.Domain, null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceNoTypes_NullFunction()
        {
            var action = new RemoteAction();
            action.Invoke(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceOneType_NullFunction()
        {
            var action = new RemoteAction<int>();
            action.Invoke(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceTwoTypes_NullFunction()
        {
            var action = new RemoteAction<int, int>();
            action.Invoke(1, 2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceThreeTypes_NullFunction()
        {
            var action = new RemoteAction<int, int, int>();
            action.Invoke(1, 2, 3, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_InstanceFourTypes_NullFunction()
        {
            var action = new RemoteAction<int, int, int, int>();
            action.Invoke(1, 2, 3, 4, null);
        }

        [TestMethod]
        public void Invoke_MarshalObjectByRef_NoArguments()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                RemoteAction.Invoke(context.Domain, () => {});
            }
        }

        [TestMethod]
        public void Invoke_MarshalObjectByRef_OneArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = new Test();
                RemoteAction.Invoke(
                    context.Domain,
                    actual,
                    (test) =>
                    {
                        test.Value1 = 10;
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
            }
        }

        [TestMethod]
        public void Invoke_MarshalObjectByRef_TwoArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = new Test();
                RemoteAction.Invoke(
                    context.Domain,
                    actual,
                    (short)11,
                    (test, value2) =>
                    {
                        test.Value1 = 10;
                        test.Value2 = value2;
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
            }
        }

        [TestMethod]
        public void Invoke_MarshalObjectByRef_ThreeArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = new Test();
                RemoteAction.Invoke(
                    context.Domain,
                    actual,
                    (short)11,
                    new Double?(12.0),
                    (test, value2, value3) =>
                    {
                        test.Value1 = 10;
                        test.Value2 = value2;
                        test.Value3 = value3;
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
                Assert.AreEqual(12.0, actual.Value3);
            }
        }

        [TestMethod]
        public void Invoke_MarshalObjectByRef_FourArg()
        {
            using (var context = AppDomainContext<AssemblyTargetLoader, PathBasedAssemblyResolver>.Create())
            {
                var actual = new Test();
                RemoteAction.Invoke(
                    context.Domain,
                    actual,
                    (short)11,
                    new Double?(12.0),
                    new Composite() { Value = 13 },
                    (test, value2, value3, value4) =>
                    {
                        test.Value1 = 10;
                        test.Value2 = value2;
                        test.Value3 = value3;
                        test.Value4 = value4;
                    });

                Assert.IsNotNull(actual);
                Assert.AreEqual(10, actual.Value1);
                Assert.AreEqual(11, actual.Value2);
                Assert.AreEqual(12.0, actual.Value3);

                Assert.IsNotNull(actual.Value4);
                Assert.AreEqual(13, actual.Value4.Value);
            }
        }

        #endregion

        #region Inner Classes

        class Test : MarshalByRefObject
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

        class Composite : MarshalByRefObject
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
