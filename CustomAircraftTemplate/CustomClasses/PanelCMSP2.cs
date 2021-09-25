using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class PanelCMSP2 : PanelText
    {
        public FlareCountermeasure flares;
        public AutoCMS autoCms;

        private int currentPage = 1;
        private int flareQuantity = 1;

        private void Start()
        {
            this.autoCms.SetFlareRelase(flareQuantity);
            this.autoCms.OnModeSwitch.AddListener(ModeSwitched);
        }

        protected override void UpdateText()
        {
            switch (this.currentPage)
            {
                case 1:
                    SetText(PageOne());
                    break;
                case 2:
                    SetText(PageTwo());
                    break;
            }

        }

        private string PageOne()
        {
            string flareCount = flares.count.ToString("D3");

            return "  " + flareCount + "\nFLAR";

        }

        private string PageTwo()
        {
            string bundleCount = flareQuantity.ToString("D3");

            return "  " + bundleCount + "\nFLAR";
        }

        public void SetPage(int state)
        {
            if (state == 2)
            {
                currentPage = 2;
                return;
            }
            currentPage = 1;
        }

        private void ModeSwitched()
        {
            if (this.autoCms.GetCurrentMode() == AutoCMS.Mode.MANUAL)
            {
                this.autoCms.SetFlareRelase(flareQuantity);
            }
        }

        public void IncrementFlareQuantity()
        {
            if (currentPage != 2 || this.autoCms.GetCurrentMode() == AutoCMS.Mode.AUTO || this.autoCms.GetCurrentMode() == AutoCMS.Mode.SEMI) return;

            flareQuantity++;
            if (flareQuantity > 2)
            {
                flareQuantity = 0;
            }

            autoCms.SetFlareRelase(flareQuantity);
        }
    }
}
