using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class PanelLock : PanelText
    {
        public ModuleRWR rwr;
        private List<ModuleRWR.RWRContact> lockedOnThreats = new List<ModuleRWR.RWRContact>();

        private void Start()
        {
            this.lockedOnThreats = new List<ModuleRWR.RWRContact>();
        }


        protected override void UpdateText()
        {

            int airCount = 0;
            int groundCount = 0;
            int missileCount = 0;
            foreach(Actor a in this.rwr.GetDetectedActors(TeamOptions.OtherTeam))
            {
                switch (a.role)
                {
                    case Actor.Roles.Air:
                        airCount++;
                        break;
                    case Actor.Roles.Ground:
                    case Actor.Roles.GroundArmor:
                    case Actor.Roles.Ship:
                        groundCount++;
                        break;
                    case Actor.Roles.Missile:
                        missileCount++;
                        break;
                    default:
                        break;
                }
            }

            if (this.rwr.isMissileLocked)
            {
                SetText("MSSL: " + missileCount.ToString("D2"));
                return;
            }

            SetText($"G:{groundCount.ToString("D2")} A:{airCount.ToString("D2")}");
          
        }
    }
}
