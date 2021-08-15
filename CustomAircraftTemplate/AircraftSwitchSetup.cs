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

            HPEquippable gunEquip = wm.GetEquip(0);
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
        ));


            VRLever hudToggleLever = AircraftAPI.FindInteractable(AircraftSetup.customAircraft, "HUD Toggle").GetComponent<VRLever>();
            hudToggleLever.OnSetState.AddListener(AircraftAPI.GetChildWithName(AircraftSetup.Fa26, "HUDCanvas").GetComponent<ObjectPowerUnit>().SetConnection);




        }


    }
}
