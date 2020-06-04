using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using UnityEngine;
using RoR2;

namespace CustomItem
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(AssetPlus), nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class CustomItem : BaseUnityPlugin
    {
        public static BuffIndex indexNoHealBuff;
        private const string ModVer = "1.0.0";
        private const string ModName = "MyCustomItemPlugin";
        public const string ModGuid = "com.MyName.MyCustomItemPlugin";

        internal new static ManualLogSource Logger; // allow access to the logger across the plugin classes

        public void Awake()
        {
            Logger = base.Logger;
            var noHeal = new R2API.CustomBuff("noHeal" ,new BuffDef{
                        buffColor = Color.red,
                        canStack = true,
                        isDebuff = true,
                        name = "NoHeal",
                    });
                    
                    

                  indexNoHealBuff = BuffAPI.Add(noHeal);
            Assets.Init();
            Hooks.Init();
        }
    }
}