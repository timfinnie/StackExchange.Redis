﻿using System.Diagnostics;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace StackExchange.Redis.Tests
{
    public class ConnectToUnexistingHost : TestBase
    {
        public ConnectToUnexistingHost(ITestOutputHelper output) : base (output) { }

#if DEBUG
        [Fact]
        public void FailsWithinTimeout()
        {
            const int timeout = 1000;
            var sw = Stopwatch.StartNew();
            try
            {
                var config = new ConfigurationOptions
                {
                    EndPoints = { { "invalid", 1234 } },
                    ConnectTimeout = timeout
                };
                
                using (var muxer = ConnectionMultiplexer.Connect(config, Writer))
                {
                    Thread.Sleep(10000);
                }

                Assert.True(false, "Connect should fail with RedisConnectionException exception");
            }
            catch (RedisConnectionException)
            {
                var elapsed = sw.ElapsedMilliseconds;
                Output.WriteLine("Elapsed time: " + elapsed);
                Output.WriteLine("Timeout: " + timeout);
                Assert.True(elapsed < 9000, "Connect should fail within ConnectTimeout, ElapsedMs: " + elapsed);
            }
        }
#endif
    }
}
