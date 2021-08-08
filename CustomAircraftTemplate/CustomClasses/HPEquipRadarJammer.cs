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
    public class HPEquipRadarJammer : HPEquippable, IMassObject
    {
        public override int GetCount()
        {
            return 1;
        }
        public float GetMass()
        {
            return 0.272f; // for some reason 272 is 272k and will make your plane instantly crash when equipped
        }
    }
}