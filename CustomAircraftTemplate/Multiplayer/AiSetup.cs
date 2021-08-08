using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace A10Mod
{
    class AiSetup : MonoBehaviour
    {
		private static Vector3 aircraftLocalPosition = new Vector3(0.127f, -1.971f, 0.389f);
		private static Vector3 aircraftLocalEuler = new Vector3(0.02f, 89.00f, 1.04f);
		private static Vector3 aircraftLocalScale = Vector3.one * 2.745594f;

		public static void CreateAi(GameObject aiObject)
        {
			UnityMover mover = aiObject.gameObject.AddComponent<UnityMover>();
			mover.gs = aiObject.gameObject;
			mover.FileName = AircraftInfo.AIUnityMoverFileName;
			//mover.load(false);

			Disable26MeshAi(aiObject);

			GameObject aircraft = Instantiate<GameObject>(Main.aircraftPrefab);
			aircraft.transform.SetParent(aiObject.transform);
			aircraft.transform.localPosition = aircraftLocalPosition;
			aircraft.transform.localEulerAngles = aircraftLocalEuler;
			aircraft.transform.localScale = aircraftLocalScale;

			AIPilot aiPilot = aiObject.GetComponentInChildren<AIPilot>();
			GearAnimator gearAnim = aiPilot.gearAnimator;

			AnimationToggle animToggle = AircraftAPI.GetChildWithName(aircraft, "GearAnimator").GetComponent<AnimationToggle>();
			gearAnim.OnOpen.AddListener(new UnityAction(animToggle.Retract));
			gearAnim.OnClose.AddListener(new UnityAction(animToggle.Deploy));

			CreateControlSurfaces(aiObject, aircraft);

			SetUpRefuelPort(aiObject, aircraft);


			SetUpRadarIcon(aiObject);
		}

		public static void Disable26MeshAi(GameObject go)
		{
			WeaponManager componentInChildren = go.GetComponentInChildren<WeaponManager>(true);
			AircraftAPI.DisableMesh(go, componentInChildren);
			
		}

		public static void CreateControlSurfaces(GameObject aiAircraft, GameObject customAircraft)
		{

			AeroController controller = aiAircraft.GetComponentInChildren<AeroController>();

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
		}

		public static void SetUpRCS(GameObject aiAircraft)
		{
			RadarCrossSection rcs = aiAircraft.GetComponent<RadarCrossSection>();
			rcs.size = 7.381652f;
			rcs.overrideMultiplier = 0.5f;

			foreach (HPEquippable hp in aiAircraft.GetComponentsInChildren<HPEquippable>(true))
			{
				hp.rcsMasked = true;
			}
		}

		public static void SetUpRefuelPort(GameObject aiAircraft, GameObject customAircraft)
		{
			RefuelPort port = aiAircraft.GetComponentInChildren<RefuelPort>();
			AnimationToggle animToggle = AircraftAPI.GetChildWithName(customAircraft, "FuelPort_Animation_Parent").GetComponent<AnimationToggle>();

			port.OnOpen.AddListener(animToggle.Deploy);
			port.OnClose.AddListener(animToggle.Retract);

		}

		public static void SetUpRadarIcon(GameObject aiAircraft)
        {
			Radar radar = aiAircraft.GetComponentInChildren<Radar>(true);
			radar.radarSymbol = "10";

        }


	}




}
