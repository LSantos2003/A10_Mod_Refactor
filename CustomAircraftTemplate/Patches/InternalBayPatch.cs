using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace A10Mod
{
    
    [HarmonyPatch(typeof(InternalWeaponBay),"Awake")]
    class InternalBayPatch
    {

        public static bool Prefix()
        {
            return true;
        }
    }


    [HarmonyPatch(typeof(InternalWeaponBay), "Start")]
    class InternalBayPatchStart
    {

        public static bool Prefix(InternalWeaponBay __instance)
        {
          
            return true;
        }
    }

 
	



}
