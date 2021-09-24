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
using UnityEngine.UI;

namespace A10Mod
{
    class RefuelIndexer : MonoBehaviour
    {
        private Traverse RefuelPlaneTraverse;
        private refuelStates currentState;
        public RefuelPlane refueler;
        public RefuelPort port;
        public Battery battery;
        public VTText ready;
        public VTText latched;
        public VTText disconnected;

        public enum refuelStates
        {
            NO_REFUELER,
            FUELPORT_CLOSED,
            READY,
            LATCHED,
            DISCONNECTED
        }


        private void Awake()
        {
            //port = FlightSceneManager.instance.playerActor.gameObject.GetComponentInChildren<RefuelPort>();
            //refueler = port.currentRefuelPlane;
            //RefuelPlaneTraverse = Traverse.Create(refueler);
        }

        private void Update()
        {
            this.currentState = determineStates(this.port, this.port.currentRefuelPlane);
            UpdateRefuelStates(this.currentState);

        }

        private refuelStates determineStates(RefuelPort port, RefuelPlane refueler)
        {
            if (!port.open)
            {
                return refuelStates.FUELPORT_CLOSED;
            }

            if (this.currentState == refuelStates.LATCHED && refueler == null)
            {
                return refuelStates.DISCONNECTED;
            }

            if (refueler == null && this.currentState != refuelStates.DISCONNECTED)
            {
                return refuelStates.READY;
            }

            if (refueler != null)
            {
                return refuelStates.LATCHED;
            }
            return refuelStates.DISCONNECTED;
        }


        private void UpdateRefuelStates(refuelStates state)
        {
            switch (state)
            {
                case refuelStates.FUELPORT_CLOSED:
                case refuelStates.NO_REFUELER:
                    this.ready.color = Color.black;
                    this.latched.color = Color.black;
                    this.disconnected.color = Color.black;
                    this.ready.SetEmission(false);
                    this.latched.SetEmission(false);
                    this.disconnected.SetEmission(false);
                    break;
                case refuelStates.READY:
                    this.ready.color = Color.green;
                    this.latched.color = Color.black;
                    this.disconnected.color = Color.black;
                    this.ready.SetEmission(true);
                    this.latched.SetEmission(false);
                    this.disconnected.SetEmission(false);
                    break;
                case refuelStates.LATCHED:
                    this.ready.color = Color.black;
                    this.latched.color = Color.green;
                    this.disconnected.color = Color.black;
                    this.ready.SetEmission(false);
                    this.latched.SetEmission(true);
                    this.disconnected.SetEmission(false);
                    break;
                case refuelStates.DISCONNECTED:
                    this.ready.color = Color.black;
                    this.latched.color = Color.black;
                    this.disconnected.color = Color.green;
                    this.ready.SetEmission(false);
                    this.latched.SetEmission(false);
                    this.disconnected.SetEmission(true);
                    break;
                default:
                    Debug.Log("my refuel states :(");
                    break;

            }
            this.ready.ApplyText();
            this.latched.ApplyText();
            this.disconnected.ApplyText();
        }
    }
}