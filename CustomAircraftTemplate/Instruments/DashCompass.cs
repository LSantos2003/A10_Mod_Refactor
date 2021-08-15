using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    class DashCompass : MonoBehaviour
    {

        public Transform compassTransform;

        public float slerpRate = 1;

        public FlightInfo flightInfo;

        public Vector3 axis;


        private void Update()
        {
            //TODO fix compass so that it points north, even if upside down
            Vector3 slerpVector = Vector3.Scale(new Vector3(this.flightInfo.heading, this.flightInfo.heading, this.flightInfo.heading), this.axis);
            slerpVector += new Vector3(-90, 0, 79.9f);
            this.compassTransform.localRotation = Quaternion.Slerp(this.compassTransform.localRotation, Quaternion.Euler(slerpVector), this.slerpRate * Time.deltaTime);

        }


    }
}
