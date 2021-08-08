using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{
    class AircraftSetup
    {
        public static GameObject Fa26;
        public static GameObject customAircraft;

        public static void CreateCanopyAnimation()
        {
            WeaponManager wm = Fa26.GetComponentInChildren<WeaponManager>(true);

            GameObject faCanopyAnim = AircraftAPI.GetChildWithName(Fa26, "CanopyAnimator");
            faCanopyAnim.SetActive(false);
            CanopyAnimator canopyAnim = faCanopyAnim.GetComponent<CanopyAnimator>();

            //Attach any handles for the canopy animation here!
            //canopyAnim.handleInteractables[0] = leftHandleInt;
            
            canopyAnim.animator = AircraftAPI.GetChildWithName(customAircraft, "CanopyAnimationObject").GetComponent<Animator>();
            canopyAnim.canopyTf = AircraftAPI.GetChildWithName(customAircraft, "CanopyAnimationObject").transform;

            Fa26.GetComponentInChildren<EjectionSeat>().canopyObject = AircraftAPI.GetChildWithName(customAircraft, "CanopyAnimationObject");

            faCanopyAnim.SetActive(true);
            AircraftAPI.DisableMesh(AircraftAPI.GetChildWithName(Fa26, "Canopy"), wm);

            VRInteractable canopyInteractable = AircraftAPI.FindInteractable("Canopy");
            VRLever canopyLever = canopyInteractable.gameObject.GetComponentInChildren<VRLever>(true);
            canopyAnim.SetCanopyImmediate(canopyLever.currentState == 1);
        }


        //This is where you create all your control surface!
        //Note: These surfaces are really just for show and do not affect the flight characteristics
        public static void CreateControlSurfaces()
        {

            AeroController controller = Fa26.GetComponentInChildren<AeroController>();

            //Elevator
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "Elavator").transform, new Vector3(0, 1, 0), 35, 70, 1, 0, 0, 0, 20, false, 0, 0);


            //Right aileron
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronRightTf").transform, new Vector3(0, 0, -1), 35, 70, 0, 1, 0, 0, 20, false, 0, 0);
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronRightTopBrk").transform, new Vector3(0, 0, 1), 75, 30, 0, 0, 0, 1, -1, false, 0, 0);
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronRightBottomBrk").transform, new Vector3(0, 0, -1), 70, 30, 0, 0, 0, 1, -1, false, 0, 0);

            //Leftaileron
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronLeftTopTf").transform, new Vector3(0, 0, 1), 35, 70, 0, 1, 0, 0, 20, false, 0, 0); ;
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronLeftTopBrk").transform, new Vector3(0, 0, 1), 75, 30, 0, 0, 0, 1, -1, false, 0, 0);
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "AileronLeftBottomBrk").transform, new Vector3(0, 0, -1), 75, 30, 0, 0, 0, 1, -1, false, 0, 0);

            //RightRudder
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "RudderRightTf").transform, new Vector3(0, -1, 0), 15, 80, 0, 0, 1, 0, -1, false, 0, 0);

            //LeftRudder
            AircraftAPI.CreateControlSurface(controller, AircraftAPI.GetChildWithName(customAircraft, "RudderLeftTf").transform, new Vector3(0, -1, 0), 15, 80, 0, 0, 1, 0, -1, false, 0, 0);

            //Creates the flaps
            VRLever flapsLever = AircraftAPI.FindInteractable("Flaps").GetComponent<VRLever>();

            //Right Flaps
            GameObject a10RightFlaps = AircraftAPI.GetChildWithName(customAircraft, "FlapsRight");


            FlapsAnimator a10RightFlapMover = a10RightFlaps.AddComponent<FlapsAnimator>();
            a10RightFlapMover.setValues(a10RightFlaps.transform.localPosition, new Vector3(0.561f, 0.658f, 0.6405255f), 1, new Vector3(0, 0, 0), new Vector3(0, 0, -13));
            flapsLever.OnSetState.AddListener(a10RightFlapMover.leverListner);

            //LeftFlaps
            GameObject a10LeftFlaps = AircraftAPI.GetChildWithName(customAircraft, "FlapsLeft");

            FlapsAnimator a10LeftFlapsMover = a10LeftFlaps.AddComponent<FlapsAnimator>();
            a10LeftFlapsMover.setValues(a10LeftFlaps.transform.localPosition, new Vector3(0.561f, 0.658f, -0.6167379f), 1, new Vector3(0, 0, 0), new Vector3(0, 0, -13));
            flapsLever.OnSetState.AddListener(a10LeftFlapsMover.leverListner);




        }

        public static void CreateLandingGear()
        {
            VRLever gearLever = AircraftAPI.FindInteractable("Landing Gear").gameObject.GetComponent<VRLever>();
            CustomLandingGear gear = customAircraft.AddComponent<CustomLandingGear>();
            gear.animToggle = AircraftAPI.GetChildWithName(customAircraft, "GearAnimator").GetComponent<AnimationToggle>();
            gear.gearLever = gearLever;
        }
        public static void SetUpHardpoints()
        {
            GameObject hpRight = AircraftAPI.GetChildWithName(customAircraft, "HPRightTf");
            GameObject hpLeft = AircraftAPI.GetChildWithName(customAircraft, "HPLeftTf");

            Transform hp12 = AircraftAPI.FindHardpoint(12).transform;
            Transform hp11 = AircraftAPI.FindHardpoint(11).transform;

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
            AnimationToggle animToggle = AircraftAPI.GetChildWithName(customAircraft, "FuelPort_Animation_Parent").GetComponent<AnimationToggle>();

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
                AircraftAPI.DisableMesh(hp.transform.parent.gameObject);
            }
           
        }

        public static void SetUpEngines()
        {
            foreach(ModuleEngine engine in Fa26.GetComponentsInChildren<ModuleEngine>(true))
            {
                if (engine.name.ToLower().Contains("left"))
                {
                    foreach(EngineRotator rotator in customAircraft.GetComponentsInChildren<EngineRotator>(true))
                    {
                        if (rotator.name.ToLower().Contains("left"))
                        {
                            rotator.engine = engine;
                        }
                    }
                }
                else
                {
                    foreach (EngineRotator rotator in customAircraft.GetComponentsInChildren<EngineRotator>(true))
                    {
                        if (rotator.name.ToLower().Contains("right"))
                        {
                            rotator.engine = engine;
                        }
                    }
                }
                engine.autoAB = false;
                engine.autoABThreshold = 1f;
                engine.maxThrust = 100f;
            }

            VRThrottle throttle = Fa26.GetComponentInChildren<VRThrottle>(true);
            throttle.abGate = false;
            throttle.abGateThreshold = 1.1f;

        }

        public static void SetUpWheels()
        {
            WheelsController wheelController = Fa26.GetComponent<WheelsController>();
            CopyRotation copyRot = AircraftAPI.GetChildWithName(customAircraft, "FrontWheelPivot").GetComponentInChildren<CopyRotation>(true);
            copyRot.target = wheelController.steeringTransform;

            SuspensionWheelAnimator frontSus = AircraftAPI.GetChildWithName(Fa26, "FrontGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator leftSus = AircraftAPI.GetChildWithName(Fa26, "LeftGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator rightSus = AircraftAPI.GetChildWithName(Fa26, "RightGear").GetComponentInChildren<SuspensionWheelAnimator>(true);

            SuspensionWheelAnimator frontSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "FrontWheelPivot").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator leftSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "GearLeft").GetComponentInChildren<SuspensionWheelAnimator>(true);
            SuspensionWheelAnimator rightSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "GearRight").GetComponentInChildren<SuspensionWheelAnimator>(true);

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
            Fa26.GetComponentInChildren<EjectionSeat>(true).canopyObject = AircraftAPI.GetChildWithName(customAircraft, "Canopy_Main") ;
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

            if (bays == null) return;

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

        public static void DisableWingFlex()
        {
            foreach(WingFlex flex in Fa26.GetComponentsInChildren<WingFlex>(true))
            {
                flex.flexFactor = 0;
            }
        }
        
        public static void ScaleNavMap()
        {
            Transform mfd1 = AircraftAPI.GetChildWithName(Fa26, "MFD1").transform;
            Transform mapParent = AircraftAPI.GetChildWithName(Fa26, "MapParent").transform;
            Transform mapDisplay = AircraftAPI.GetChildWithName(Fa26, "MapDisplay").transform;
            Transform mapTest = AircraftAPI.GetChildWithName(Fa26, "MapTest").transform;
            Transform mapTransform = AircraftAPI.GetChildWithName(Fa26, "MapTransform").transform;

            float small = mfd1.transform.localScale.x / 99.73274f;
            float big = 99.73274f / mfd1.transform.localScale.x;

            Vector3 smallScale = Vector3.one * small;
            Vector3 bigScale = Vector3.one * big;

            mapParent.transform.localScale = smallScale;
            mapDisplay.transform.localScale = bigScale;
            mapTest.transform.localScale = bigScale;
            mapTransform.transform.localScale = bigScale;

        }

        public static void SetUpRCS()
        {
            RadarCrossSection rcs = Fa26.GetComponent<RadarCrossSection>();
            rcs.size = 7.381652f;
            rcs.overrideMultiplier = 0.5f;

            foreach(HPEquippable hp in Fa26.GetComponentsInChildren<HPEquippable>(true))
            {
                hp.rcsMasked = true;
            }
        }

        public static void SetWingFold()
        {
            Main.instance.StartCoroutine(WingFoldRoutine());

        }

        public static IEnumerator WingFoldRoutine()
        {
            yield return new WaitForSeconds(1);

            Fa26.GetComponentInChildren<VehicleMaster>(true).SetWingFoldImmediate(false);
            Fa26.GetComponentInChildren<FlightWarnings>(true).RemoveCommonWarning(FlightWarnings.CommonWarnings.WingFold);

            VRLever wingLever = AircraftAPI.FindInteractable("Wing Fold").gameObject.GetComponent<VRLever>();
            wingLever.gameObject.GetComponent<AudioSource>().volume = 0;
            wingLever.RemoteSetState(0);

        }
    }
}
