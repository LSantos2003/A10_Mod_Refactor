using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class PanelCMSP3 : PanelText
    {
        public AutoCMS autoCms;

        private int currentPage = 1;

        private float autoReleaseInterval = 0.25f;

        private void Start()
        {
            this.autoCms.SetInterval(autoReleaseInterval);
            this.autoCms.OnModeSwitch.AddListener(this.ModeSwitched);
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
            string cmCount = "000";

            return "  " + cmCount + "\nOTR1";

        }

        private string PageTwo()
        {
            string program = autoReleaseInterval.ToString("F2");

            return "  " + program + "\nINTV";
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
                this.autoCms.SetInterval(autoReleaseInterval);
            }
        }

        public void IncrementReleaseInterval()
        {
            if (currentPage != 2 || this.autoCms.GetCurrentMode() == AutoCMS.Mode.AUTO || this.autoCms.GetCurrentMode() == AutoCMS.Mode.SEMI) return;

            autoReleaseInterval += 0.25f;
            if (autoReleaseInterval > 2)
            {
                autoReleaseInterval = 0.25f;
            }

            autoCms.SetInterval(autoReleaseInterval);
        }
    }
}
