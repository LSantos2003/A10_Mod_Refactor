﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplate
{
    class AircraftSetup
    {
        public static GameObject Fa26;
        public static GameObject customAircraft;

        public static void CreateCanopyAnimation()
        {
            WeaponManager wm = Fa26.GetComponentInChildren<WeaponManager>(true);

            GameObject faCanopyAnim = AircraftHelper.GetChildWithName(Fa26, "CanopyAnimator");
            faCanopyAnim.SetActive(false);
            CanopyAnimator canopyAnim = faCanopyAnim.GetComponent<CanopyAnimator>();

            //Attach any handles for the canopy animation here!
            //canopyAnim.handleInteractables[0] = leftHandleInt;
            
            canopyAnim.animator = AircraftHelper.GetChildWithName(customAircraft, "CanopyAnimator").GetComponent<Animator>();
            canopyAnim.canopyTf = AircraftHelper.GetChildWithName(customAircraft, "CanopyTf").transform;

            Fa26.GetComponentInChildren<EjectionSeat>().canopyObject = AircraftHelper.GetChildWithName(customAircraft, "CanopyTf");

            faCanopyAnim.SetActive(true);
            AircraftHelper.DisableMesh(AircraftHelper.GetChildWithName(Fa26, "Canopy"), wm);

            VRInteractable canopyInteractable = AircraftHelper.FindInteractable("Canopy");
            VRLever canopyLever = canopyInteractable.gameObject.GetComponentInChildren<VRLever>(true);
            canopyAnim.SetCanopyImmediate(canopyLever.currentState == 1);
        }


        //This is where you create all your control surface!
        //Note: These surfaces are really just for show and do not affect the flight characteristics
        public static void CreateControlSurfaces()
        {

            AeroController controller = Fa26.GetComponentInChildren<AeroController>();

            //Aileron Example
            AircraftHelper.createControlSurface(controller, AircraftHelper.GetChildWithName(customAircraft, "LeftAileronTf").transform, new Vector3(0, 0, 1), 35, 70, 0, 1, 0, 0, 20, false, 0, 0);
            AircraftHelper.createControlSurface(controller, AircraftHelper.GetChildWithName(customAircraft, "RightAileronTf").transform, new Vector3(0, 0, -1), 35, 70, 0, 1, 0, 0, 20, false, 0, 0);

            AircraftHelper.createControlSurface(controller, AircraftHelper.GetChildWithName(customAircraft, "LeftTailTf").transform, new Vector3(0, 1, 0), 25, 50, 0.6f, 0, 0.4f, 0, 20, false, 0, 0);
            AircraftHelper.createControlSurface(controller, AircraftHelper.GetChildWithName(customAircraft, "RightTailTf").transform, new Vector3(0, -1, 0), 25, 50, 0.6f, 0, -0.4f, 0, 20, false, 0, 0);
        }

        public static void CreateLandingGear()
        {
            VRLever gearLever = AircraftHelper.FindInteractable("Landing Gear").gameObject.GetComponent<VRLever>();
            CustomLandingGear gear = customAircraft.AddComponent<CustomLandingGear>();
            gear.animToggle = AircraftHelper.GetChildWithName(customAircraft, "GearAnimator").GetComponent<AnimationToggle>();
            gear.gearLever = gearLever;
        }
        public static void SetUpHardpoints()
        {
            GameObject hpRight = AircraftHelper.GetChildWithName(customAircraft, "HPRightTf");
            GameObject hpLeft = AircraftHelper.GetChildWithName(customAircraft, "HPLeftTf");

            Transform hp12 = AircraftHelper.FindHardpoint(12).transform;
            Transform hp11 = AircraftHelper.FindHardpoint(11).transform;

            hp12.transform.SetParent(hpRight.transform);
            hp12.transform.localPosition = Vector3.zero;
            hp12.transform.localEulerAngles = Vector3.zero;

            hp11.transform.SetParent(hpLeft.transform);
            hp11.transform.localPosition = Vector3.zero;
            hp11.transform.localEulerAngles = Vector3.zero;


        }

        public static void SetUpRefuelPort()
        {
            RefuelPort port = Fa26.GetComponentInChildren<RefuelPort>();
            AnimationToggle animToggle = AircraftHelper.GetChildWithName(customAircraft, "RefuelPortAnimator").GetComponent<AnimationToggle>();

            port.OnOpen.AddListener(animToggle.Deploy);
            port.OnClose.AddListener(animToggle.Retract);

        }

        public static void SetUpInvisTGP()
        {
            WeaponManager wm = Fa26.GetComponentInChildren<WeaponManager>(true);
            wm.OnWeaponEquipped += WeaponManager_OnWeaponEquipped;

        }

        private static void WeaponManager_OnWeaponEquipped(HPEquippable hp)
        {
            Debug.Log("Weapon equip thing ran!");
            if (hp.shortName == "TGP1")
            {
                Debug.Log("Disabled tgp mesh!");
                AircraftHelper.DisableMesh(hp.transform.parent.gameObject);
            }
           
        }

        public static void SetUpEngines()
        {
            foreach(ModuleEngine engine in Fa26.GetComponentsInChildren<ModuleEngine>(true))
            {
                engine.autoAB = false;
                engine.autoABThreshold = 1f;
            }

            VRThrottle throttle = Fa26.GetComponentInChildren<VRThrottle>(true);
            throttle.abGate = false;
            throttle.abGateThreshold = 1.1f;

        }

        public static void SetUpWheels()
        {
            SuspensionWheelAnimator frontSus = AircraftHelper.GetChildWithName(Fa26, "FrontGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator leftSus = AircraftHelper.GetChildWithName(Fa26, "LeftGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator rightSus = AircraftHelper.GetChildWithName(Fa26, "RightGear").GetComponentInChildren<SuspensionWheelAnimator>(true);

            SuspensionWheelAnimator frontSusCustomAircraft = AircraftHelper.GetChildWithName(customAircraft, "FrontLegTf").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator leftSusCustomAircraft = AircraftHelper.GetChildWithName(customAircraft, "LeftLeftTf").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator rightSusCustomAircraft = AircraftHelper.GetChildWithName(customAircraft, "RightLegTf").GetComponentInChildren<SuspensionWheelAnimator>(true);

            frontSusCustomAircraft.suspension = frontSus.suspension;
            leftSusCustomAircraft.suspension = leftSus.suspension;
            rightSusCustomAircraft.suspension = rightSus.suspension;

        }
        public static void SetUpHud()
        {
            CollimatedHUDUI hud = Fa26.GetComponentInChildren<CollimatedHUDUI>(true);
            hud.depth = 1000f;
            hud.UIscale = 1.5f;
        }

        public static void SetUpEjectionSeat()
        {
            Fa26.GetComponentInChildren<EjectionSeat>(true).canopyObject = AircraftHelper.GetChildWithName(customAircraft, "CanopyTf") ;
        }

        public static void SetUpEOTS()
        {
            OpticalTargeter targeter = customAircraft.GetComponentInChildren<OpticalTargeter>();

            WeaponManager wm = Fa26.GetComponentInChildren<WeaponManager>(true); ;
            targeter.actor = Fa26.GetComponentInChildren<Actor>(true);
            targeter.wm = wm;

            wm.opticalTargeter = targeter;
            Fa26.GetComponentInChildren<TargetingMFDPage>(true).opticalTargeter = targeter;
        }


        public static void SetUpMissileLaunchers()
        {
            InternalWeaponBay[] bays = customAircraft.GetComponentsInChildren<InternalWeaponBay>(true);

            foreach (MissileLauncher ml in Fa26.GetComponentsInChildren<MissileLauncher>(true))
            {
                FlightLogger.Log("Found " + ml.missilePrefab.name);
                ml.openAndCloseBayOnLaunch = true;

                foreach(InternalWeaponBay bay in bays)
                {
                    if(bay.hardpointIdx == ml.gameObject.GetComponent<HPEquippable>().hardpointIdx)
                    {
                        FlightLogger.Log("Assigning iwb");
                        ml.SetInternalWeaponBay(bay);
                    }
                }
                
            }

        }


    }
}