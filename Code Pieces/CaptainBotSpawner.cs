using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerBots;
using UnityEngine;
using static PlayerBotsItemsMod.Utils.ItemHelpers;

namespace PlayerBotsItemsMod.Items{

    public class CaptainBotSpawner : ItemBase
    {
        public override string ItemName => "C4PTN";

        public override string ItemLangTokenName => "CAPTAIN_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Captain!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{NumC4PTNBotsSpawned} per stack)</style> of Captain.";

        public override string ItemLore => "Sometimes... new recruits just aren't enough. Deploying unit, codename:  C4PTN";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Captain/C4PTN.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Captain/C4PTN.png";

        public int NumC4PTNBotsSpawned;

        public static GameObject ItemBodyModelPrefab;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config) {

            NumC4PTNBotsSpawned = config.Bind<int>("Item: " + ItemName, "Number of Bots Spawned", 1, "How many C4PTN bots should spawn when item is picked up?").Value;
            
        }

        public override void Hooks()
        {
            
            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private IReadOnlyCollection<PlayerCharacterMasterController> GetPlayers() {

            var players = PlayerCharacterMasterController.instances;

            return players;
        
        }

        private int botCapCount = 0;

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {

            orig(self);

            SurvivorIndex index = SurvivorIndex.Captain;

            if ((self.playerCharacterMasterController != null) && (GetCount(self) > 0) && (GetCount(self) > botCapCount))
            {

                PlayerBotManager.SpawnPlayerbots(self, index, NumC4PTNBotsSpawned);
                botCapCount = GetCount(self);

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
