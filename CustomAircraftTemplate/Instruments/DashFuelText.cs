using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace A10Mod
{
    class DashFuelText : MonoBehaviour
    {
        public TextMeshPro text;
        public FuelTank tank;
        public DashFuelNumGauge gauge;


        public void Update()
        {
            switch (this.gauge.GetFuelDisplay())
            {
                case DashFuelNumGauge.FuelDisplay.Total:
                    this.text.text = Mathf.RoundToInt(this.tank.totalFuel).ToString("D5");
                    break;

                case DashFuelNumGauge.FuelDisplay.Internal:
                    this.text.text = Mathf.RoundToInt(this.tank.fuel).ToString("D5");
                    break;

                case DashFuelNumGauge.FuelDisplay.External:
                    float totalFuel = 0;
                    float maxFuel = -1;
                    this.tank.GetSubFuelInfo(out totalFuel, out maxFuel);
                    this.text.text = Mathf.RoundToInt(totalFuel).ToString("D5");
                    break;
                default:
                    break;
            }
        }



    }
}
