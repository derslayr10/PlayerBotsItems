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

    public class EngiBotSpawner : ItemBase
    {
        public override string ItemName => "3NG";

        public override string ItemLangTokenName => "ENGI_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Engineer!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{Num3NGBotsSpawned} per stack)</style> of Engineer.";

        public override string ItemLore => "Need advanced targeting... Deploying unit, codename:  3NG";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Engi/3NG.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Engi/3NG.png";

        public int Num3NGBotsSpawned;

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

            Num3NGBotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many 3NG bots should spawn when item is picked up?").Value;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private int botEngiCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            SurvivorIndex index = SurvivorIndex.Engi;

            if ((self.playerCharacterMasterController != null) && (self.inventory.GetItemCount(Index) > 0) && (self.inventory.GetItemCount(Index) > botEngiCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, Num3NGBotsSpawned);
                botEngiCount = GetCount(self);

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
