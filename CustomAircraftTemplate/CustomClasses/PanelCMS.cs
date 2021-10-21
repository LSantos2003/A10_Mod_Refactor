using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace A10Mod
{
    public class PanelCMS : PanelText
    {
        public ChaffCountermeasure chaff;
        public FlareCountermeasure flares;
        public AutoCMS autoCms;

        protected override void UpdateText()
        {


            SetText(GenerateText());

        }

        private string GenerateText()
        {
            string cmProgram = this.autoCms.GetCurrentProgram();
            string chaffCounter = chaff.count.ToString("D3");

            string cmMode = " ";
            switch (this.autoCms.GetCurrentMode())
            {
                case AutoCMS.Mode.AUTO:
                    cmMode = "A";
                    break;
                case AutoCMS.Mode.SEMI:
                    cmMode = "S";
                    break;
                case AutoCMS.Mode.MANUAL:
                    cmMode = "M";
                    break;

            }
      
            string flareCounter = flares.count.ToString("D3");

            return cmProgram + chaffCounter + cmMode + flareCounter;
        }

    }

}
