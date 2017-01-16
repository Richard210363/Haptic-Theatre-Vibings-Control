using Microsoft.VisualStudio.TestTools.UnitTesting;
using Haptic_Theatre_Vibings_Control.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haptic_Theatre_Vibings_Control.Classes.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void SwitchDisplayBySensorValueTest()
        {
            ModeChangeTriggering.SwitchDisplayBySensorValue(40);
        }
    }
}