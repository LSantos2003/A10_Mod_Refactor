using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Harmony;

namespace A10Mod
{
    class AircraftSetup
    {
        public static GameObject Fa26;
        public static GameObject customAircraft;

        //Destroys or enables gameobjects that are inactive
        //Useful for spawning in the ai version of the a-10 without having to create a new assetbundle
        public static void GameObjectCheck(bool deleteObject)
        {
            foreach(GameObject go in customAircraft.GetComponentsInChildren<GameObject>(true))
            {
                if (!go.activeSelf && deleteObject)
                {
                    GameObject.Destroy(go);
                }else if (!go.activeSelf)
                {
                    go.SetActive(true);
                }
            }
        }

       
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

        public static void SetUpGun()
        {
            WeaponManager wm = Fa26.GetComponentInChildren<WeaponManager>(true);
            HPEquippable gunEquip = wm.GetEquip(0);

            gunEquip = wm.GetEquip(0);
            if (gunEquip.gameObject.GetComponent<Gun>() != null)
            {
                Gun gun = gunEquip.gameObject.GetComponent<Gun>();
                gun.maxAmmo = 1300;
                gun.rpm = 3900;
                gun.gunMass = 0.281f;
                gun.currentAmmo = 1300;
                gun.recoilFactor = 5f;
                //gun.gameObject.GetComponent<GunBarrelRotator>().rotationTransform = AircraftAPI.GetChildWithName(customAircraft, "BarrelTf").transform;

                gun.bulletInfo.speed = 1010;
                gun.bulletInfo.speed = 1010;
                gun.bulletInfo.tracerWidth = 0.12f;
                gun.bulletInfo.dispersion = 0.5f;
                gun.bulletInfo.damage = 30f;
                gun.bulletInfo.detonationRange = 8;
                gun.bulletInfo.lifetimeVariance = 0;
                gun.bulletInfo.projectileMass = 0.000365f;
                gun.bulletInfo.totalMass = 0.000727f;

                GunBarrelRotator rotator = gun.gameObject.AddComponent<GunBarrelRotator>();
                rotator.rotationTransform = AircraftAPI.GetChildWithName(customAircraft, "BarrelTf").transform;
                rotator.axis = new Vector3(1, 0, 0);
                rotator.gun = gun;
                rotator.speed = 1525f;
                rotator.windDownRate = 2;
                rotator.windupRate = 10;
                rotator.minFiringSpeed = 1200;

                gun.barrelRotator = rotator;


                AircraftAPI.DisableMesh(gun.gameObject);

                gunEquip.armable = true;
                gunEquip.armed = true;
            }
        }

        public static void SetUpGauges()
        {

            //Intially gets nessesary components for the gauges
            Battery battery = Fa26.GetComponentInChildren<Battery>(true);
            FlightInfo flightInfo = Fa26.GetComponentInChildren<FlightInfo>(true);

            ModuleEngine engineL = null;
            ModuleEngine engineR = null;
            foreach (ModuleEngine engine in Fa26.GetComponentsInChildren<ModuleEngine>(true))
            {
                if (engine.name.ToLower().Contains("left"))
                {
                    engineL = engine;
                }
                else
                {
                    engineR = engine;
                }

            }


            //AOA Gauge
            GameObject aoaGauge = AircraftAPI.GetChildWithName(customAircraft, "AOAGauge");
            DashAOAGauge aoa = aoaGauge.AddComponent<DashAOAGauge>();
            aoa.battery = battery;
            aoa.dialHand = AircraftAPI.GetChildWithName(aoaGauge, "dialHand").transform;
            aoa.axis = new Vector3(0, 1, 0);
            aoa.arcAngle = 360;
            aoa.maxValue = 40;
            aoa.lerpRate = 8;
            aoa.loop = true;
            aoa.gizmoRadius = 0.02f;
            aoa.gizmoHeight = 0.005f;
            aoa.doCalibration = true;
            aoa.calibrationSpeed = 1;
            aoa.info = flightInfo;


            GameObject accelGauge = AircraftAPI.GetChildWithName(customAircraft, "AccelGauge");
            DashAccelGauge accel = accelGauge.AddComponent<DashAccelGauge>();
            accel.battery = battery;
            accel.dialHand = AircraftAPI.GetChildWithName(accelGauge, "dialHand").transform;
            accel.axis = new Vector3(0, 1, 0);
            accel.arcAngle = 360;
            accel.maxValue = 20;
            accel.lerpRate = 8;
            accel.loop = true;
            accel.gizmoRadius = 0.02f;
            accel.gizmoHeight = 0.005f;
            accel.doCalibration = true;
            accel.calibrationSpeed = 1;
            accel.info = flightInfo;


            GameObject vertGauge = AircraftAPI.GetChildWithName(customAircraft, "ClimbGauge");
            DashVertGauge vert = vertGauge.AddComponent<DashVertGauge>();
            vert.battery = battery;
            vert.dialHand = AircraftAPI.GetChildWithName(vertGauge, "dialHand").transform;
            vert.axis = new Vector3(0, 1, 0);
            vert.arcAngle = 360;
            vert.maxValue = 10;
            vert.lerpRate = 8;
            vert.loop = true;
            vert.gizmoRadius = 0.02f;
            vert.gizmoHeight = 0.005f;
            vert.doCalibration = true;
            vert.calibrationSpeed = 1;
            vert.info = flightInfo;
            vert.measures = Fa26.GetComponent<MeasurementManager>();


            GameObject leftDial = AircraftAPI.GetChildWithName(customAircraft, "LeftFuelDialParent");
            leftDial.SetActive(false);
            DashFuelNumGauge dashFuelL = leftDial.AddComponent<DashFuelNumGauge>();

            dashFuelL.battery = battery;
            dashFuelL.dialHand = AircraftAPI.GetChildWithName(leftDial, "LeftFuelDialTf").transform;
            dashFuelL.axis = new Vector3(1, 0, 0);
            dashFuelL.arcAngle = 155.211f;
            dashFuelL.maxValue = 6;
            dashFuelL.lerpRate = 5;
            dashFuelL.loop = true;
            dashFuelL.gizmoRadius = 0.02f;
            dashFuelL.gizmoHeight = 0.005f;
            dashFuelL.doCalibration = true;
            dashFuelL.calibrationSpeed = 1;
            dashFuelL.tank = Fa26.GetComponent<FuelTank>();

            leftDial.SetActive(true);

            GameObject rightDial = AircraftAPI.GetChildWithName(customAircraft, "RightFuelDialParent");
            rightDial.SetActive(false);
            DashFuelNumGauge dashFuelR = rightDial.AddComponent<DashFuelNumGauge>();

            dashFuelR.battery = battery;
            dashFuelR.dialHand = AircraftAPI.GetChildWithName(rightDial, "RightWingFuelArrow").transform;
            dashFuelR.axis = new Vector3(1, 0, 0);
            dashFuelR.arcAngle = -155.211f;
            dashFuelR.maxValue = 6;
            dashFuelR.lerpRate = 5;
            dashFuelR.loop = true;
            dashFuelR.gizmoRadius = 0.02f;
            dashFuelR.gizmoHeight = 0.005f;
            dashFuelR.doCalibration = true;
            dashFuelR.calibrationSpeed = 1;
            dashFuelR.tank = Fa26.GetComponent<FuelTank>();
            rightDial.SetActive(true);


            GameObject fuelGauge = AircraftAPI.GetChildWithName(customAircraft, "FuelGaugeParent");
            DashFuelText fuelText = fuelGauge.AddComponent<DashFuelText>();
            fuelText.tank = Fa26.GetComponent<FuelTank>();
            fuelText.text = fuelGauge.GetComponentInChildren<TextMeshPro>(true);
            fuelText.gauge = dashFuelR;




            GameObject fuelFlowR = AircraftAPI.GetChildWithName(customAircraft, "FuelFlowGaugeR");

            DashEngineFlow rightEngineFlow = fuelFlowR.AddComponent<DashEngineFlow>();
            rightEngineFlow.battery = battery;
            rightEngineFlow.dialHand = AircraftAPI.GetChildWithName(fuelFlowR, "needle1 (1)").transform;
            rightEngineFlow.axis = new Vector3(0, 1, 0);
            rightEngineFlow.arcAngle = 240;
            rightEngineFlow.maxValue = 10.2f / 8;
            rightEngineFlow.lerpRate = 8;
            rightEngineFlow.loop = false;
            rightEngineFlow.gizmoRadius = 0.022f;
            rightEngineFlow.gizmoHeight = 0.006f;
            rightEngineFlow.doCalibration = true;
            rightEngineFlow.calibrationSpeed = 1.5f;
            rightEngineFlow.engineToMeasure = engineR;
            rightEngineFlow.engineToCheck = engineL;
            rightEngineFlow.tank = Fa26.GetComponentInChildren<FuelTank>();


            GameObject fuelFlowL = AircraftAPI.GetChildWithName(customAircraft, "FuelFlowGaugeL");

            DashEngineFlow leftEngineFlow = fuelFlowL.AddComponent<DashEngineFlow>();
            leftEngineFlow.battery = battery;
            leftEngineFlow.dialHand = AircraftAPI.GetChildWithName(fuelFlowL, "needle1 (1)").transform;
            leftEngineFlow.axis = new Vector3(0, 1, 0);
            leftEngineFlow.arcAngle = 240;
            leftEngineFlow.maxValue = 10.2f / 8;
            leftEngineFlow.lerpRate = 8;
            leftEngineFlow.loop = false;
            leftEngineFlow.gizmoRadius = 0.022f;
            leftEngineFlow.gizmoHeight = 0.006f;
            leftEngineFlow.doCalibration = true;
            leftEngineFlow.calibrationSpeed = 1.5f;
            leftEngineFlow.engineToMeasure = engineL;
            leftEngineFlow.engineToCheck = engineR;
            leftEngineFlow.tank = Fa26.GetComponentInChildren<FuelTank>();

           

            GameObject tempGaugeR = AircraftAPI.GetChildWithName(customAircraft, "EngineRTemp");

            DashTempGauge rightTempGauge = tempGaugeR.AddComponent<DashTempGauge>();
            HeatEmitter rightEmitter = engineR.gameObject.GetComponent<HeatEmitter>();
            rightTempGauge.battery = battery;
            rightTempGauge.dialHand = AircraftAPI.GetChildWithName(tempGaugeR, "needle1 (1)").transform;
            rightTempGauge.axis = new Vector3(0, 1, 0);
            rightTempGauge.arcAngle = 240;
            rightTempGauge.maxValue = 400f;
            rightTempGauge.lerpRate = 2;
            rightTempGauge.loop = false;
            rightTempGauge.gizmoRadius = 0.022f;
            rightTempGauge.gizmoHeight = 0.006f; ;
            rightTempGauge.doCalibration = true;
            rightTempGauge.calibrationSpeed = 1.5f;
            rightTempGauge.heatEmitter = rightEmitter;
            rightTempGauge.engineHealth = engineR.gameObject.GetComponent<Health>();


            GameObject tempGaugeL = AircraftAPI.GetChildWithName(customAircraft, "EngineLTemp");

            DashTempGauge leftTempGauge = tempGaugeL.AddComponent<DashTempGauge>();
            HeatEmitter leftEmitter = engineL.gameObject.GetComponent<HeatEmitter>();
            leftTempGauge.battery = battery;
            leftTempGauge.dialHand = AircraftAPI.GetChildWithName(tempGaugeL, "needle1 (1)").transform;
            leftTempGauge.axis = new Vector3(0, 1, 0);
            leftTempGauge.arcAngle = 240;
            leftTempGauge.maxValue = 400f;
            leftTempGauge.lerpRate = 2;
            leftTempGauge.loop = false;
            leftTempGauge.gizmoRadius = 0.022f;
            leftTempGauge.gizmoHeight = 0.006f; ;
            leftTempGauge.doCalibration = true;
            leftTempGauge.calibrationSpeed = 1.5f;
            leftTempGauge.heatEmitter = leftEmitter;
            leftTempGauge.engineHealth = engineL.gameObject.GetComponent<Health>();

            GameObject tempGaugeAPU = AircraftAPI.GetChildWithName(customAircraft, "APUTemp");

            DashAPUTemp apuGauge = tempGaugeAPU.AddComponent<DashAPUTemp>();
            AuxilliaryPowerUnit apu = Fa26.gameObject.GetComponentInChildren<AuxilliaryPowerUnit>(true);
            apuGauge.battery = battery;
            apuGauge.dialHand = AircraftAPI.GetChildWithName(tempGaugeAPU, "needle1 (1)").transform;
            apuGauge.axis = new Vector3(0, 1, 0);
            apuGauge.arcAngle = 240;
            apuGauge.maxValue = 1f;
            apuGauge.lerpRate = 2;
            apuGauge.loop = false;
            apuGauge.gizmoRadius = 0.022f;
            apuGauge.gizmoHeight = 0.006f; ;
            apuGauge.doCalibration = true;
            apuGauge.calibrationSpeed = 1.5f;
            apuGauge.apu = apu;
            apuGauge.apuHealth = apu.gameObject.GetComponent<Health>();


            GameObject ogSphere = AircraftAPI.GetChildWithName(Fa26, "AttSphereParent");
            GameObject backUpAttSphere = GameObject.Instantiate(ogSphere, ogSphere.transform.parent);
            //GameObject backUpAttBracket = Instantiate(Functions.GetChildWithName(this.go, "AttitudeIndicator"));
            //backUpAttBracket.transform.SetParent(backUpAttSphere.transform);

            backUpAttSphere.transform.localPosition = new Vector3(-260.4f, -237.7f, 137.9f);
            backUpAttSphere.transform.localEulerAngles = new Vector3(187.596f, 0, 180f);
            backUpAttSphere.transform.localScale = new Vector3(63, 63, 63);



            GameObject compass = AircraftAPI.GetChildWithName(customAircraft, "Heading_Meter");
            DashCompass piss = compass.AddComponent<DashCompass>();
            piss.axis = new Vector3(0, 0, -1);
            piss.compassTransform = compass.transform;
            piss.flightInfo = flightInfo;



        }

        public static void SetUpGaugeGlow()
        {
            EmissiveTextureLight emissLight = customAircraft.GetComponentInChildren<EmissiveTextureLight>(true);

            AircraftAPI.FindInteractable("Instrument Lights").gameObject.GetComponent<VRLever>().OnSetState.AddListener(emissLight.SetStatus);
        }

        public static void SetUpIndexers()
        {
            GameObject cage = AircraftAPI.GetChildWithName(customAircraft, "Accel_&_Heading_Instruments");

            GameObject refuelReady = AircraftAPI.GetChildWithName(cage, "ReadyTxt");
            GameObject refuelLatched = AircraftAPI.GetChildWithName(cage, "LatchedText");
            GameObject refuelDisc = AircraftAPI.GetChildWithName(cage, "DisconnectText");

            VTText ready = refuelReady.GetComponentInChildren<VTText>();
            VTText latch = refuelLatched.GetComponentInChildren<VTText>();
            VTText disconnect = refuelDisc.GetComponentInChildren<VTText>();

            GameObject aoaUp = AircraftAPI.GetChildWithName(cage, "AOAUp");
            GameObject aoaCenter = AircraftAPI.GetChildWithName(cage, "AOACenter");
            GameObject aoaDown = AircraftAPI.GetChildWithName(cage, "AOADown");

            VTText up = aoaUp.GetComponentInChildren<VTText>();
            VTText mid = aoaCenter.GetComponentInChildren<VTText>();
            VTText down = aoaDown.GetComponentInChildren<VTText>();



            RefuelIndexer refuelIndex = cage.AddComponent<RefuelIndexer>();
            refuelIndex.ready = AircraftAPI.ReplaceVTText(refuelReady, ready);
            refuelIndex.latched = AircraftAPI.ReplaceVTText(refuelLatched, latch);
            refuelIndex.disconnected = AircraftAPI.ReplaceVTText(refuelDisc, disconnect);
            refuelIndex.port = Fa26.GetComponentInChildren<RefuelPort>();
            refuelIndex.battery = Fa26.GetComponentInChildren<Battery>();


            AoAIndexer indexer = cage.AddComponent<AoAIndexer>();
            indexer.aoaUpText = AircraftAPI.ReplaceVTText(aoaUp, up);
            indexer.aoaDownText = AircraftAPI.ReplaceVTText(aoaDown, down);
            indexer.aoaCenterText = AircraftAPI.ReplaceVTText(aoaCenter, mid);
            indexer.battery = Fa26.GetComponentInChildren<Battery>();
            indexer.flightInfo = Fa26.GetComponentInChildren<FlightInfo>();
            indexer.gear = Fa26.GetComponentInChildren<GearAnimator>();

        }

        //Call this method after things like gauges and systems are set up
        public static void SetUpKnobs()
        {
            VRTwistKnobInt fuelKnob = AircraftAPI.GetChildWithName(customAircraft, "FuelGaugeInteractable").GetComponent<VRTwistKnobInt>();

            foreach(DashFuelNumGauge gauge in customAircraft.GetComponentsInChildren<DashFuelNumGauge>())
            {
                fuelKnob.OnSetState.AddListener(gauge.ChangeDisplayValue);
            }

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
                engine.maxThrust = 50f;
                engine.fuelDrain = 0.8f;
            }

            VRThrottle throttle = Fa26.GetComponentInChildren<VRThrottle>(true);
            throttle.abGate = false;
            throttle.abGateThreshold = 1.1f;

        }

        public static void SetUpMass()
        {
            MassUpdater faMass = Fa26.GetComponent<MassUpdater>();
            faMass.baseMass = 10.5f;



        }

        public static void SetUpFlightModel()
        {
            SimpleDrag drag = Fa26.GetComponent<SimpleDrag>();
            //drag.area = 0.25f;

            //Changes flight characteristics
            Wing wingLeft = AircraftAPI.GetChildWithName(Fa26, "wingLeftAero").GetComponent<Wing>();
            Wing wingRight = AircraftAPI.GetChildWithName(Fa26, "wingRightAero").GetComponent<Wing>();

            //wingLeft.liftCoefficient = 0.4f;
            wingLeft.liftArea = 4;

            wingRight.liftArea = 4;
            //wingRight.liftCoefficient = 0.4f;

            Wing flapLeft = AircraftAPI.GetChildWithName(Fa26, "flapLeftAero").GetComponent<Wing>();
            flapLeft.liftArea = 2.4f;

            Wing flapRight = AircraftAPI.GetChildWithName(Fa26, "flapRightAero").GetComponent<Wing>();
            flapRight.liftArea = 2.4f;


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

        public static void SetUpWingDamage()
        {
            GameObject leftWing = AircraftAPI.GetChildWithName(customAircraft, "LeftOuterWing");
            leftWing.transform.SetParent(AircraftAPI.GetChildWithName(Fa26, "wingLeftPart").transform, true);

            GameObject rightWing = AircraftAPI.GetChildWithName(customAircraft, "RightOuterWing");
            rightWing.transform.SetParent(AircraftAPI.GetChildWithName(Fa26, "wingRightPart").transform, true);
        }

        public static void SetUpFlares()
        {
            Fa26.GetComponentInChildren<FlareCountermeasure>(true).maxCount = 240;
        }

        public static void SetUpAutoCMS()
        {
            AutoCMS cms = Fa26.AddComponent<AutoCMS>();
            cms.flares = Fa26.GetComponentInChildren<FlareCountermeasure>(true);
            cms.chaff = Fa26.GetComponentInChildren<ChaffCountermeasure>(true);
            cms.mws = Fa26.GetComponentInChildren<MissileDetector>(true);
            cms.manager = Fa26.GetComponentInChildren<CountermeasureManager>(true);
            cms.rwr = Fa26.GetComponentInChildren<ModuleRWR>(true);

            VRThrottle throttle = Fa26.GetComponentInChildren<VRThrottle>();
            throttle.OnMenuButtonDown = new UnityEvent();
            throttle.OnMenuButtonUp = new UnityEvent();

            throttle.OnMenuButtonDown.AddListener(cms.StartCMS);
            throttle.OnMenuButtonUp.AddListener(cms.StopCMS);


        }
        
        public static void SetUpFrontCMSPanel()
        {
            Battery battery = Fa26.GetComponentInChildren<Battery>(true);


            GameObject bottomLeftText = AircraftAPI.GetChildWithName(customAircraft, "CMS_BOT_LEFT");
            PanelMWS panelMws = bottomLeftText.AddComponent<PanelMWS>();
            panelMws.text = bottomLeftText.GetComponent<TextMeshPro>();
            panelMws.battery = battery;
            panelMws.detector = Fa26.GetComponentInChildren<MissileDetector>(true);

            GameObject topRightText = AircraftAPI.GetChildWithName(customAircraft, "CMS_TOP_RIGHT");
            PanelCMS panelCms = topRightText.AddComponent<PanelCMS>();
            panelCms.text = topRightText.GetComponent<TextMeshPro>();
            panelCms.battery = battery;
            panelCms.flares = Fa26.GetComponentInChildren<FlareCountermeasure>(true);
            panelCms.chaff = Fa26.GetComponentInChildren<ChaffCountermeasure>(true);
            panelCms.autoCms = Fa26.GetComponentInChildren<AutoCMS>(true);

        }

        public static void SetUpSideCMSPanel()
        {
            Battery battery = Fa26.GetComponentInChildren<Battery>(true);

            GameObject cm1 = AircraftAPI.GetChildWithName(customAircraft, "CMSP1");
            PanelCMSP1 cmsp1 = cm1.AddComponent<PanelCMSP1>();
            cmsp1.text = cmsp1.GetComponent<TextMeshPro>();
            cmsp1.battery = battery;
            cmsp1.chaff = Fa26.GetComponentInChildren<ChaffCountermeasure>(true);
            cmsp1.autoCms = Fa26.GetComponentInChildren<AutoCMS>(true);

            GameObject cm2 = AircraftAPI.GetChildWithName(customAircraft, "CMSP2");
            PanelCMSP2 cmsp2 = cm2.AddComponent<PanelCMSP2>();
            cmsp2.text = cmsp2.GetComponent<TextMeshPro>();
            cmsp2.battery = battery;
            cmsp2.flares = Fa26.GetComponentInChildren<FlareCountermeasure>(true);
            cmsp2.autoCms = Fa26.GetComponentInChildren<AutoCMS>(true);

            GameObject cm3 = AircraftAPI.GetChildWithName(customAircraft, "CMSP3");
            PanelCMSP3 cmsp3 = cm3.AddComponent<PanelCMSP3>();
            cmsp3.text = cmsp3.GetComponent<TextMeshPro>();
            cmsp3.battery = battery;
            cmsp3.autoCms = Fa26.GetComponentInChildren<AutoCMS>(true);

            GameObject cm4 = AircraftAPI.GetChildWithName(customAircraft, "CMSP4");
            PanelCMSP4 cmsp4 = cm4.AddComponent<PanelCMSP4>();
            cmsp4.text = cmsp4.GetComponent<TextMeshPro>();
            cmsp4.battery = battery;
            cmsp4.autoCms = Fa26.GetComponentInChildren<AutoCMS>(true);


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

        public static void SetUpMFD()
        {
            foreach (Text text in AircraftAPI.GetChildWithName(Fa26, "MFD1").GetComponentsInChildren<Text>(true))
            {
                text.color = new Color32(21, 175, 37, 255);
            }

            foreach (Text text in AircraftAPI.GetChildWithName(Fa26, "MFD2").GetComponentsInChildren<Text>(true))
            {
                text.color = new Color32(21, 175, 37, 255);
            }
            Debug.Log("Set all MFD Text to Green");
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

        public static void SetUpFormationLights()
        {
            SwitchableMaterialEmission emission = customAircraft.GetComponentInChildren<SwitchableMaterialEmission>(true);
            emission.battery = Fa26.GetComponentInChildren<Battery>(true);

            VRTwistKnob formationKnob = AircraftAPI.FindInteractable(customAircraft, "Formation Lights").GetComponent<VRTwistKnob>();
            formationKnob.OnSetState.AddListener(new UnityAction<float>((num) =>
            {
                if (num > 0)
                {
                    emission.SetEmission(true);
                }
                else
                {
                    emission.SetEmission(false);
                }

                emission.SetEmissionMultiplier(num);
            }
            ));

        }

        public static void SetUpRWR()
        {
            //Main.instance.StartCoroutine(RWRRoutine());
        }


        public static IEnumerator RWRRoutine()
        {
            yield return new WaitForSeconds(1);
            DashRWR rwr = AircraftSetup.Fa26.GetComponentInChildren<DashRWR>(true);

            foreach (var mfd in Fa26.GetComponentsInChildren<MFD>(true))
            {
                if (mfd.name == "MiniMFDLeft")
                {
                    Debug.Log("1");
                    mfd.SetPage("rwr");
                    if (!mfd.powerOn)
                    {
                        Debug.Log("2");
                        mfd.TogglePower();
                        Debug.Log("3");
                    }


                    Traverse.Create(mfd).Field("homePage").SetValue(mfd.manager.GetPage("rwr"));
                    //UnityEngine.Object.Destroy(AircraftAPI.GetChildWithName(AircraftAPI.GetChildWithName(mfd.manager.transform.parent.gameObject, "BG"), "RWRText")); // Text gets in the way


                    break;
                }
            }

        }

        public static void ChangeRWRIcon()
        {
            Fa26.GetComponentInChildren<Radar>(true).radarSymbol = "custom";
        }

    }
}
