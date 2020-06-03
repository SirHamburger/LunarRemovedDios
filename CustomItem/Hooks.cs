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
                 if (damageInfo.attacker)
                {
                    
                    Inventory Inv = damageInfo.attacker.GetComponent<CharacterBody>().inventory;
                    Inventory EnemyInventory = self.GetComponent<CharacterBody>().inventory;
                    float BoxingGloveCount = Inv.GetItemCount(Assets.BiscoLeashItemIndex);


                    if(!self.body.isPlayerControlled&&BoxingGloveCount>0)
                    {
                        var count = Inv.GetItemCount(ItemIndex.ExtraLife);
                        if(count >0)
                            Inv.RemoveItem(ItemIndex.ExtraLife,count);
                    }
                    DamageInfo bla = new DamageInfo
                    {
                        damage = 0f,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        attacker = null,
                        crit = damageInfo.crit,
                        force = damageInfo.force,
                        inflictor = null,
                        position = damageInfo.position,
                        procChainMask = damageInfo.procChainMask,
                        procCoefficient = 0f
                    };
                    
                    var cb = damageInfo.attacker.GetComponent<CharacterBody>();
                    float chance = (float)(BoxingGloveCount)*5;
                    bool shouldTrigger = Util.CheckRoll(chance,cb.master);
                    if (shouldTrigger)
                    {
                        EnemyInventory.RemoveItem(ItemIndex.ExtraLife, 1);   
                        self.GetComponent<CharacterBody>().AddBuff(CustomItem.indexNoHealBuff);
                        //self.GetComponent<CharacterBody>().AddTimedBuff(CustomItem.indexNoHealBuff,60);
                        self.TakeDamage(bla);
                    }
                }

                orig(self, damageInfo); // dont forget this !
            };
        }
    }
}
