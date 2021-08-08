using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10Mod
{

    class DashAOAGauge : DashGauge
    {


        protected override float GetMeteredValue()
        {
            if (this.info.surfaceSpeed >= 0.01)
            {
                return -this.info.aoa;
            }

            return 0;
        }

        public FlightInfo info;



    }
    
}
