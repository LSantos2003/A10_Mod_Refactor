using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace A10Mod
{
    class DashEngineFlow : DashGauge
    {
        public ModuleEngine engineToMeasure;
        public ModuleEngine engineToCheck;
        public FuelTank tank;


        protected override float GetMeteredValue()
        {
            if (engineToMeasure.startedUp && engineToCheck.startedUp)
            {
                return tank.fuelDrain / 2;
            }
            else if (engineToMeasure.startedUp)
            {
                return tank.fuelDrain;
            }

            return 0;

        }



    }
}