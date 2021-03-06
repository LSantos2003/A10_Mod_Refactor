using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class CustomLandingGear : MonoBehaviour
    {
        public AnimationToggle animToggle;
        public GearAnimator gearAnim;
        public VRLever gearLever;
        private void Start()
        {
           
            //this.gearLever.OnSetState.AddListener(this.onGearLever);
            this.gearAnim.OnOpen.AddListener(animToggle.Retract);
            this.gearAnim.OnClose.AddListener(animToggle.Deploy);
        }


        public void onGearLever(int state)
        {
            if(state == 0)
            {
                animToggle.Retract();
                return;
            }

            animToggle.Deploy();
        }


    }
}
