using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace CustomItem
{
    internal static class Assets
    {
        internal static GameObject BiscoLeashPrefab;
        internal static ItemIndex BiscoLeashItemIndex;
        internal static EquipmentIndex BiscoLeashEquipmentIndex;

        private const string ModPrefix = "@CustomItem:";
        private const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/belt_icon/belt_icon.png";

        internal static void Init()
        {
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomItem.rampage")) 
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                BiscoLeashPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            }

            BiscoLeashAsWhiteTierItem();
           // BiscoLeashAsEquipment();

            AddLanguageTokens();
        }

        private static void BiscoLeashAsWhiteTierItem()
        {
            var biscoLeashItemDef = new ItemDef
            {
                name = "BiscosLeash", // its the internal name, no spaces, apostrophes and stuff like that
                //tier = ItemTier.Tier1,
                tier = ItemTier.Lunar,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "BISCOLEASH_NAME", // stylised name
                pickupToken = "BISCOLEASH_PICKUP",
                descriptionToken = "BISCOLEASH_DESC",
                loreToken = "BISCOLEASH_LORE",
                tags = new[]
                {
                    ItemTag.Utility,
                    ItemTag.Damage
                }
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var biscoLeash = new R2API.CustomItem(biscoLeashItemDef, itemDisplayRules);

            BiscoLeashItemIndex = ItemAPI.Add(biscoLeash); // ItemAPI sends back the ItemIndex of your item
        }

        private static void AddLanguageTokens()
        {
            //The Name should be self explanatory
            R2API.AssetPlus.Languages.AddToken("BISCOLEASH_NAME", "Bisco's Leash");
            //The Pickup is the short text that appears when you first pick this up. This text should be short and to the point, nuimbers are generally ommited.
            R2API.AssetPlus.Languages.AddToken("BISCOLEASH_PICKUP", "Gain Rampage stack on kill");
            //The Description is where you put the actual numbers and give an advanced description.
            R2API.AssetPlus.Languages.AddToken("BISCOLEASH_DESC",
                "Grants <style=cDeath>RAMPAGE</style> on kill. \n<style=cDeath>RAMPAGE</style> : Specifics rewards for reaching kill streaks. \nIncreases <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cIsDamage>(+1% per item stack)</style> <style=cStack>(+1% every 20 Rampage Stacks)</style>. \nIncreases <style=cIsUtility>damage</style> by <style=cIsUtility>2%</style> <style=cIsDamage>(+2% per item stack)</style> <style=cStack>(+2% every 20 Rampage Stacks)</style>.");
            //The Lore is, well, flavor. You can write pretty much whatever you want here.
            R2API.AssetPlus.Languages.AddToken("BISCOLEASH_LORE",
                "You were always there, by my side, whether we sat or played. Our friendship was a joyful ride, I wish you could have stayed.");
        }
    }
}
