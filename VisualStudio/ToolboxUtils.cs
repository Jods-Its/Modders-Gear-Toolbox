using UnityEngine.AddressableAssets;
using UnityEngine;
using Il2Cpp;

namespace GearToolbox
{
    internal static class ToolboxUtils
    {
        public static GameObject flint = Addressables.LoadAssetAsync<GameObject>("GEAR_Flint").WaitForCompletion();
        public static GameObject rock = Addressables.LoadAssetAsync<GameObject>("GEAR_Rock").WaitForCompletion();
        public static GearItem gunParts = Addressables.LoadAssetAsync<GameObject>("GEAR_GunParts").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem wood1 = Addressables.LoadAssetAsync<GameObject>("GEAR_SoftWood").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem wood2 = Addressables.LoadAssetAsync<GameObject>("GEAR_HardWood").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem scrap = Addressables.LoadAssetAsync<GameObject>("GEAR_ScrapMetal").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem bones = Addressables.LoadAssetAsync<GameObject>("GEAR_Bones").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem tools1 = Addressables.LoadAssetAsync<GameObject>("GEAR_SimpleTools").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem tools2 = Addressables.LoadAssetAsync<GameObject>("GEAR_HighQualityTools").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem bundleString = Addressables.LoadAssetAsync<GameObject>("GEAR_StringBundle").WaitForCompletion().GetComponent<GearItem>();
        public static string tools1Name = "GEAR_SimpleTools";
        public static string tools2Name = "GEAR_HighQualityTools";
        public static string bundleStringName = "GEAR_StringBundle";

        public static void LaunchGearToolbox()
        {
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Yellow, "Breaking down objects...");
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Yellow, "Distributing scrap materials...");
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Yellow, "Taperolling...");
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, "Gear Toolbox v1.0.3 Loaded!");
            Settings.instance.AddToModSettings("Modders' Gear Toolbox");
        }
        public static GameObject GetPlayer()
        {
            return GameManager.GetPlayerObject();
        }
        public static bool IsGun(string gearItemName)
        {
            string[] guns = { "GEAR_Rifle", "GEAR_Rifle_Curators", "GEAR_Rifle_Vaughns", "GEAR_Rifle_Barbs", "GEAR_Revolver", "GEAR_RevolverFancy", "GEAR_RevolverGreen", "GEAR_RevolverStubNosed" };
            for (int i = 0; i < guns.Length; i++)
            {
                if (gearItemName == guns[i]) return true;
            }
            return false;
        }
        public static T? GetComponentSafe<T>(this Component? component) where T : Component
        {
            return component == null ? default : GetComponentSafe<T>(component.GetGameObject());
        }
        public static T? GetComponentSafe<T>(this GameObject? gameObject) where T : Component
        {
            return gameObject == null ? default : gameObject.GetComponent<T>();
        }
        public static T? GetOrCreateComponent<T>(this Component? component) where T : Component
        {
            return component == null ? default : GetOrCreateComponent<T>(component.GetGameObject());
        }
        public static T? GetOrCreateComponent<T>(this GameObject? gameObject) where T : Component
        {
            if (gameObject == null)
            {
                return default;
            }

            T? result = GetComponentSafe<T>(gameObject);

            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }
        internal static GameObject? GetGameObject(this Component? component)
        {
            try
            {
                return component == null ? default : component.gameObject;
            }
            catch (System.Exception exception)
            {
                MelonLoader.MelonLogger.Msg($"Returning null since this could not obtain a Game Object from the component. Stack trace:\n{exception.Message}");
            }
            return null;
        }
    }
}
