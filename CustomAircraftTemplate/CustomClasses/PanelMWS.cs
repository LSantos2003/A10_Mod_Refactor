using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    public class PanelMWS : PanelText
    {
        public MissileDetector detector;

        private float warningTime = 4;
        private bool launchDetected = false;

        private Coroutine detectRoutine = null;
        private void Start()
        {
            this.detector.OnMissileLaunchDetected.AddListener(OnLaunchDetected);
        }

        private void OnDisable()
        {
            this.detector.OnMissileLaunchDetected.RemoveListener(OnLaunchDetected);
        }

        protected override void UpdateText()
        {
       

            if (!detector.enabled)
            {
                SetText("OFF");
                return;
            }

            if(detector.enabled && launchDetected)
            {
                SetText("LAUNCH");
                return;
            }

            SetText("ACTIVE");
        }
        private void OnLaunchDetected()
        {
            if(detectRoutine != null)
            {
                StopCoroutine(detectRoutine);
                return;
            }

            this.detectRoutine = StartCoroutine(LaunchDetectRoutine());

        }


        private IEnumerator LaunchDetectRoutine()
        {
            this.launchDetected = true;
            yield return new WaitForSeconds(this.warningTime);

            this.launchDetected = false;
            this.detectRoutine = null;

        }

    }

}
