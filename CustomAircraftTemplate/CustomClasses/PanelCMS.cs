using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10Mod
{
    public class PanelCMS : PanelText
    {
        public ChaffCountermeasure chaff;
        public FlareCountermeasure flares;

        protected override void UpdateText()
        {
            SetText(GenerateText());

        }

        private string GenerateText()
        {
            string cmProgram = "D";
            string chaffCounter = chaff.count.ToString("D3");
            string cmMode = "S";
            string flareCounter = flares.count.ToString("D3");

            return cmProgram + chaffCounter + cmMode + flareCounter;
        }

    }

}
