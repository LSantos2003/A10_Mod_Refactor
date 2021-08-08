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
    public class RadarJammer : MonoBehaviour
    {
        private void OnDisable()
        {
            if (this.text != null)
            {
                this.text.text = "SBY";
                this.text.ApplyText();
            }
            jammingUnit = JammingUnit.None;
        }
        private void OnEnable()
        {
            SetJammingUnitListener();
        }
        public void SetJammingUnit(JammingUnit unit)
        {
            this.jammingUnit = unit;
        }

        public void SetJammingUnitListener()
        {
            if (!enabled || this.text == null)
                return;
            switch (jammingUnit)
            {
                case JammingUnit.Air:
                case JammingUnit.None:
                    this.jammingUnit = JammingUnit.Sam;
                    this.text.text = "SAM";
                    //DebugFlightLogger.Log("Jamming sams");
                    break;
                case JammingUnit.Sam:
                    this.jammingUnit = JammingUnit.AAA;
                    this.text.text = "AAA";
                    //DebugFlightLogger.Log("Jamming AAAs");
                    break;
                case JammingUnit.AAA:
                    this.jammingUnit = JammingUnit.Air;
                    this.text.text = "AIR";
                    //DebugFlightLogger.Log("Jamming Airs");
                    break;
                default:
                    Debug.Log("Shit went down in the radar jammer class lmao, pls fix");
                    break;
            }
            this.text.ApplyText();
        }
        public bool JammedUnit(Actor actor)
        {
            return (actor.role == Actor.Roles.Air && this.jammingUnit == RadarJammer.JammingUnit.Air) ||
                ((actor.actorName.Contains("Anti-Air Artillery") || actor.actorName.Contains("SAAW")) && jammingUnit == JammingUnit.AAA) ||
                ((actor.actorName.Contains("SAM") || actor.actorName.Contains("SAAW")) && jammingUnit == JammingUnit.Sam);
        }

        public JammingUnit jammingUnit;
        public VTText text;
        public enum JammingUnit
        {
            Air,
            Sam,
            AAA,
            None
        }
    }
}