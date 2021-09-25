using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod.Patches
{
    [HarmonyPatch(typeof(MFD), nameof(MFD.Initialize))]
    class Patch_MFDButtons
    {
        public static bool Prefix(MFD __instance, MFDPage homePage)
        {
            if (homePage.buttons == null || homePage.buttons.Length == 0)
                return true;
            MFDPage.MFDButtonInfo[] newButtons = new MFDPage.MFDButtonInfo[14];
            int trueIDX = 0;
            Debug.Log("The length of homepage buttons is " + homePage.buttons.Length);
            for (int i = 0; i < 14 && trueIDX < homePage.buttons.Length; i++)
            {
                if (homePage.buttons[trueIDX].label == "RADAR")
                {
                    Debug.Log("Found radar page.");
                    trueIDX++;
                }
                newButtons[i] = homePage.buttons[trueIDX];
                Debug.Log($"i: {i}, trueIDX: {trueIDX}.");
                trueIDX++;
            }
            homePage.buttons = newButtons;
            return true;
        }
    }

    [HarmonyPatch(typeof(MFDPage), nameof(MFDPage.Awake))]
    class Patch_TGPButtons
    {
        public static bool Prefix(MFDPage __instance)
        {
            if (__instance.pageName == "target")
            {
                MFDPage.MFDButtonInfo[] newButtons = new MFDPage.MFDButtonInfo[12];
                int trueIDX = 0;
                Debug.Log("The length of tgp buttons is " + __instance.buttons.Length);
                for (int i = 0; i < 12 && trueIDX < __instance.buttons.Length; i++)
                {
                    if (__instance.buttons[trueIDX].label == "PWR")
                    {
                        Debug.Log("Found pwr page.");
                        trueIDX++;
                    }
                    newButtons[i] = __instance.buttons[trueIDX];
                    Debug.Log($"i: {i}, trueIDX: {trueIDX}.");
                    trueIDX++;
                }
                __instance.buttons = newButtons;
            }
            return true;
        }
    }
}
