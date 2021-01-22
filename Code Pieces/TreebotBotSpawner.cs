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

    public class TreebotBotSpawner : ItemBase
    {
        public override string ItemName => "R3KS";

        public override string ItemLangTokenName => "TREEBOT_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Rex!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{NumR3KSBotsSpawned} per stack)</style> of Rex.";

        public override string ItemLore => "Moranis help us all... Deploying unit, codename:  R3KS";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Treebot/R3KS.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Treebot/R3KS.png";

        public int NumR3KSBotsSpawned;

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

            NumR3KSBotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many R3KS bots should spawn when item is picked up?").Value;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private int botRexCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            SurvivorIndex index = SurvivorIndex.Treebot;

            if ((self.playerCharacterMasterController != null) && (GetCount(self) > 0) && (GetCount(self) > botRexCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, NumR3KSBotsSpawned);
                botRexCount = GetCount(self);
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