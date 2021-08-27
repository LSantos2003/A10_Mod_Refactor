using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class AutoCMS : MonoBehaviour
    {
        public FlareCountermeasure flares;
        public ChaffCountermeasure chaff;
        public MissileDetector mws;

        private bool active = true;
        private float cmsTimer = 0;
        private float autoReleaseTime = 0.5f;
        private void Update()
        {
            if (this.active)
            {
                this.cmsTimer += Time.deltaTime;
                if(this.mws.missileIncomingDetected && cmsTimer > autoReleaseTime)
                {
                    this.chaff.FireCM();
                    this.flares.FireCM();

                    this.cmsTimer = 0;
                }
            }

        }

        public void ToggleAutoCMS(int state)
        {

        }
    }
}
