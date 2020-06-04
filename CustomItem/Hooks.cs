using BepInEx;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BepInEx.Configuration;
using Mono.Cecil.Cil;
using System;
using TMPro;
using UnityEngine.Networking;
using Path = System.IO.Path;
using System.Collections.ObjectModel;
using TILER2;
using static TILER2.MiscUtil;


namespace CustomItem
{

    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]

    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(ResourcesAPI), nameof(PlayerAPI), nameof(PrefabAPI), nameof(BuffAPI), nameof(LoadoutAPI))]

    public class Hooks
    {
        internal static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                try
                {
                    if (damageInfo.attacker)
                    {
                        Inventory Inv = damageInfo.attacker.GetComponent<CharacterBody>().inventory;
                        Inventory EnemyInventory = self.GetComponent<CharacterBody>().inventory;
                        float BoxingGloveCount = Inv.GetItemCount(Assets.BiscoLeashItemIndex);


                        //if(!self.body.isPlayerControlled&&BoxingGloveCount>0)
                        //{
                        //    var count = Inv.GetItemCount(ItemIndex.ExtraLife);
                        //    if(count >0)
                        //        Inv.RemoveItem(ItemIndex.ExtraLife,count);
                        //}
                        //DamageInfo bla = new DamageInfo
                        //{
                        //    damage = 0f,
                        //    damageColorIndex = DamageColorIndex.Default,
                        //    damageType = DamageType.Generic,
                        //    attacker = null,
                        //    crit = damageInfo.crit,
                        //    force = damageInfo.force,
                        //    inflictor = null,
                        //    position = damageInfo.position,
                        //    procChainMask = damageInfo.procChainMask,
                        //    procCoefficient = 0f
                        //};

                        var cb = damageInfo.attacker.GetComponent<CharacterBody>();
                        float chance = (float)(BoxingGloveCount) * 5;
                        bool shouldTrigger = Util.CheckRoll(chance, cb.master);
                        if (shouldTrigger)
                        {
                            List<ItemIndex> lstItemIndex = new List<ItemIndex>();
                            foreach (var element in ItemCatalog.allItems)
                            {
                                if (EnemyInventory.GetItemCount(element) > 0)
                                {
                                    lstItemIndex.Add(element);
                                }
                            }
                            var rand = new System.Random();
                            int randomPosition = rand.Next(0, lstItemIndex.Count - 1);
                            ItemIndex itemToRemove = lstItemIndex[randomPosition];
                            EnemyInventory.RemoveItem(itemToRemove, 1);
                            Chat.AddMessage("Removed " + itemToRemove);



                            //EnemyInventory.RemoveItem(ItemIndex.ExtraLife, 1);   
                            self.GetComponent<CharacterBody>().AddBuff(CustomItem.indexNoHealBuff);
                            //self.GetComponent<CharacterBody>().AddTimedBuff(CustomItem.indexNoHealBuff,60);
                            //self.TakeDamage(bla);
                        }
                    }
                    if(damageInfo.inflictor)
                    {
                        Inventory Inv = damageInfo.attacker.GetComponent<CharacterBody>().inventory;
                        Inventory EnemyInventory = self.GetComponent<CharacterBody>().inventory;
                        float BoxingGloveCount = EnemyInventory.GetItemCount(Assets.BiscoLeashItemIndex);
                        var cb = damageInfo.attacker.GetComponent<CharacterBody>();
                        float chance = (float)(BoxingGloveCount* 0.5) ;
                        bool shouldTrigger = Util.CheckRoll(chance, cb.master);
                        if (shouldTrigger)
                        {
                            List<ItemIndex> lstItemIndex = new List<ItemIndex>();
                            foreach (var element in ItemCatalog.allItems)
                            {
                                if (EnemyInventory.GetItemCount(element) > 0)
                                {
                                    lstItemIndex.Add(element);
                                }
                            }
                            var rand = new System.Random();
                            int randomPosition = rand.Next(0, lstItemIndex.Count - 1);
                            ItemIndex itemToRemove = lstItemIndex[randomPosition];
                            if(itemToRemove != Assets.BiscoLeashItemIndex)
                            {
                                EnemyInventory.RemoveItem(itemToRemove, 1);
                                Chat.AddMessage("Removed " + itemToRemove + " from " + damageInfo.attacker.name);
                                self.GetComponent<CharacterBody>().AddBuff(CustomItem.indexNoHealBuff);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    RoR2.Console.print(e);
                }

                orig(self, damageInfo); // dont forget this !
            };
            On.RoR2.UI.ItemInventoryDisplay.OnInventoryChanged += (On.RoR2.UI.ItemInventoryDisplay.orig_OnInventoryChanged orig, RoR2.UI.ItemInventoryDisplay self) =>
            {
                orig(self);
            };
            On.RoR2.GlobalEventManager.OnInteractionBegin += (On.RoR2.GlobalEventManager.orig_OnInteractionBegin orig, global::RoR2.GlobalEventManager self, global::RoR2.Interactor interactor, IInteractable interactable, GameObject interactableObject) =>
              {
                  orig(self, interactor, interactable, interactableObject);
              };
            On.RoR2.GenericPickupController.GrantItem += (On.RoR2.GenericPickupController.orig_GrantItem orig, global::RoR2.GenericPickupController self, global::RoR2.CharacterBody body, global::RoR2.Inventory inventory) =>
            {
                RoR2.Console.print("----------------grant item---------------");
                if (inventory.GetItemCount(Assets.BiscoLeashItemIndex) > 0)
                {
                    if (inventory.GetItemCount(ItemIndex.ExtraLife) > 0)
                    {
                        inventory.RemoveItem(ItemIndex.ExtraLife, inventory.GetItemCount(ItemIndex.ExtraLife));
                    }
                    if (self.pickupIndex.itemIndex == ItemIndex.ExtraLife)
                        return;
                    if (self.pickupIndex.itemIndex == Assets.BiscoLeashItemIndex)
                        inventory.RemoveItem(ItemIndex.ExtraLife, inventory.GetItemCount(ItemIndex.ExtraLife));
                }
                if (self.pickupIndex.itemIndex == Assets.BiscoLeashItemIndex)
                    inventory.RemoveItem(ItemIndex.ExtraLife, inventory.GetItemCount(ItemIndex.ExtraLife));
                orig(self, body, inventory);
            };
        }
    }
}
