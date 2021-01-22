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

    public class CommandoBotSpawner : ItemBase
    {
        public override string ItemName => "C0MND0";

        public override string ItemLangTokenName => "COMMANDO_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Commando!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{NumC0MND0BotsSpawned} per stack)</style> of Commando.";

        public override string ItemLore => "Rookie's gotta learn somehow... Deploying unit, codename:  C0MND0";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Commando/C0MND0.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Commando/C0MND0.png";

        public int NumC0MND0BotsSpawned;

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

            NumC0MND0BotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many C0MND0 bots should spawn when item is picked up?").Value;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private int botComCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            

            SurvivorIndex index = SurvivorIndex.Commando;

            if ((self.playerCharacterMasterController != null) && (GetCount(self) > 0) && (GetCount(self) > botComCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, NumC0MND0BotsSpawned);
                botComCount = GetCount(self);

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
