namespace AppDomainToolkit.UnitTests
{
    using System;
    using Xunit;


    public class RemoteFuncUnitTests
    {
        #region Test Methods

        [Fact]
        public void Invoke_NullDomain()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 RemoteFunc.Invoke(null, () => { return 1; });
             });
        }

        [Fact]
        public void Invoke_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 using (var context = AppDomainContext.Create())
                 {
                     var actual = RemoteFunc.Invoke<int>(context.Domain, null);
                 }
             });
        }

        [Fact]
        public void Invoke_EmptyFunction()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = RemoteFunc.Invoke(context.Domain, () => { return 1; });
                Assert.Equal(1, actual);
            }
        }

        [Fact]
        public void Invoke_InstanceOneType_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 var action = new RemoteFunc<int>();
                 action.Invoke(null);
             });
        }

        [Fact]
        public void Invoke_InstanceTwoTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 var action = new RemoteFunc<int, int>();
                 action.Invoke(1, null);
             });
        }

        [Fact]
        public void Invoke_InstanceThreeTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 var action = new RemoteFunc<int, int, int>();
                 action.Invoke(1, 2, null);
             });
        }

        [Fact]
        public void Invoke_InstanceFourTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 var action = new RemoteFunc<int, int, int, int>();
                 action.Invoke(1, 2, 3, null);
             });
        }

        [Fact]
        public void Invoke_InstanceFiveTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 var action = new RemoteFunc<int, int, int, int, int>();
                 action.Invoke(1, 2, 3, 4, null);
             });
        }

        [Fact]
        public void Invoke_Serializable_NoArguments()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    () =>
                    {
                        return new Test() { Value1 = 10 };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public void Invoke_Serializable_OneArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (value) =>
                    {
                        return new Test() { Value1 = value };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public void Invoke_Serializable_TwoArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = RemoteFunc.Invoke(
                    context.Domain,
                    10,
                    (short)11,
                    (value1, value2) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2 };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
            }
        }

        [Fact]
        public void Invoke_Serializable_ThreeArg()
        {
            using (var context = AppDomainContext.Create())
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

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
                Assert.Equal(12, actual.Value3);
            }
        }

        [Fact]
        public void Invoke_Serializable_FourArg()
        {
            using (var context = AppDomainContext.Create())
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

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
                Assert.Equal(12, actual.Value3);

                Assert.NotNull(actual.Value4);
                Assert.Equal(13, actual.Value4.Value);
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
