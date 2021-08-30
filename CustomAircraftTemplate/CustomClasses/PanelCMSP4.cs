using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class PanelCMSP4 : PanelText
    {
        public AutoCMS autoCms;

        private int currentPage = 1;
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
            string program = this.autoCms.GetCurrentProgram();

            return "      " + program + "\nPROG";

        }

        private string PageTwo()
        {
            string program = "10";
            return ""; //Uncomment here to set up a new display on page 2
            return "   " + program + "\nCYCL";
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

    }
}
