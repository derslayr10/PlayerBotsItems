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

    public class LoaderBotSpawner : ItemBase
    {
        public override string ItemName => "L0DR";

        public override string ItemLangTokenName => "LOADER_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Loader!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{NumL0DRBotsSpawned} per stack)</style> of Loader.";

        public override string ItemLore => "Time for some heavy lifting... Deploying unit, codename:  L0DR";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Loader/L0DR.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Loader/L0DR.png";

        public int NumL0DRBotsSpawned;

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

            NumL0DRBotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many L0DR bots should spawn when item is picked up?").Value;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private int botLoadCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            SurvivorIndex index = SurvivorIndex.Loader;

            if ((self.playerCharacterMasterController != null) && (self.inventory.GetItemCount(Index) > 0) && (self.inventory.GetItemCount(Index) > botLoadCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, NumL0DRBotsSpawned);
                botLoadCount = GetCount(self);

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