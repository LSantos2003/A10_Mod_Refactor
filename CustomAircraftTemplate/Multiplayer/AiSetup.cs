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
			mover.load(false);

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

			SetUpEngines(aiObject, aircraft);
			SetUpWheels(aiObject, aircraft);
			SetUpRadarIcon(aiObject);
			//DisableGun(aiObject);
		}

		public static void DisableGun(GameObject aiAircraft)
        {

			Gun gun = aiAircraft.GetComponentInChildren<Gun>(true);
			AircraftAPI.DisableMesh(gun.gameObject);
		}


		public static void SetUpWheels(GameObject aiAircraft, GameObject customAircraft)
		{
			WheelsController wheelController = aiAircraft.GetComponent<WheelsController>();
			CopyRotation copyRot = AircraftAPI.GetChildWithName(customAircraft, "FrontWheelPivot").GetComponentInChildren<CopyRotation>(true);
			copyRot.target = wheelController.steeringTransform;

			SuspensionWheelAnimator frontSus = AircraftAPI.GetChildWithName(aiAircraft, "FrontGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
			SuspensionWheelAnimator leftSus = AircraftAPI.GetChildWithName(aiAircraft, "LeftGear").GetComponentInChildren<SuspensionWheelAnimator>(true);
			SuspensionWheelAnimator rightSus = AircraftAPI.GetChildWithName(aiAircraft, "RightGear").GetComponentInChildren<SuspensionWheelAnimator>(true);

			SuspensionWheelAnimator frontSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "FrontWheelPivot").GetComponentInChildren<SuspensionWheelAnimator>(true);
			SuspensionWheelAnimator leftSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "GearLeft").GetComponentInChildren<SuspensionWheelAnimator>(true);
			SuspensionWheelAnimator rightSusCustomAircraft = AircraftAPI.GetChildWithName(customAircraft, "GearRight").GetComponentInChildren<SuspensionWheelAnimator>(true);

			frontSusCustomAircraft.suspension = frontSus.suspension;
			leftSusCustomAircraft.suspension = leftSus.suspension;
			rightSusCustomAircraft.suspension = rightSus.suspension;

		}


		public static void SetUpEngines(GameObject aiAircraft, GameObject customAircraft)
        {
			foreach(EngineRotator r in customAircraft.GetComponentsInChildren<EngineRotator>(true))
            {
				r.engine = aiAircraft.GetComponentInChildren<ModuleEngine>(true);
            }

			foreach(ModuleEngine e in aiAircraft.GetComponentsInChildren<ModuleEngine>(true))
			{
				e.autoAB = false;
            }
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
