using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{

    [HarmonyPatch(typeof(Gun), "Start")]
    class GunSpawnPatch
    {


        public static void Prefix(Gun __instance)
        {


            bool mpCheck = true;

            if (MpPlugin.MPActive)
            {
                mpCheck = Main.instance.plugin.CheckPlaneSelected();

            }

            if (mpCheck && __instance.gameObject.GetComponentInParent<PlayerFlightLogger>() && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected)
            {
                __instance.maxAmmo = 1300;
                __instance.ejectTransform = null;

                __instance.audioProfiles[0].firingSound = Main.gauFireClip;
                __instance.audioProfiles[0].stopFiringSound = Main.gauEndClip;
                __instance.audioProfiles[0].audioSource.pitch = 1f;
                AircraftAPI.DisableMesh(__instance.gameObject);

            }
        }
    }

}
