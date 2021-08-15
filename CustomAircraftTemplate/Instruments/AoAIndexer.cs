using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class AoAIndexer : MonoBehaviour
    {



        private void Update()
        {
            this.currentState = determineState(flightInfo.aoa);


            if (this.gear.state == GearAnimator.GearStates.Extended && this.battery.connected)
            {
                //sets emission mult to 1 cause for some reason restarting the mission sets it to 0
                this.aoaUpText.emissionMult = 1;
                this.aoaDownText.emissionMult = 1;
                this.aoaCenterText.emissionMult = 1;
                this.updateAOA(this.currentState);
            }
            else
            {
                //Turns off the aoa indicator, also overrides the rgb instrument lights mod
                this.aoaUpText.emissionMult = 1;
                this.aoaDownText.emissionMult = 1;
                this.aoaCenterText.emissionMult = 1;

                this.aoaUpText.color = Color.black;
                this.aoaCenterText.color = Color.black;
                this.aoaDownText.color = Color.black;
                this.aoaUpText.SetEmission(false);
                this.aoaCenterText.SetEmission(false);
                this.aoaDownText.SetEmission(false);
                this.aoaUpText.ApplyText();
                this.aoaCenterText.ApplyText();
                this.aoaDownText.ApplyText();
            }
        }
        private AOAStates determineState(float aoa)
        {
            if (aoa >= 9.4f)
            {
                return AOAStates.VERY_SLOW;
            }
            else if (aoa >= 8.6f && aoa < 9.4f)
            {
                return AOAStates.SLOW;
            }
            else if (aoa >= 7.8f && aoa < 8.6f)
            {
                return AOAStates.ON_SPEED;
            }
            else if (aoa >= 7f && aoa < 7.8f)
            {
                return AOAStates.FAST;
            }
            else if (aoa >= 0f && aoa < 7f)
            {
                return AOAStates.VERY_FAST;
            }

            return AOAStates.UNDETERMINED;




        }

        private void updateAOA(AOAStates state)
        {



            switch (state)
            {
                case AOAStates.VERY_SLOW:
                    this.aoaUpText.color = Color.yellow;
                    this.aoaCenterText.color = Color.black;
                    this.aoaDownText.color = Color.black;
                    this.aoaUpText.emission = Color.yellow;
                    this.aoaCenterText.emission = Color.black;
                    this.aoaDownText.emission = Color.black;
                    this.aoaUpText.SetEmission(true);
                    this.aoaCenterText.SetEmission(false);
                    this.aoaDownText.SetEmission(false);

                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();

                    break;
                case AOAStates.SLOW:
                    this.aoaUpText.color = Color.yellow;
                    this.aoaCenterText.color = Color.green;
                    this.aoaDownText.color = Color.black;
                    this.aoaUpText.emission = Color.yellow;
                    this.aoaCenterText.emission = Color.green;
                    this.aoaDownText.emission = Color.black;
                    this.aoaUpText.SetEmission(true);
                    this.aoaCenterText.SetEmission(true);
                    this.aoaDownText.SetEmission(false);
                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();

                    break;

                case AOAStates.ON_SPEED:
                    this.aoaUpText.color = Color.black;
                    this.aoaCenterText.color = Color.green;
                    this.aoaDownText.color = Color.black;
                    this.aoaUpText.emission = Color.black;
                    this.aoaCenterText.emission = Color.green;
                    this.aoaDownText.emission = Color.black;
                    this.aoaUpText.SetEmission(false);
                    this.aoaCenterText.SetEmission(true);
                    this.aoaDownText.SetEmission(false);
                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();
                    break;
                case AOAStates.FAST:
                    this.aoaUpText.color = Color.black;
                    this.aoaCenterText.color = Color.green;
                    this.aoaDownText.color = Color.yellow;
                    this.aoaUpText.emission = Color.black;
                    this.aoaCenterText.emission = Color.green;
                    this.aoaDownText.emission = Color.yellow;
                    this.aoaUpText.SetEmission(false);
                    this.aoaCenterText.SetEmission(true);
                    this.aoaDownText.SetEmission(true);
                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();
                    break;
                case AOAStates.VERY_FAST:
                    this.aoaUpText.color = Color.black;
                    this.aoaCenterText.color = Color.black;
                    this.aoaDownText.color = Color.yellow;
                    this.aoaUpText.emission = Color.black;
                    this.aoaCenterText.emission = Color.black;
                    this.aoaDownText.emission = Color.yellow;
                    this.aoaUpText.SetEmission(false);
                    this.aoaCenterText.SetEmission(false);
                    this.aoaDownText.SetEmission(true);
                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();
                    break;
                case AOAStates.UNDETERMINED:
                default:
                    this.aoaUpText.color = Color.black;
                    this.aoaCenterText.color = Color.black;
                    this.aoaDownText.color = Color.black;
                    this.aoaUpText.SetEmission(false);
                    this.aoaCenterText.SetEmission(false);
                    this.aoaDownText.SetEmission(false);
                    this.aoaUpText.ApplyText();
                    this.aoaCenterText.ApplyText();
                    this.aoaDownText.ApplyText();
                    break;
            }


        }

        public enum AOAStates
        {
            VERY_SLOW,
            SLOW,
            ON_SPEED,
            FAST,
            VERY_FAST,
            UNDETERMINED
        }

        public AOAStates currentState = AOAStates.UNDETERMINED;

        public FlightInfo flightInfo;

        public VTText aoaUpText;
        public VTText aoaCenterText;
        public VTText aoaDownText;

        public GearAnimator gear;
        public Battery battery;

        private Color orange = new Color(1, 0.6687184f, 0, 1);
    }
}