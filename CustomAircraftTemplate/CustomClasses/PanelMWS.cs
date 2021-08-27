using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    public class PanelMWS : PanelText
    {
        public MissileDetector detector;


        protected override void UpdateText()
        {
       

            if (!detector.enabled)
            {
                SetText("OFF");
                return;
            }

            if(detector.enabled && detector.missileIncomingDetected)
            {
                SetText("LAUNCH");
                return;
            }

            SetText("ACTIVE");
        }

    }
}
