using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10Mod
{
    class DashAccelGauge : DashGauge
    {
        protected override float GetMeteredValue()
        {


            return this.info.playerGs;
        }

        public FlightInfo info;


    }
}
