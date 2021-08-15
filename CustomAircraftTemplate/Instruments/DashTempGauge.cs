using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{
    class DashTempGauge : DashGauge
    {

        public HeatEmitter heatEmitter;
        public Health engineHealth;
        protected override float GetMeteredValue()
        {
            if(engineHealth.currentHealth <= 0)
            {
                return this.maxValue;
            }
            //DebugFlightLogger.Log(this.heatEmitter.heat.ToString());
            return this.heatEmitter.heat;
        }


    }
}