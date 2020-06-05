using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using UnityEngine;
using RoR2;
using BepInEx.Configuration;



using Path = System.IO.Path;




namespace CustomItem
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(AssetPlus), nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]

    public class CustomItem : BaseUnityPlugin
    {

        public static ConfigEntry<double> EnemyChanceToStealItem;
        public static ConfigEntry<int> EnemyItemDivisor;
        public static ConfigEntry<double> PlayerChanceToStealItem;


        public static BuffIndex indexNoHealBuff;
        private const string ModVer = "1.0.0";
        private const string ModName = "ItemSteal";
        public const string ModGuid = "com.SirHamburger.LunarStealItems";

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
            
            ConfigFile cfgFile = new ConfigFile(Path.Combine(Paths.ConfigPath, ModGuid + ".cfg"), true);
            EnemyChanceToStealItem = cfgFile.Bind(new ConfigDefinition("Global.VanillaTweaks", "EnemyChanceToStealItems"), 0.5, new ConfigDescription(
                "Base chance in percent that enemys steal items from you (basechance*(itemcount/EnemyItemDivisor)"));   
            EnemyItemDivisor = cfgFile.Bind(new ConfigDefinition("Global.VanillaTweaks", "EnemyItemDivisor"), 20, new ConfigDescription(
                "Enemy item devisior for stealing items from you (basechance*(itemcount/EnemyItemDivisor)"));  
            PlayerChanceToStealItem = cfgFile.Bind(new ConfigDefinition("Global.VanillaTweaks", "PlayerChanceToStealItem"), 8.0, new ConfigDescription(
                "Chance in percent that you steal items from the enemy")); 

                  indexNoHealBuff = BuffAPI.Add(noHeal);
            Assets.Init();
            Hooks.Init();
        }
    }
}