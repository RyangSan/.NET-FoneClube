using FoneClube.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class StatusAccessTest
    {
        [TestMethod]
        public void CanGetSystemMemory()
        {
            var statusMemory = new StatusAPIAccess().GetSystemInfo();
        }

        [TestMethod]
        public void CanGetPCU()
        {
            var getCPUCounter = new StatusAPIAccess().getCPUCounter();
        }
    }
}
