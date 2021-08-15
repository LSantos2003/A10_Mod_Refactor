using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{
    class DashAPUTemp : DashGauge
    {

        public AuxilliaryPowerUnit apu;
        public Health apuHealth;

        protected override float GetMeteredValue()
        {
            //Returns the max heat if apu is destroyed
            if(apuHealth.currentHealth <= 0)
            {
                return 1;
            }

            //Returns heat that is lower than rpm so they don't match exactly
            //Realism :tm:
            return apu.rpm / 1.8f;
        }







    }
}