namespace AppDomainToolkit.UnitTests
{
    using System;
    using Xunit;

    public class RemoteActionUnitTests
    {
        #region Test Methods
        
        [Fact]
        public void Invoke_NullDomain()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                RemoteAction.Invoke(null, () => { });
            });
        }
        
        [Fact]
        public void Invoke_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    RemoteAction.Invoke(context.Domain, null);
                }
            });
        }
        
        [Fact]
        public void Invoke_InstanceNoTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteAction();
                action.Invoke(null);
            });
        }
        
        [Fact]
        public void Invoke_InstanceOneType_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteAction<int>();
                action.Invoke(1, null);
            });
        }
        
        [Fact]
        public void Invoke_InstanceTwoTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteAction<int, int>();
                action.Invoke(1, 2, null);
            });
        }
        
        [Fact]
        public void Invoke_InstanceThreeTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteAction<int, int, int>();
                action.Invoke(1, 2, 3, null);
            });
        }
        
        [Fact]
        public void Invoke_InstanceFourTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteAction<int, int, int, int>();
                action.Invoke(1, 2, 3, 4, null);
            });
        }
        
        [Fact]
        public void Invoke_MarshalObjectByRef_NoArguments()
        {
            using (var context = AppDomainContext.Create())
            {
                RemoteAction.Invoke(context.Domain, () => { });
            }
        }
        
        [Fact]
        public void Invoke_MarshalObjectByRef_OneArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = new Test();
                RemoteAction.Invoke(
                    context.Domain,
                    actual,
                    (test) =>
                    {
                        test.Value1 = 10;
                    });
                
                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }
        
        [Fact]
        public void Invoke_MarshalObjectByRef_TwoArg()
        {
            using (var context = AppDomainContext.Create())
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
                
                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
            }
        }
        
        [Fact]
        public void Invoke_MarshalObjectByRef_ThreeArg()
        {
            using (var context = AppDomainContext.Create())
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
                
                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
                Assert.Equal(12.0, actual.Value3);
            }
        }
        
        [Fact]
        public void Invoke_MarshalObjectByRef_FourArg()
        {
            using (var context = AppDomainContext.Create())
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
                
                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
                Assert.Equal(12.0, actual.Value3);
                
                Assert.NotNull(actual.Value4);
                Assert.Equal(13, actual.Value4.Value);
            }
        }
        
        #endregion
        
        #region Inner Classes
        
        private class Test : MarshalByRefObject
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
        
        private class Composite : MarshalByRefObject
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