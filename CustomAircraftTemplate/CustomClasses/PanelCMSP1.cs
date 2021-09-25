using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class PanelCMSP1 : PanelText
    {
        public ChaffCountermeasure chaff;
        public AutoCMS autoCms;

        private int currentPage = 1;
        private int chaffQuantity = 1;

        private void Start()
        {
            this.autoCms.SetChaffRelease(chaffQuantity);
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
            string chaffCount = chaff.count.ToString("D3");

            return "  " + chaffCount + "\nCHAF";

        }

        private string PageTwo()
        {
            string bundleCount = chaffQuantity.ToString("D3");

            return "  " + bundleCount + "\nCHAF";
        }

        public void SetPage(int state)
        {
            if(state == 2)
            {
                currentPage = 2;
                return;
            }
            currentPage = 1;
        }

        private void ModeSwitched()
        {
            if(this.autoCms.GetCurrentMode() == AutoCMS.Mode.MANUAL)
            {
                this.autoCms.SetChaffRelease(chaffQuantity);
            }
        }
        public void IncrementChaffQuantity()
        {
            if (currentPage != 2 || this.autoCms.GetCurrentMode() == AutoCMS.Mode.AUTO || this.autoCms.GetCurrentMode() == AutoCMS.Mode.SEMI) return;

            chaffQuantity++;
            if(chaffQuantity > 2)
            {
                chaffQuantity = 0;
            }
            this.autoCms.SetChaffRelease(chaffQuantity);
        }
    }
}
