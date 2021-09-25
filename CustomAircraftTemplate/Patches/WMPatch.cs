using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace A10Mod
{
  

    [HarmonyPatch(typeof(WeaponManager), "EquipWeapons")]
    public static class WmPatch
    {
        public static bool Prefix(WeaponManager __instance, Loadout loadout)
        {
            if (!AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) return true;


            Traverse wm = Traverse.Create(__instance);

            wm.Field("maxAntiAirRange").SetValue(0f);
            wm.Field("maxAntiRadRange").SetValue(0f);
            wm.Field("maxAGMRange").SetValue(0f);

            MassUpdater component = __instance.vesselRB.GetComponent<MassUpdater>();
            HPEquippable[] equips = (HPEquippable[])wm.Field("equips").GetValue();
            for (int i = 0; i < equips.Length; i++)
            {
                if (equips[i] != null)
                {
                    foreach (IMassObject o in equips[i].GetComponentsInChildren<IMassObject>())
                    {
                        component.RemoveMassObject(o);
                    }
                    equips[i].OnUnequip();



                    GameObject.Destroy(equips[i].gameObject);
                    equips[i] = null;
                }
            }
            string[] hpLoadout = loadout.hpLoadout;
            int num = 0;
            while (num < __instance.hardpointTransforms.Length && num < hpLoadout.Length)
            {
                if (!string.IsNullOrEmpty(hpLoadout[num]))
                {
                    string path = __instance.resourcePath + "/" + hpLoadout[num];
                    //Literally copy and pasted the original function, just added this one if sttaement to change the path
                    if (hpLoadout[num] == "gau-8")
                    {
                        Debug.Log("Found gau in loadout");
                        path = PilotSaveManager.GetVehicle("AV-42C").equipsResourcePath + "/" + hpLoadout[num];
                    }
                    UnityEngine.Object @object = Resources.Load(path);
                    if (@object)
                    {
                        GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object, __instance.hardpointTransforms[num]);
                        gameObject.name = hpLoadout[num];
                        gameObject.transform.localRotation = Quaternion.identity;
                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localScale = Vector3.one;
                        HPEquippable component2 = gameObject.GetComponent<HPEquippable>();
                        component2.SetWeaponManager(__instance);
                        equips[num] = component2;
                        component2.wasPurchased = true;
                        component2.hardpointIdx = num;
                        component2.Equip();

                        if (component2.jettisonable)
                        {
                            Rigidbody component3 = component2.GetComponent<Rigidbody>();
                            if (component3)
                            {
                                component3.interpolation = RigidbodyInterpolation.None;
                            }
                        }

                        List<string> uniqueWeapons = (List<string>)wm.Field("uniqueWeapons").GetValue();
                        if (component2.armable)
                        {
                            component2.armed = true;
                            if (!uniqueWeapons.Contains(component2.shortName))
                            {
                                uniqueWeapons.Add(component2.shortName);
                                wm.Field("uniqueWeapons").SetValue(uniqueWeapons);
                            }
                        }
                        gameObject.SetActive(true);
                        foreach (Component component4 in component2.gameObject.GetComponentsInChildren<Component>())
                        {
                            if (component4 is IParentRBDependent)
                            {
                                ((IParentRBDependent)component4).SetParentRigidbody(__instance.vesselRB);
                            }
                            if (component4 is IRequiresLockingRadar)
                            {
                                ((IRequiresLockingRadar)component4).SetLockingRadar(__instance.lockingRadar);
                            }
                            if (component4 is IRequiresOpticalTargeter)
                            {
                                ((IRequiresOpticalTargeter)component4).SetOpticalTargeter(__instance.opticalTargeter);
                            }
                        }
                        if (component2 is HPEquipIRML || component2 is HPEquipRadarML)
                        {
                            if (component2.dlz)
                            {
                                wm.Field("maxAntiAirRange").SetValue(Mathf.Max(component2.dlz.maxLaunchRange, __instance.maxAntiAirRange));

                            }
                        }
                        else if (component2 is HPEquipARML)
                        {
                            if (component2.dlz)
                            {
                                wm.Field("maxAntiRadRange").SetValue(Mathf.Max(component2.dlz.maxLaunchRange, __instance.maxAntiRadRange));

                            }
                        }
                        else if (component2 is HPEquipOpticalML && component2.dlz)
                        {
                            wm.Field("maxAGMRange").SetValue(Mathf.Max(component2.dlz.maxLaunchRange, __instance.maxAGMRange));

                        }
                    }
                }
                num++;
            }
            if (__instance.vesselRB)
            {
                __instance.vesselRB.ResetInertiaTensor();
            }
            if (loadout.cmLoadout != null)
            {
                CountermeasureManager componentInChildren = __instance.gameObject.GetComponentInChildren<CountermeasureManager>();
                if (componentInChildren)
                {
                    int num2 = 0;
                    while (num2 < componentInChildren.countermeasures.Count && num2 < loadout.cmLoadout.Length)
                    {
                        componentInChildren.countermeasures[num2].count = Mathf.Clamp(loadout.cmLoadout[num2], 0, componentInChildren.countermeasures[num2].maxCount);
                        componentInChildren.countermeasures[num2].UpdateCountText();
                        num2++;
                    }
                }
            }

            wm.Field("weaponIdx").SetValue(0);
            __instance.ToggleMasterArmed();
            __instance.ToggleMasterArmed();
            if (__instance.OnWeaponChanged != null)
            {
                __instance.OnWeaponChanged.Invoke();
            }
            component.UpdateMassObjects();

            wm.Field("rcsAddDirty").SetValue(true);
            Debug.Log("New wm thing ran");
            return false;
        }
    }
}
