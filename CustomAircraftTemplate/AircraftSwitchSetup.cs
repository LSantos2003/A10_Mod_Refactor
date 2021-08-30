using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace A10Mod
{
    class AircraftSwitchSetup
    {


        public static bool quickStart = false;
        public static bool hotStart = false;


        public static void SetUpBottomLeft()
        {
            

            WeaponManager wm = AircraftSetup.Fa26.GetComponentInChildren<WeaponManager>(true);

            VRLever altModeSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Alt Select").GetComponent<VRLever>();
            altModeSwitch.OnSetState.AddListener(AircraftSetup.Fa26.GetComponent<VehicleMaster>().SetRadarAltMode);

            VRLever masterArmSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Master Arm").GetComponent<VRLever>();
            masterArmSwitch.OnSetState.AddListener(AircraftSetup.Fa26.GetComponentInChildren<WeaponManagerUI>().UISetMasterArm);


            VRLever tgpSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "TGP Arm").GetComponent<VRLever>();
            TargetingMFDPage targetPage = AircraftSetup.Fa26.GetComponentInChildren<TargetingMFDPage>(true);
            tgpSwitch.OnSetState.AddListener(new UnityAction<int>((num) =>
            {
                if (!targetPage.powered && num == 1)
                {
                    targetPage.TGPPwrButton();

                }
                else if (targetPage.powered && num == 0)
                {
                    targetPage.TGPPwrButton();
                }

            }
            ));


            ApplyColorToAllUI changedHud = AircraftSetup.Fa26.GetComponentInChildren<ApplyColorToAllUI>(true);
            VRLever hudColorSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "HUD Color").GetComponent<VRLever>();
            hudColorSwitch.OnSetState.AddListener(delegate (int num)
            {
                //GameObject HUD = AircraftAPI.GetChildWithName(AircraftSetup.customAircraft, "HUDParent");
                if (num > 0)
                {
                    changedHud.color = new Color32(163, 255, 79, 216);

                }
                else
                {
                    changedHud.color = new Color32(140, 91, 27, 250);

                }

                changedHud.apply = true;
            }
            );

            /*HPEquippable gunEquip = wm.GetEquip(0);
            VRLever gunArmLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Gun Arm").GetComponent<VRLever>();
            gunArmLever.OnSetState.AddListener(new UnityAction<int>((num) =>
            {
                if (num > 0)
                {
                    gunEquip.armed = true;
                }
                else 
                { 
                    gunEquip.armed = false;
                    wm.CycleActiveWeapons();
                }


            }
        ));*/


            VRLever hudToggleLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "HUD Toggle").GetComponent<VRLever>();
            hudToggleLever.OnSetState.AddListener(AircraftAPI.GetChildWithName(AircraftSetup.Fa26, "HUDCanvas").GetComponent<ObjectPowerUnit>().SetConnection);

            VTOLQuickStart quickStart = AircraftSetup.Fa26.GetComponentInChildren<VTOLQuickStart>(true);

            quickStart.quickStartComponents.OnApplySettings.AddListener(new UnityAction(() =>
            {
                hudToggleLever.RemoteSetState(1);

            }
            ));
        }




        public static void SetUpEngineStart()
        {


        }

        public static void SetUpGenerators()
        {

            Battery battery = AircraftSetup.Fa26.GetComponentInChildren<Battery>(true);
         
            VRLever batteryLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Battery").GetComponent<VRLever>();
            batteryLever.OnSetState.AddListener(battery.SetConnection);

            ModuleEngine engineL = null;
            ModuleEngine engineR = null;
            foreach (ModuleEngine engine in AircraftSetup.Fa26.GetComponentsInChildren<ModuleEngine>(true))
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

            VRLever leftEngineLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Left Engine Start").GetComponent<VRLever>();
            leftEngineLever.OnSetState.AddListener(engineL.SetPower);

            VRLever rightEngineLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Right Engine Start").GetComponent<VRLever>();
            rightEngineLever.OnSetState.AddListener(engineR.SetPower);


            VRLever apuLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "APU Start").GetComponent<VRLever>();
            apuLever.OnSetState.AddListener(AircraftSetup.Fa26.GetComponentInChildren<AuxilliaryPowerUnit>(true).SetPower);


            VTOLQuickStart quickStart = AircraftSetup.Fa26.GetComponentInChildren<VTOLQuickStart>(true);

            quickStart.quickStartComponents.OnApplySettings.AddListener(new UnityAction(() =>
            {
                batteryLever.RemoteSetState(1);
                leftEngineLever.RemoteSetState(1);
                rightEngineLever.RemoteSetState(1);

            }
            ));

        }

        public static void SetUpRightPanel()
        {
            DashRWR rwr = AircraftSetup.Fa26.GetComponentInChildren<DashRWR>(true);
            MissileDetector mws = rwr.missileDetector;

            VRLever mwsToggleLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "MWS Switch").GetComponent<VRLever>();
            mwsToggleLever.OnSetState.AddListener(new UnityAction<int>((num) =>
            {
                mws.enabled = num == 1 || num == 2;
            }));

            MFD mmfdLeft = null;
            foreach (var mfd in AircraftSetup.Fa26.GetComponentsInChildren<MFD>(true))
            {
                if (mfd.name == "MiniMFDLeft")
                {
                    mmfdLeft = mfd;
                    break;
                }
            }


            VRLever rwrSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "RWR Switch").GetComponent<VRLever>();
            rwrSwitch.OnSetState.AddListener(new UnityAction<int>((num) =>
            {
                if (!mmfdLeft.powerOn)
                {
                    mmfdLeft.TogglePower();
                }

                if (num == 0)
                {
                    rwr.SetMasterMode((int)DashRWR.RWRModes.Off);
                    // Traverse.Create(rwr).Field("detectedPings").SetValue(new Dictionary<int, RWRIcon>());
                }
                else if (num == 1)
                {
                    mmfdLeft.SetPage("rwr");
                    rwr.SetMasterMode((int)DashRWR.RWRModes.Silent);
                }
                else
                {
                    mmfdLeft.SetPage("rwr");
                    rwr.SetMasterMode((int)DashRWR.RWRModes.On);

                }

            })); // you can tell where i took over this code  

            VRLever cmsSwitch = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "CMS Dispenser").GetComponent<VRLever>();
            cmsSwitch.OnSetState.AddListener(AircraftSetup.Fa26.GetComponentInChildren<CMSConfigUI>(true).SetFlares);
            cmsSwitch.OnSetState.AddListener(AircraftSetup.Fa26.GetComponentInChildren<CMSConfigUI>(true).SetChaff);
            cmsSwitch.OnSetState.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP1>(true).SetPage);
            cmsSwitch.OnSetState.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP2>(true).SetPage);
            cmsSwitch.OnSetState.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP3>(true).SetPage);
            cmsSwitch.OnSetState.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP4>(true).SetPage);

            VRInteractable set1 = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "SET CHAFF");
            set1.OnInteract.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP1>(true).IncrementChaffQuantity);

            VRInteractable set2 = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "SET FLARE");
            set2.OnInteract.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP2>(true).IncrementFlareQuantity);

            VRInteractable set3 = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "SET INTERVAL");
            set3.OnInteract.AddListener(AircraftSetup.customAircraft.GetComponentInChildren<PanelCMSP3>(true).IncrementReleaseInterval);


            VRTwistKnobInt cmsKnob = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "CMS Mode Select").GetComponent<VRTwistKnobInt>();
            cmsKnob.OnSetState.AddListener(AircraftSetup.Fa26.GetComponentInChildren<AutoCMS>(true).SetCMSMode);

            VTOLQuickStart quickStart = AircraftSetup.Fa26.GetComponentInChildren<VTOLQuickStart>(true);

            quickStart.quickStartComponents.OnApplySettings.AddListener(new UnityAction(() =>
            {
                cmsSwitch.RemoteSetState(1);
                mwsToggleLever.RemoteSetState(1);
                rwrSwitch.RemoteSetState(2);

            }
            ));
        }


        public static void SetUpEmergency()
        {
            WeaponManager wm = AircraftSetup.Fa26.GetComponentInChildren<WeaponManager>(true);

            VRInteractable jettisonButton = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Emergency Jettison");
            jettisonButton.OnInteract.AddListener(new UnityAction(() =>
            {

                WeaponManagerUI ui = AircraftSetup.Fa26.GetComponentInChildren<WeaponManagerUI>(true);
                ui.MarkAllJettison();

                for (int i = 0; i < wm.equipCount; i++)
                {
                    if (wm.GetEquip(i) && wm.GetEquip(i).weaponType == HPEquippable.WeaponTypes.AAM)
                    {
                        wm.GetEquip(i).markedForJettison = false;
                    }
                }
                ui.JettisonMarkedItems();
            }
            ));


            AuxilliaryPowerUnit apu = AircraftSetup.Fa26.GetComponentInChildren<AuxilliaryPowerUnit>(true);
            ModuleEngine engineL = null;
            ModuleEngine engineR = null;
            foreach (ModuleEngine engine in AircraftSetup.Fa26.GetComponentsInChildren<ModuleEngine>(true))
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

            EjectHandle rightEject = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Right EXT").GetComponent<EjectHandle>();
            rightEject.OnHandlePull.AddListener(new UnityAction(() => { ExtinguishFire(engineR); }));

            EjectHandle leftEject = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "Left EXT").GetComponent<EjectHandle>();
            leftEject.OnHandlePull.AddListener(new UnityAction(() => { ExtinguishFire(engineL); }));

            EjectHandle apuEject = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "APU Ext").GetComponent<EjectHandle>();
            apuEject.OnHandlePull.AddListener(new UnityAction(() => { RepairAPU(apu); }));


        }


        private static void ExtinguishFire(ModuleEngine engine)
        {


            if (UnityEngine.Random.Range(1, 5) == 1)
            {
                foreach (var vp in engine.GetComponentsInChildren<VehiclePart>())
                {
                    vp.Repair();
                }
            }
            else
            {
                // bool enginesFailed = engine.failed;
                foreach (var vp in engine.GetComponentsInChildren<VehiclePart>())
                {
                    vp.Repair();
                }
                //if (enginesFailed)
                //  engine.FailEngine(); // this will have the effect of getting rid of the fire but still failing the engine
            }
            FlightLogger.Log("Temperz has tried to repair the engine!"); // IM A FUNCTION MORTYYYYYYYYYYYYYY
        }


        private static void RepairAPU(AuxilliaryPowerUnit apu)
        {
            apu.RepairUnit();
            if (UnityEngine.Random.Range(1, 5) == 1)
            {
                apu.RepairUnit();
            }
            FlightLogger.Log("Temperz has tried to repair the apu!");
        }


       

    }
}
