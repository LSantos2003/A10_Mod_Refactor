using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace A10Mod
{
    class FlapsAnimator : MonoBehaviour
    {
        //This class is used to control the flaps, since they move out 

        public Vector3 initPos;
        public Vector3 finalPos;
        public float transitionTime;

        public Vector3 noAngle;
        public Vector3 fullAngle;
        private Coroutine currentCoroutine;

        private float normTime = 1;

        public void setValues(Vector3 initPos, Vector3 finalPos, float transitionTime, Vector3 noAngle, Vector3 fullAngle)
        {
            this.initPos = initPos;
            this.finalPos = finalPos;
            this.transitionTime = transitionTime;
            this.noAngle = noAngle;
            this.fullAngle = fullAngle;

        }

        public void leverListner(int state)
        {
            if (state == 0)
            {
                this.flaps0();
            }
            else if (state == 1)
            {
                this.flaps1();
            }
            else
            {
                this.flaps2();

            }
        }

        public void flaps0()
        {
            if (this.currentCoroutine != null)
            {
                base.StopCoroutine(currentCoroutine);
            }

            this.currentCoroutine = base.StartCoroutine(openRoutine(this.transitionTime));
        }

        public void flaps2()
        {
            if (this.currentCoroutine != null)
            {
                base.StopCoroutine(currentCoroutine);
            }

            this.currentCoroutine = base.StartCoroutine(closeRoutine(this.transitionTime));
        }

        public void flaps1()
        {
            if (this.currentCoroutine != null)
            {
                base.StopCoroutine(currentCoroutine);
            }

            this.currentCoroutine = base.StartCoroutine(halfRoutine(this.transitionTime));
        }



        private IEnumerator openRoutine(float timeToMove)
        {


            while (this.normTime < 1)
            {
                normTime += Time.deltaTime / timeToMove;
                transform.localPosition = Vector3.Lerp(this.finalPos, this.initPos, normTime);
                transform.localEulerAngles = Vector3.Lerp(this.fullAngle, this.noAngle, normTime);
                yield return null;
            }
            this.normTime = 1;
            yield break;
        }
        private IEnumerator halfRoutine(float timeToMove)
        {

            if (this.normTime > 0.5f)
            {
                while (this.normTime > 0.5f)
                {
                    normTime -= Time.deltaTime / timeToMove;
                    transform.localPosition = Vector3.Lerp(this.finalPos, this.initPos, normTime);
                    transform.localEulerAngles = Vector3.Lerp(this.fullAngle, this.noAngle, normTime);
                    yield return null;
                }
                this.normTime = 0.5f;
                yield break;
            }
            else
            {

                while (this.normTime < 0.5f)
                {
                    normTime += Time.deltaTime / timeToMove;
                    transform.localPosition = Vector3.Lerp(this.finalPos, this.initPos, normTime);
                    transform.localEulerAngles = Vector3.Lerp(this.fullAngle, this.noAngle, normTime);
                    yield return null;
                }
                this.normTime = 0.5f;
                yield break;
            }




        }
        private IEnumerator closeRoutine(float timeToMove)
        {


            while (this.normTime > 0)
            {
                normTime -= Time.deltaTime / timeToMove;
                transform.localPosition = Vector3.Lerp(this.finalPos, this.initPos, normTime);
                transform.localEulerAngles = Vector3.Lerp(this.fullAngle, this.noAngle, normTime);
                yield return null;
            }
            this.normTime = 0;
            yield break;
        }

    }
}
