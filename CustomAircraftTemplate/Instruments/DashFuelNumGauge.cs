using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{
    class DashFuelNumGauge : DashGauge
    {

        public FuelTank tank;

        private FuelDisplay currentDisplay = FuelDisplay.Total;

        public FuelDisplay GetFuelDisplay()
        {
            return this.currentDisplay;
        }


        protected override float GetMeteredValue()
        {

            switch (this.currentDisplay)
            {
                case FuelDisplay.Total:
                    return this.tank.totalFuel / 2000f;

                case FuelDisplay.Internal:
                    return this.tank.fuel / 2000f;

                case FuelDisplay.External:
                    float totalFuel = 0;
                    float maxFuel = -1;
                    this.tank.GetSubFuelInfo(out totalFuel,out maxFuel);
                    return totalFuel / 2000f;
                default:
                    return 0;
            }



        }

        public void ChangeDisplayValue(int state)
        {
            switch (state)
            {
                case 0:
                    this.currentDisplay = FuelDisplay.External;
                    break;
                case 1:
                    this.currentDisplay = FuelDisplay.Internal;
                    break;
                case 2:
                    this.currentDisplay = FuelDisplay.Total;
                    break;
                default:
                    break;

            }


        }

        public enum FuelDisplay
        {
            Total,
            Internal,
            External
        }




    }
}