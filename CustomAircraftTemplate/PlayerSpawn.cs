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
    [HarmonyPatch(typeof(WeaponManager), "Awake")]
    class PlayerSpawnAwakePatch
    {

        private static Vector3 aircraftLocalPosition = new Vector3(0.127f, -1.971f, 0.389f);
        private static Vector3 aircraftLocalEuler = new Vector3(0.02f, 89.00f, 1.04f);
        private static Vector3 aircraftLocalScale= Vector3.one * 2.745594f;
       

        public static void Prefix(WeaponManager __instance)
        {
            FlightLogger.Log("Awake prefix ran in wm!");
            bool mpCheck = true;

            if (MpPlugin.MPActive)
            {
                mpCheck = Main.instance.plugin.CheckPlaneSelected();

            }

            PerTeamRadarSymbol symbol = __instance.gameObject.GetComponentInChildren<PerTeamRadarSymbol>(true);
            //the check to see if the actor belongs to the local player
            bool isLocalAircraft = __instance.isPlayer && mpCheck && symbol && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected; 

            //the check to see if another player is using a custom aircraft and if their base aircraft is an fa-26
            //NOTE: Only works if the base aircraft has a radar. Gonna have to do a different check to see if it's an AV-42
            bool isClientAircraft = symbol && symbol.teamASymbol == "26" && !__instance.isPlayer; 

            if (isLocalAircraft || isClientAircraft)
            {
                Main.playerGameObject = __instance.gameObject;


                AircraftAPI.FindSwitchBounds();

                
                UnityMover mover = __instance.gameObject.AddComponent<UnityMover>();
                mover.gs = __instance.gameObject;
                mover.FileName = AircraftInfo.UnityMoverFileName;
                mover.load(true);
               

             

                FlightLogger.Log($"About to add {AircraftInfo.AircraftNickName}");



                GameObject aircraft = GameObject.Instantiate(Main.aircraftPrefab);
                aircraft.transform.SetParent(Main.playerGameObject.transform);
                aircraft.transform.localPosition = aircraftLocalPosition;
                aircraft.transform.localEulerAngles = aircraftLocalEuler;
                aircraft.transform.localScale = aircraftLocalScale;

                AircraftSetup.Fa26 = Main.playerGameObject;
                AircraftSetup.customAircraft = aircraft;


                //Creates the canopy animation and assigns the canopyobject to the ejection seat
                AircraftSetup.CreateCanopyAnimation();


                //Creates the control surfaces
                AircraftSetup.CreateControlSurfaces();

                //Creates the custom landing gear
                AircraftSetup.CreateLandingGear();

                //Moves the hardpoints in the correct location
                //AircraftSetup.SetUpHardpoints();

                //Attaches the refuel port animation to the refuel port class
                AircraftSetup.SetUpRefuelPort();


                //Assigns the suspension components to the custom aircraft landing gear
                AircraftSetup.SetUpWheels();

                //Changes characteristics of the engines
                AircraftSetup.SetUpEngines();

                //Reduces FA-26 mass
                AircraftSetup.SetUpMass();

                //Increases lift
                AircraftSetup.SetUpFlightModel();

                //Parents a10 wings to fa26's
                //AircraftSetup.SetUpWingDamage();

                //AircraftSetup.SetUpMissileLaunchers();

                //Disables the Fa26's wingflex so nav lights don't get screwy
                AircraftSetup.DisableWingFlex();

                //Assigns the correct variables for the EOTS
                //AircraftSetup.SetUpEOTS();


                AircraftSetup.ChangeRWRIcon();

                AircraftSetup.SetUpFlares();
                //AircraftAPI.FindInteractable("Toggle Altitude Mode").OnInteract.AddListener(logRCS);

                AircraftSetup.SetUpFormationLights();


                if (isLocalAircraft)
                {
                    //Changes depth and scale of the hud to make it legible
                    AircraftSetup.SetUpHud();

                    //Fixes the weird shifting nav map bug. Must be called after unity mover
                    AircraftSetup.ScaleNavMap();

                    //Changes mfd color to green
                    AircraftSetup.SetUpMFD();

                    AircraftSetup.SetUpGauges();

                    AircraftSetup.SetUpClock();

                    AircraftSetup.SetUpGaugeGlow();


                    AircraftSetup.SetUpAutoCMS();

                    AircraftSetup.SetUpFrontCMSPanel();

                    AircraftSetup.SetUpSideCMSPanel();

                    AircraftSetup.SetUpRWRSounds();

                    //Sets up knob interactables in the a-10
                    //Make sure this is one of the last methods called in order for it
                    //to grab the right components
                    AircraftSetup.SetUpKnobs();
                }


                FlightLogger.Log("Disabling mesh");
                AircraftAPI.Disable26Mesh();
               


            }
        }

        public static void logRCS()
        {
            foreach(HPEquippable hp in Main.playerGameObject.GetComponentsInChildren<HPEquippable>(true))
            {
                FlightLogger.Log($"RCS of idx {hp.hardpointIdx}: {hp.GetRadarCrossSection()}");
            }
        }
    }


    [HarmonyPatch(typeof(WeaponManager), "Start")]
    class PlayerSpawnStartPatch
    {


        public static void Prefix(WeaponManager __instance)
        {

            FlightLogger.Log("Start prefix ran in wm!");

            PerTeamRadarSymbol symbol = __instance.gameObject.GetComponentInChildren<PerTeamRadarSymbol>(true);


            bool isLocalAircraft =  symbol && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected;
            bool isClientAircraft = symbol && symbol.teamASymbol == "26";

            if (isLocalAircraft || isClientAircraft)
            {

                GameObject fa26 = __instance.gameObject;
                //Makes missiles compatabile with the internal bays
                //AircraftSetup.SetUpMissileLaunchers();

                //Reduces the rcs of the fa-26 and intiially sets the hard point rcs to 0
                //AircraftSetup.SetUpRCS();

                //Folds the wings down on spawn. Runs a coroutine that waits one second to do so
                AircraftSetup.SetWingFold();
                AircraftSetup.SetUpGun();

                if (isLocalAircraft)
                {
                    AircraftSwitchSetup.SetUpBottomLeft(fa26, fa26);
                    AircraftSwitchSetup.SetUpEngineStart();
                    AircraftSwitchSetup.SetUpGenerators(fa26, fa26);
                    AircraftSwitchSetup.SetUpEmergency(fa26, fa26);
                    AircraftSwitchSetup.SetUpRightPanel(fa26, fa26);
                }

            }
        }


        public static void Postfix(WeaponManager __instance)
        {
            FlightLogger.Log("Start postfix ran in wm!");


            PerTeamRadarSymbol symbol = __instance.gameObject.GetComponentInChildren<PerTeamRadarSymbol>(true);

            bool isLocalAircraft = symbol && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected;
            bool isClientAircraft = symbol && symbol.teamASymbol == "26";


            if (isLocalAircraft || isClientAircraft)
            {
                //Gets rid of the steam textures on the rudder
                AircraftAPI.Disable26SteamTextures();


                if (isLocalAircraft)
                {
                    AircraftSetup.SetUpIndexers();

                    //Auto selects the rwr page on the MMFD
                    AircraftSetup.SetUpRWR();
                }

                //AircraftSetup.SetUpGun();

            }
        }



    }


 }
