using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercell.Cloudia.AuditLogging;

namespace Mercell.Cloudia.AuditLogging.Tests
{
    [TestClass]
    public class AuditLoggerContextTest
    {
        [TestCleanup]
        public void TestCleanup()
        {
            // We need to do this because the
            // Parallel.Invoke runs the first task in the main thread
            AuditLoggerContext.Clear();
        }

        /// <summary>
        /// Test that each new thread gets new context.
        /// </summary>
        [TestMethod]
        public void TestMultipleThreads()
        {
            static void action()
            {
                Assert.AreEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);
                AuditLoggerContext.Properties.Ssid = Guid.NewGuid();
            }

            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 3 };
            Parallel.Invoke(parallelOptions, action, action, action);
        }

        /// <summary>
        /// Test that the context persists within single thread.
        /// </summary>
        [TestMethod]
        public void TestSingleThread()
        {
            static void action()
            {
                Assert.AreNotEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);
                AuditLoggerContext.Properties.Ssid = Guid.NewGuid();
            }

            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 1 };
            // Parallel.Invoke uses the main-thread if parallelism is set to 1
            // so set the ssid here to some Guid value.
            AuditLoggerContext.Properties.Ssid = Guid.NewGuid();
            Parallel.Invoke(parallelOptions, action, action, action);
        }

        /// <summary>
        /// Test that clearing and initializing context works.
        /// </summary>
        [TestMethod]
        public void TestContextClearing()
        {
            Assert.AreEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);

            AuditLoggerContext.Properties.Ssid = Guid.NewGuid();
            Assert.AreNotEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);
            AuditLoggerContext.Clear();
            Assert.AreEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);

            AuditLoggerContext.Properties.Ssid = Guid.NewGuid();
            Assert.AreNotEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);
            AuditLoggerContext.Initialize();
            Assert.AreEqual(Guid.Empty, AuditLoggerContext.Properties.Ssid);
        }
    }
}
