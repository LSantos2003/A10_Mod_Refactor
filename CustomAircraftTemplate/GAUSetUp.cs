using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VTOLVR.Multiplayer;


namespace A10Mod
{
    class GAUSetUp
    {
        private static WeaponManager weaponManager;
        public static void AddGau(LoadoutConfigurator config)
        {
            //Waits for loadout configuator to exist

            //Gets the gau-8 gameobject
            string path = PilotSaveManager.GetVehicle("AV-42C").equipsResourcePath + "/" + "gau-8";
            PlayerVehicle av = VTResources.GetPlayerVehicle("AV-42C");
            GameObject gau = av.GetEquipPrefab("gau-8");

            GameObject instGau = GameObject.Instantiate(gau);
            instGau.name = "gau-8";
            instGau.SetActive(false);
            EqInfo value = new EqInfo(instGau, path);

            Traverse loadTraverse = Traverse.Create(config);
            Debug.Log("1 ran");

            Dictionary<string, EqInfo> unlockedWeapons = (Dictionary<string, EqInfo>)loadTraverse.Field("unlockedWeaponPrefabs").GetValue();
            if (!unlockedWeapons.ContainsKey("gau-8"))
            {
                unlockedWeapons.Add(instGau.name, value);
                loadTraverse.Field("unlockedWeaponPrefabs").SetValue(unlockedWeapons);
            }
            else
            {
                unlockedWeapons.Remove(gau.name);
                loadTraverse.Field("unlockedWeaponPrefabs").SetValue(unlockedWeapons);
                unlockedWeapons.Add(instGau.name, value);
                loadTraverse.Field("unlockedWeaponPrefabs").SetValue(unlockedWeapons);
            }

            Debug.Log("2 ran");

            Dictionary<string, EqInfo> allWeapons = (Dictionary<string, EqInfo>)loadTraverse.Field("allWeaponPrefabs").GetValue();
            if (!allWeapons.ContainsKey("gau-8"))
            {
                allWeapons.Add(gau.name, value);
                loadTraverse.Field("allWeaponPrefabs").SetValue(allWeapons);
                Debug.Log("Already had gau");
            }
            else
            {
                allWeapons.Remove(gau.name);
                loadTraverse.Field("allWeaponPrefabs").SetValue(allWeapons);
                allWeapons.Add(gau.name, value);
                loadTraverse.Field("allWeaponPrefabs").SetValue(allWeapons);
                Debug.Log("Didn't have gau");
            }
            Debug.Log("3 ran");

            //Changes stats of gun
            Gun gun = instGau.GetComponentInChildren<Gun>(true);
            gun.maxAmmo = 1300;
            gun.currentAmmo = 1300;
            gun.recoilFactor = 5f;

            //  config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
            config.OnAttachHPIdx += HideGau;
            config.OnAttachHPIdx += SetUpGun;
            config.AttachImmediate("gau-8", 0);

            if (!VTOLMPUtils.IsMultiplayer())
            {

                GameObject go = VTOLAPI.GetPlayersVehicleGameObject();
                WeaponManager wm = go.GetComponent<WeaponManager>();
                weaponManager = wm;


                AircraftAPI.DisableMesh(weaponManager.hardpointTransforms[0].gameObject);
            }
           
        }

        private static void HideGau(int value)
        {
            if (value == 0)
            {
                AircraftAPI.DisableMesh(weaponManager.hardpointTransforms[0].gameObject);
            }
        }

        public static void SetUpGun(int value)
        {
            Gun gun = weaponManager.hardpointTransforms[0].gameObject.GetComponentInChildren<Gun>(true);
            //HPEquipGun gunEquip = Fa26.GetComponentInChildren<HPEquipGun>(true);
            if (gun != null)
            {

                gun.maxAmmo = 1300;
                gun.currentAmmo = 1300;
                gun.recoilFactor = 2.5f;
                Traverse gunTraverse = Traverse.Create(gun);
                gunTraverse.Field("hasEjectTf").SetValue(false);
                gun.gameObject.GetComponent<GunBarrelRotator>().rotationTransform = AircraftAPI.GetChildWithName(AircraftSetup.customAircraft, "BarrelTf").transform;
                gun.audioProfiles[0].firingSound = Main.gauFireClip;
                gun.audioProfiles[0].stopFiringSound = Main.gauEndClip;
                gun.audioProfiles[0].audioSource.pitch = 1f;
                AircraftAPI.DisableMesh(gun.gameObject);

                //gun.gameObject.GetComponent<GunBarrelRotator>().rotationTransform = AircraftAPI.GetChildWithName(customAircraft, "BarrelTf").transform;

            }
        }

    }
}
