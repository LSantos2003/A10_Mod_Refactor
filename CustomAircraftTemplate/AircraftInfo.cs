using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10Mod
{
    public class AircraftInfo
    {

        //READ ME, IMPORTANT!!!!!!!!
        //You must change HarmonyId in order for your custom aircraft mod to be compatable with other aircraft mods
        public const string HarmonyId = "C137.Warthog";

        //Stores if your custom aircraft is selected.
        //This is what prevents your aircraft from constantly replacing the FA-26
        public static bool AircraftSelected = false;

        //Info about your aircraft
        public const string AircraftName = "A-10D";
        public const string AircraftNickName = "Warthog";
        public const string AircraftDescription = "\"Warthog\" A dedicated close air support platform. Well known for its durability, endurance, and its 30mm cannon";

        //Names of the various files you need to put in your builds folder
        public const string PreviewPngFileName = "A10_Preview.png";
        public const string AircraftAssetbundleName = "a10c";
        public const string UnityMoverFileName = "warthogPositions.surg";
        public const string AIUnityMoverFileName = "warthogPositionsAI.surg";

        //Name of the prefab of your aircraft from the assetbundle
        public const string AircraftPrefabName = "A10_V3.prefab";



    }
}
