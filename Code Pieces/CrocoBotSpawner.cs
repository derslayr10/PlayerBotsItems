using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerBots;
using UnityEngine;
using static PlayerBotsItemsMod.Utils.ItemHelpers;

namespace PlayerBotsItemsMod.Items
{

    public class CrocoBotSpawner : ItemBase
    {
        public override string ItemName => "4CRD";

        public override string ItemLangTokenName => "CROCO_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Acrid!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{Num4CRDBotsSpawned} per stack)</style> of Acrid.";

        public override string ItemLore => "To beat an abomination, you need to use one... Deploying unit, codename:  4CRD";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Croco/4CRD.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Croco/4CRD.png";

        public int Num4CRDBotsSpawned;

        public static GameObject ItemBodyModelPrefab;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {

            Num4CRDBotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many 4CRD bots should spawn when item is picked up?").Value;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private int botAcridCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            SurvivorIndex index = SurvivorIndex.Croco;

            if ((self.playerCharacterMasterController != null) && (self.inventory.GetItemCount(Index) > 0) && (self.inventory.GetItemCount(Index) > botAcridCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, Num4CRDBotsSpawned);

                botAcridCount = GetCount(self);

            }

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = Resources.Load<GameObject>(ItemModelPath);
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
            {

                new RoR2.ItemDisplayRule
               {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(1, 1, 1)
                }

            });

            return rules;

        }
    }

}
