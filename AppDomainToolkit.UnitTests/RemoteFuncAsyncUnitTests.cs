namespace AppDomainToolkit.UnitTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AppDomainToolkit;
    using Xunit;

    public class RFuncAsync
    {
        [Fact]
        public async Task NullDomain()
        {
            await Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await RemoteFuncAsync.InvokeAsync(null, async () =>
                {
                    return 1;
                });
            });
        }

        [Fact]
        public async Task NullFunction()
        {
            await Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    var actual = await RemoteFuncAsync.InvokeAsync<int>(context.Domain, null);
                }
            });
        }

        [Fact]
        public async Task EmptyFunction()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(context.Domain, async () => { return 1; });
                Assert.Equal(1, actual);
            }
        }

        [Fact]
        public void InstanceOneType_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteFuncAsync<int>();
                action.Invoke(null, null);
            });
        }

        [Fact]
        public void InstanceTwoTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteFuncAsync<int, int>();
                action.Invoke(1, null, null);
            });
        }

        [Fact]
        public void InstanceThreeTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteFuncAsync<int, int, int>();
                action.Invoke(1, 2, null, null);
            });
        }

        [Fact]
        public void InstanceFourTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                var action = new RemoteFuncAsync<int, int, int, int>();
                action.Invoke(1, 2, 3, null, null);
            });
        }

        [Fact]
        public void InstanceFiveTypes_NullFunction()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                try
                {
                    var action = new RemoteFuncAsync<int, int, int, int, int>();
                    action.Invoke(1, 2, 3, 4, null, null);
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [Fact]
        public async Task Serializable_NoArguments()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    async () =>
                    {
                        return new Test() { Value1 = 10 };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public async Task Delay_Short()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    async (value) =>
                    {
                        await Task.Delay(100);
                        return new Test() { Value1 = value };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public async Task Delay_Long()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    async (value) =>
                    {
                        await Task.Delay(1000);
                        return new Test() { Value1 = value };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public async Task TaskYields()
        {
            using (var context = AppDomainContext.Create())
            {
                await Task.Yield();
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    async (value) =>
                    {
                        await Task.Yield();
                        var test = new Test() { Value1 = value };
                        await Task.Yield();
                        return test;
                    });
                await Task.Yield();
                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public async Task Serializable_OneArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    async (value) =>
                    {
                        return new Test() { Value1 = value };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
            }
        }

        [Fact]
        public async Task Serializable_TwoArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    (short)11,
                    async (value1, value2) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2 };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
            }
        }

        [Fact]
        public async Task Serializable_ThreeArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    (short)11,
                    new Double?(12),
                    async (value1, value2, value3) =>
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
        public async Task Serializable_FourArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    (short)11,
                    new Double?(12.0),
                    new Composite() { Value = 13 },
                    async (value1, value2, value3, value4) =>
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

        [Fact]
        public async Task Serializable_FiveArg()
        {
            using (var context = AppDomainContext.Create())
            {
                var actual = await RemoteFuncAsync.InvokeAsync(
                    context.Domain,
                    10,
                    (short)11,
                    new Double?(12.0),
                    new Composite() { Value = 13 },
                    "Last",
                    async (value1, value2, value3, value4, value5) =>
                    {
                        return new Test() { Value1 = value1, Value2 = value2, Value3 = value3, Value4 = value4, Value5 = value5 };
                    });

                Assert.NotNull(actual);
                Assert.Equal(10, actual.Value1);
                Assert.Equal(11, actual.Value2);
                Assert.Equal(12, actual.Value3);
                Assert.NotNull(actual.Value4);
                Assert.Equal("Last", actual.Value5);
                Assert.Equal(13, actual.Value4.Value);
            }
        }

        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync(typeof(Exception), async () =>
            {
                using (var context = AppDomainContext.Create())
                {
                    Test actual = null;

                    CancellationTokenSource tcs = new CancellationTokenSource();
                    actual = await RemoteFuncAsync.InvokeAsync(
                        context.Domain,
                        10,
                        (short)11,
                        new Double?(12.0),
                        new Composite() { Value = 13 },
                        "Last",
                        async (value1, value2, value3, value4, value5) =>
                        {
                            if (value5 == "Last")
                                throw new Exception("");
                            return new Test() { Value1 = value1, Value2 = value2, Value3 = value3, Value4 = value4, Value5 = value5 };
                        });

                    Assert.NotNull(actual);
                    Assert.Equal(10, actual.Value1);
                    Assert.Equal(11, actual.Value2);
                    Assert.Equal(12, actual.Value3);
                    Assert.NotNull(actual.Value4);
                    Assert.Equal("Last", actual.Value5);
                    Assert.Equal(13, actual.Value4.Value);
                }
            });
        }

        [Serializable]
        private class Test
        {
            public Test()
            {
            }

            public int Value1 { get; set; }

            public short Value2 { get; set; }

            public Double? Value3 { get; set; }

            public Composite Value4 { get; set; }

            public String Value5 { get; set; }
        }

        [Serializable]
        private class Composite
        {
            public Composite()
            {
            }

            public short Value { get; set; }
        }
    }
}