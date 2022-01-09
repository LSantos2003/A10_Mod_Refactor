using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace A10Mod
{

    [HarmonyPatch(typeof(Gun), "Awake")]
    class GunSpawnPatch
    {


        public static void Prefix(Gun __instance)
        {


            bool mpCheck = true;

            if (MpPlugin.MPActive)
            {
                mpCheck = Main.instance.plugin.CheckPlaneSelected();

            }

            PerTeamRadarSymbol symbol = __instance.gameObject.GetComponentInChildren<PerTeamRadarSymbol>(true);
            PlayerVehicleNetSync entity = __instance.gameObject.GetComponent<PlayerVehicleNetSync>();

            //Breaks out in case the actor doesn't have a playervehiclenetsync object


            //the check to see if the actor belongs to the local player
            bool isLocalAircraft = entity && entity.isMine && mpCheck && symbol && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected;

            //the check to see if another player is using a custom aircraft and if their base aircraft is an fa-26
            //NOTE: Only works if the base aircraft has a radar. Gonna have to do a different check to see if it's an AV-42
            bool isClientAircraft = entity && !entity.isMine && symbol && symbol.teamASymbol == "26";

            if (isLocalAircraft || isClientAircraft)
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
