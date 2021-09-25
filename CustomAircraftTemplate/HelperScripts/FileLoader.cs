using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace A10Mod
{
    static class FileLoader
    {
        //PUBLIC LOADING METHODS
        //Thanks Baan/GentleLeviathan!!!!
        public static GameObject GetAssetBundleAsGameObject(string path, string name)
        {
            Debug.Log("AssetBundleLoader: Attempting to load AssetBundle...");
            AssetBundle bundle = null;
            try
            {
                bundle = AssetBundle.LoadFromFile(path);
                Debug.Log("AssetBundleLoader: Success.");
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't load AssetBundle from path: '" + path + "'. Exception details: e: " + e.Message);
            }

            Debug.Log("AssetBundleLoader: Attempting to retrieve: '" + name + "' as type: 'GameObject'.");
            try
            {
                var temp = bundle.LoadAsset(name, typeof(GameObject));
                Debug.Log("AssetBundleLoader: Success.");
                bundle.Unload(false);
                return (GameObject)temp;
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't retrieve GameObject from AssetBundle.");
                return null;
            }
        }

        public static UnityEngine.Object GetAssetBundleAsType<T>(string path, string name)
        {
            Debug.Log("AssetBundleLoader: Attempting to load AssetBundle...");
            AssetBundle bundle = null;
            try
            {
                bundle = AssetBundle.LoadFromFile(path);
                Debug.Log("AssetBundleLoader: Success.");
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't load AssetBundle from path: '" + path + "'. Exception details: e: " + e.Message);
            }

            Debug.Log("AssetBundleLoader: Attempting to retrieve: '" + name + "' as " + typeof(T).ToString() + ".");
            try
            {
                var temp = bundle.LoadAsset(name, typeof(T));
                Debug.Log("AssetBundleLoader: Success.");
                bundle.Unload(false);
                return temp;
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't retrieve " + typeof(T).ToString() + " from AssetBundle, name is " + name);
                return null;
            }
        }
    }

}



