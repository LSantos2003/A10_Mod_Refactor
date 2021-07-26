﻿using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplate
{

    [HarmonyPatch(typeof(PilotSelectUI), "Start")]
    class PilotSelectStartPatch
    {

        //Stores whether or not the ccustom vehicle is currently selected
        private static bool tempAircraftSelected = false;

        //Sets the temporary storage to if the vehicle is selected
        public static void Prefix()
        {
            if (Main.aircraftSelected)
            {
                tempAircraftSelected = true;
            }

        }

        //Changes the aircraftselected variable back to true if it was initially selected
        public static void Postfix()
        {
            if (tempAircraftSelected)
            {
                Main.aircraftSelected = true;
                tempAircraftSelected = false;
            }

        }
    }


    [HarmonyPatch(typeof(PilotSelectUI), "SelectVehicle")]
    class VehicleSelectPatch
    {
        private static bool lockAircraftSelect = false;
        public static bool Prefix(PilotSelectUI __instance, PlayerVehicle vehicle)
        {
            Debug.Log("Prefix ran!");
            if (vehicle.vehicleName == Main.vehicleName)
            {

                Debug.Log("Nighthawk ran!");
                //Bool that decides whether or not to run all the aircraft spawn code
                Main.aircraftSelected = true;
                lockAircraftSelect = true;

                __instance.SelectVehicle(PilotSaveManager.GetVehicle("F/A-26B"), null);
                return false;
            }

            if (!lockAircraftSelect)
            {
                Main.aircraftSelected = false;
            }

            lockAircraftSelect = false;
            return true;
        }
    }
}