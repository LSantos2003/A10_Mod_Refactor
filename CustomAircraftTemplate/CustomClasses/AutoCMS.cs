using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace A10Mod
{
    public class AutoCMS : MonoBehaviour
    {
        public FlareCountermeasure flares;
        public ChaffCountermeasure chaff;
        public CountermeasureManager manager;
        public MissileDetector mws;
        public ModuleRWR rwr;

        public UnityEvent OnModeSwitch = new UnityEvent();

        private bool active = true;
        private bool cmsEnabled = false;
        private float cmsTimer = 0;
        private float autoReleaseTime = 0.5f;

        private RelaseMode ChaffReleaseMode = RelaseMode.Single;
        private RelaseMode FlareRelaseMode = RelaseMode.Single;
        private Mode cmsMode = Mode.AUTO;
        private string CurrentProgram = "C";
        private void Update()
        {
            if (!this.cmsEnabled) return;



            if (this.active)
            {
                this.cmsTimer += Time.deltaTime;
                if((this.mws.missileIncomingDetected || this.cmsMode != Mode.AUTO) && cmsTimer > autoReleaseTime)
                {
                    //Sets the appropiate chaff and flare configuration depending on the threat
                    if(this.cmsMode == Mode.AUTO || this.cmsMode == Mode.SEMI)
                    {
                        if(this.rwr.isLocked || this.rwr.isMissileLocked)
                        {
                            this.ChaffReleaseMode = RelaseMode.Single;
                            this.FlareRelaseMode = RelaseMode.None;
                            this.CurrentProgram = "C";
                            this.SetInterval(0.5f);

                        }
                        else
                        {
                            this.ChaffReleaseMode = RelaseMode.None;
                            this.FlareRelaseMode = RelaseMode.Double;
                            this.CurrentProgram = "F";
                            this.SetInterval(0.25f);
                        }
                    }
                    else
                    {
                        this.CurrentProgram = "M";
                    }

                    switch (this.ChaffReleaseMode)
                    {
                        case RelaseMode.Double:
                            manager.releaseMode = CountermeasureManager.ReleaseModes.Double;
                            this.chaff.FireCM();
                            break;
                        case RelaseMode.Single:
                            manager.releaseMode = CountermeasureManager.ReleaseModes.Single_Auto;
                            this.chaff.FireCM();
                            break;
                        case RelaseMode.None:
                            break;
                    }

                    switch (this.FlareRelaseMode)
                    {
                        case RelaseMode.Double:
                            manager.releaseMode = CountermeasureManager.ReleaseModes.Double;
                            this.flares.FireCM();
                            break;
                        case RelaseMode.Single:
                            manager.releaseMode = CountermeasureManager.ReleaseModes.Single_Auto;
                            this.flares.FireCM();
                            break;
                        case RelaseMode.None:
                            break;
                    }



                    this.cmsTimer = 0;
                }

                //So the timer doesn't become some massive number
                if(this.cmsTimer > 10)
                {
                    this.cmsTimer = 0;
                }
            }

        }

        public void SetCMSEnabled(int st)
        {
            this.cmsEnabled = st > 0;
        }

        public Mode GetCurrentMode()
        {
            return this.cmsMode;
        }

        public string GetCurrentProgram()
        {
            return this.CurrentProgram;
        }

        public void SetInterval(float time)
        {
            this.autoReleaseTime = time;
        }

        public void SetFlareRelase(int numRelease)
        {
            switch (numRelease)
            {
                case 0:
                    this.FlareRelaseMode = RelaseMode.None;
                    break;
                case 1:
                    this.FlareRelaseMode = RelaseMode.Single;
                    break;
                case 2:
                    this.FlareRelaseMode = RelaseMode.Double;
                    break;
                default:
                    Debug.Log("INVALID AMOUNT OF FLARES");
                    break;

            }
        }

        public void SetChaffRelease(int numRelease)
        {
            switch (numRelease)
            {
                case 0:
                    this.ChaffReleaseMode = RelaseMode.None;
                    break;
                case 1:
                    this.ChaffReleaseMode = RelaseMode.Single;
                    break;
                case 2:
                    this.ChaffReleaseMode = RelaseMode.Double;
                    break;
                default:
                    Debug.Log("INVALID AMOUNT OF CHAFF");
                    break;

            }
        }

        public void SetCMSMode(int state)
        {
            switch (state)
            {
                case 0:
                    FlightLogger.Log("CMS Mode auto");
                    this.active = true;
                    this.cmsMode = Mode.AUTO;
                    break;
                case 1:
                    FlightLogger.Log("CMS Mode semi");
                    this.active = false;
                    this.cmsMode = Mode.SEMI;
                    break;
                case 2:
                    FlightLogger.Log("CMS Mode manual");
                    this.active = false;
                    this.cmsMode = Mode.MANUAL;
                    break;

            }

            this.OnModeSwitch.Invoke();

        }

        public void StartCMS()
        {

            this.active = true;
                
        }

        public void StopCMS()
        {
            switch (this.cmsMode)
            {
                case Mode.AUTO:
                    this.active = true;
                    break;
                default:
                    this.active = false;
                    break;

            }

        }


        public void ToggleAutoCMS(int state)
        {

        }

        public enum Mode
        {
            MANUAL, 
            SEMI, 
            AUTO

        }

        public enum RelaseMode
        {
            Single, 
            Double,
            None 
        }
    }
}
