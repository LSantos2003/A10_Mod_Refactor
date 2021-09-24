using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace A10Mod
{

    [HarmonyPatch(typeof(LoadoutConfigurator), "Initialize")]
    public static class LoadoutConfigStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance)
        {
            return true;

            if (!AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) return true;

            AircraftAPI.GetChildWithName(__instance.gameObject, "title").gameObject.GetComponent<Text>().text = AircraftInfo.AircraftName;


            Transform parent = AircraftAPI.GetChildWithName(__instance.gameObject, "vtImage").transform;

            parent.gameObject.GetComponent<RawImage>().texture = Main.a10Sprite;

            const string hpInfo = "HardpointInfo";
            
            parent.Find(hpInfo + " (11)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (12)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (14)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (15)").gameObject.SetActive(false);

            return true;
        }


        public static void Postfix(LoadoutConfigurator __instance)
        {

            if (!AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) return;

            GAUSetUp.AddGau(__instance);
            __instance.lockedHardpoints.Add(0);
            GAUSetUp.SetUpGun(0);


            AircraftAPI.GetChildWithName(__instance.gameObject, "title").gameObject.GetComponent<Text>().text = AircraftInfo.AircraftName;


            Transform parent = AircraftAPI.GetChildWithName(__instance.gameObject, "vtImage").transform;

            parent.gameObject.GetComponent<RawImage>().texture = Main.a10Sprite;

            const string hpInfo = "HardpointInfo";

            parent.Find(hpInfo + " (11)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (12)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (14)").gameObject.SetActive(false);
            parent.Find(hpInfo + " (15)").gameObject.SetActive(false);

   

            //Detaches the weapons from the aircraft
            Main.instance.StartCoroutine(DetachRoutine(__instance));




        }

        //Coroutine that detaches all the weapons
        public static IEnumerator DetachRoutine(LoadoutConfigurator config)
        {
            yield return new WaitForSeconds(1);
            GAUSetUp.SetUpGun(0);
            config.DetachImmediate(11);
            config.DetachImmediate(12);
            config.DetachImmediate(14);
            config.DetachImmediate(15);
            Debug.Log("Hardpoint count: " + config.wm.hardpointTransforms.Length);
            
        }

    }
    [HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
    public static class EquipComaptibilityPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, HPEquippable equip)
        {


            if (!AircraftInfo.AircraftSelected) return true;



            if (true) // fuck you c ; work on manners you ape
            {
                Debug.Log("Section 11");


                // this creates a dictionary of all the wepaons and where they can be mounted, just alter the second string per weapon according to the wepaon you want.
                Dictionary<string, string> allowedhardpointbyweapon = new Dictionary<string, string>();
                allowedhardpointbyweapon.Add("fa26-cft", "");
                allowedhardpointbyweapon.Add("fa26_agm89x1", "");
                allowedhardpointbyweapon.Add("fa26_agm161", "");
                allowedhardpointbyweapon.Add("fa26_aim9x2", "1, 10");
                allowedhardpointbyweapon.Add("fa26_aim9x3", "");
                allowedhardpointbyweapon.Add("fa26_cagm-6", "");
                allowedhardpointbyweapon.Add("fa26_cbu97x1", "1,2,3,10,7,6,5,4, 13, 9");
                allowedhardpointbyweapon.Add("fa26_droptank", "7,4, 13");
                allowedhardpointbyweapon.Add("fa26_droptankXL", "");
                allowedhardpointbyweapon.Add("fa26_gbu12x1", "1, 10,7,6,5,4, 13,2, 9");
                allowedhardpointbyweapon.Add("fa26_gbu12x2", "");
                allowedhardpointbyweapon.Add("fa26_gbu12x3", "7,4,3");
                allowedhardpointbyweapon.Add("fa26_gbu38x1", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("fa26_gbu38x2", "");
                allowedhardpointbyweapon.Add("fa26_gbu38x3", "7,4");
                allowedhardpointbyweapon.Add("fa26_gbu39x4uFront", "");
                allowedhardpointbyweapon.Add("fa26_gbu39x4uRear", "");
                // allowedhardpointbyweapon.Add("fa26_gun", "0");
                allowedhardpointbyweapon.Add("fa26_harmx1", "");
                allowedhardpointbyweapon.Add("fa26_harmx1dpMount", "");
                allowedhardpointbyweapon.Add("fa26_iris-t-x1", "");
                allowedhardpointbyweapon.Add("fa26_iris-t-x2", "");
                allowedhardpointbyweapon.Add("fa26_iris-t-x3", "");
                allowedhardpointbyweapon.Add("fa26_maverickx1", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("fa26_maverickx3", "3,7,6,5,4");
                allowedhardpointbyweapon.Add("fa26_mk82HDx1", "10,8,6,5,3,1, 13, 2, 9");
                allowedhardpointbyweapon.Add("fa26_mk82HDx2", "");
                allowedhardpointbyweapon.Add("fa26_mk82HDx3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("fa26_mk82x2", "");
                allowedhardpointbyweapon.Add("fa26_mk82x3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("fa26_mk83x1", "3, 13, 7, 4, 9, 5, 6, 8");
                allowedhardpointbyweapon.Add("fa26_sidearmx1", "");
                allowedhardpointbyweapon.Add("fa26_sidearmx2", "");
                allowedhardpointbyweapon.Add("fa26_sidearmx3", "");
                allowedhardpointbyweapon.Add("fa26_tgp", "9,2");
                allowedhardpointbyweapon.Add("cagm-6", "");
                allowedhardpointbyweapon.Add("cbu97x1", "3,7,6,5,4,13,9");
                allowedhardpointbyweapon.Add("gbu38x1", "10,8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("gbu38x2", "");
                allowedhardpointbyweapon.Add("gbu38x3", "7,6,5,4");
                allowedhardpointbyweapon.Add("gbu39x3", "");
                allowedhardpointbyweapon.Add("gbu39x4u", "");
                allowedhardpointbyweapon.Add("h70-4x4", "");
                allowedhardpointbyweapon.Add("h70-x7", "9,2,8,7,6,5,4,3,2");
                allowedhardpointbyweapon.Add("h70-x19", "");
                allowedhardpointbyweapon.Add("hellfirex4", "");
                allowedhardpointbyweapon.Add("iris-t-x1", "");
                allowedhardpointbyweapon.Add("iris-t-x2", "");
                allowedhardpointbyweapon.Add("iris-t-x3", "");
                allowedhardpointbyweapon.Add("m230", "");
                allowedhardpointbyweapon.Add("marmx1", "");
                allowedhardpointbyweapon.Add("maverickx1", "8,7,6,5,4,3,2");
                allowedhardpointbyweapon.Add("maverickx3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("mk82HDx1", "10,8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("mk82HDx2", "");
                allowedhardpointbyweapon.Add("mk82HDx3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("mk82x1", "1,10,8,7,6,5,4,3, 13");
                allowedhardpointbyweapon.Add("mk82x2", "");
                allowedhardpointbyweapon.Add("mk82x3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("sidearmx1", "");
                allowedhardpointbyweapon.Add("sidearmx2", "");
                allowedhardpointbyweapon.Add("sidearmx3", "");
                allowedhardpointbyweapon.Add("sidewinderx1", "");
                allowedhardpointbyweapon.Add("sidewinderx2", "");
                allowedhardpointbyweapon.Add("sidewinderx3", "");
                allowedhardpointbyweapon.Add("af_aim9", "1,10");
                allowedhardpointbyweapon.Add("af_amraam", "");
                allowedhardpointbyweapon.Add("af_amraamRail", "");
                allowedhardpointbyweapon.Add("af_amraamRailx2", "");
                allowedhardpointbyweapon.Add("af_dropTank", "13");
                allowedhardpointbyweapon.Add("af_maverickx1", "8,7,6,5,4,3,2");
                allowedhardpointbyweapon.Add("af_maverickx3", "8,7,6,5,4,3");
                allowedhardpointbyweapon.Add("af_mk82", "1,3,10, 13, 2, 9,8");
                allowedhardpointbyweapon.Add("af_tgp", "9,2,13");
                allowedhardpointbyweapon.Add("h70-x7ld", "2,8,3,9");
                allowedhardpointbyweapon.Add("h70-x7ld-under", "7,6,5,4");
                allowedhardpointbyweapon.Add("h70-x14ld-under", "");
                allowedhardpointbyweapon.Add("h70-x14ld", "");


                Debug.Log("Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);



                if (allowedhardpointbyweapon.ContainsKey(equip.name))
                {
                    equip.allowedHardpoints = (string)allowedhardpointbyweapon[equip.name];
                    Debug.Log("Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);
                }
                else
                {
                    Debug.Log("Equipment: " + equip.name + ", not in dictionary");
                }
            }
            //equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
            return true;
        }
    }

}
