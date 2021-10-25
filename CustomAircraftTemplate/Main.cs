using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace A10Mod
{
    public class Main : VTOLMOD
    {

        
        public static Main instance;

        //Stores a prefab of the aircraft in order to spawn it in whenever you want
        public static GameObject aircraftPrefab;
        public static GameObject ECMJammerPrefab;
        public static Sprite hudMaskSprite;
        public static Texture a10Sprite;

        //Gun audio clips
        public static AudioClip gauFireClip;
        public static AudioClip gauEndClip;

        //RWR audio clips
        public static AudioClip missileLaunchClip;
        public static AudioClip radarBlipClip;
        public static AudioClip radarLockClip;
        public static AudioClip newContactClip;


        public static GameObject playerGameObject;
        public MpPlugin plugin = null;


        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {

            

            HarmonyInstance harmonyInstance = HarmonyInstance.Create(AircraftInfo.HarmonyId);
            harmonyInstance.PatchAll();

            instance = this;


            string pathToBundle = Path.Combine(instance.ModFolder, AircraftInfo.AircraftAssetbundleName);

            Debug.Log(pathToBundle);
            aircraftPrefab = FileLoader.GetAssetBundleAsGameObject(pathToBundle, AircraftInfo.AircraftPrefabName);
            Debug.Log("Got le " + aircraftPrefab.name);

            //Loads in the jammer
            /*GameObject ANAL = FileLoader.GetAssetBundleAsGameObject(pathToBundle, "A-10C_ECM_Jammer.prefab"); // throwback moment
            ECMJammerPrefab = Instantiate(ANAL);
            ECMJammerPrefab.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
            HPEquippable analEquip = ECMJammerPrefab.AddComponent<HPEquipRadarJammer>();

            analEquip.allowedHardpoints = "1";
            analEquip.shortName = "ECM Jammer";
            analEquip.name = "AN/ALQ-131";
            analEquip.fullName = "AN/ALQ-131";
            analEquip.description = "A jamming pod capable of jamming AAA, SAM, or Air radars.";
            analEquip.jettisonable = false;
            analEquip.armable = false;
            analEquip.unitCost = 20000f;
            AircraftAPI.GetChildWithName(ECMJammerPrefab, "A-10C_ECM_Jammer_Model").AddComponent<RadarJammer>();
            DontDestroyOnLoad(ECMJammerPrefab);
            ECMJammerPrefab.SetActive(false);
            
            Debug.Log("Got le " + ECMJammerPrefab.name);
            */

            //Loads in the a10 sprites
            string pathToSprites = Path.Combine(instance.ModFolder, "a10sprites");
            hudMaskSprite = (Sprite)FileLoader.GetAssetBundleAsType<Sprite>(pathToSprites, "Sqaure_Mask");
            Debug.Log("Got le " + hudMaskSprite.name);

            a10Sprite = (Texture)FileLoader.GetAssetBundleAsType<Texture>(pathToSprites, "A10_Sprite");
            Debug.Log("Got le " + a10Sprite.name);

            gauFireClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "GAU8Cannon");
            Debug.Log("Got le " + gauFireClip.name);


            gauEndClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "GAU8End");
            Debug.Log("Got le " + gauEndClip.name);


            radarBlipClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "RadarSearchA10");
            Debug.Log("Got le " + radarBlipClip.name);

            radarLockClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "EnemyRadarLOCKA10");
            Debug.Log("Got le " + radarLockClip.name);

            newContactClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "NewContactA10");
            Debug.Log("Got le " + newContactClip.name);

            missileLaunchClip = (AudioClip)FileLoader.GetAssetBundleAsType<AudioClip>(pathToSprites, "MissleLaunchWarningA10");
            Debug.Log("Got le " + missileLaunchClip.name);

            //Adds the custom plane to the main menu
            StartCoroutine(AircraftAPI.CreatePlaneMenuItem());



            //This is an event the VTOLAPI calls when the game is done loading a scene
            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();

            this.checkMPloaded();


        }

        public void Start()
        {
            Debug.Log("New save path: " + PilotSaveManager.newSaveDataPath);

        }
        public void checkMPloaded()
        {
            FlightLogger.Log("checking Multiplayer is installed");
            List<Mod> list = new List<Mod>();
            list = VTOLAPI.GetUsersMods();
            foreach (Mod mod in list)
            {
                bool flag = mod.isLoaded && mod.name.Contains("ultiplayer");
                if (flag)
                {
                    this.plugin = new MpPlugin();
                    this.plugin.MPlock();
                }
            }
        }



        //This method is called every frame by Unity. Here you'll probably put most of your code
        void Update()
        {
            AircraftInfo.AircraftSelected = true;
        }

        //This method is like update but it's framerate independent. This means it gets called at a set time interval instead of every frame. This is useful for physics calculations
        void FixedUpdate()
        {

        }

        //This function is called every time a scene is loaded. this behaviour is defined in Awake().
        private void SceneLoaded(VTOLScenes scene)
        {
            //If you want something to happen in only one (or more) scenes, this is where you define it.

            //For example, lets say you're making a mod which only does something in the ready room and the loading scene. This is how your code could look:
            switch (scene)
            {
                case VTOLScenes.ReadyRoom:
                    //Add your ready room code here
                    this.checkMPloaded();
                    break;
                case VTOLScenes.LoadingScene:
                    //Add your loading scene code here
                    break;
            }
        }
    }
}