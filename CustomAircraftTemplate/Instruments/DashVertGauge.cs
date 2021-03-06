using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class DashVertGauge : DashGauge
    {

        public MeasurementManager measures;
        public FlightInfo info;

        protected override float GetMeteredValue()
        {

            //DebugDebugFlightLogger.Log(measures.ConvertedVerticalSpeed(info.verticalSpeed).ToString());
            return measures.ConvertedVerticalSpeed(info.verticalSpeed) / 1000f;
        }



    }
}
