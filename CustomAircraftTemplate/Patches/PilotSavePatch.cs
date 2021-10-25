using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace A10Mod
{
    [HarmonyPatch(typeof(PilotSave), nameof(PilotSave.SaveToConfigNode))]
    class PilotSavePatch
    {

        public static bool Prefix(PilotSave ps, ref ConfigNode __result)
        {

			ConfigNode configNode = new ConfigNode("PILOTSAVE");
			configNode.SetValue<string>("pilotName", ps.pilotName);

			Traverse psTraverse = Traverse.Create(ps);

			Dictionary<string, VehicleSave> vehicleSaves = (Dictionary<string, VehicleSave>)psTraverse.Field("vehicleSaves").GetValue();

			foreach(VehicleSave vs in vehicleSaves.Values)
			{
				if(vs.vehicleName == AircraftInfo.AircraftName)
                {
					continue;
                }

				ConfigNode node = VehicleSave.SaveToConfigNode(vs);
				configNode.AddNode(node);
			}
			configNode.SetValue<string>("lastVehicleUsed", ps.lastVehicleUsed);
			configNode.SetValue<string>("totalFlightTime", ConfigNodeUtils.WriteObject(ps.totalFlightTime));
			configNode.SetValue<string>("skinColor", ConfigNodeUtils.WriteVector3(ColorUtils.ToVector3(ps.skinColor)));
			configNode.SetValue<string>("suitColor", ConfigNodeUtils.WriteVector3(ColorUtils.ToVector3(ps.suitColor)));
			configNode.SetValue<string>("vestColor", ConfigNodeUtils.WriteVector3(ColorUtils.ToVector3(ps.vestColor)));
			configNode.SetValue<string>("gSuitColor", ConfigNodeUtils.WriteVector3(ColorUtils.ToVector3(ps.gSuitColor)));
			configNode.SetValue<string>("strapsColor", ConfigNodeUtils.WriteVector3(ColorUtils.ToVector3(ps.strapsColor)));

			__result = configNode;
			return false;
		}
    }
}
