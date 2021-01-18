using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerBots;

namespace PlayerBotsItemsMod.Items{

    public class CaptainBotSpawner : ItemBase
    {
        public override string ItemName => "C4PTN";

        public override string ItemLangTokenName => "CAPTAIN_BOT_SPAWNER";

        public override string ItemPickupDesc => "Summon a bot of Captain!";

        public override string ItemFullDescription => $"When picked up, will <style=cIsUtility>Summon</style> an AI controlled bot <style=cStack>(+{NumC4PTNBotsSpawned} per stack)</style> of Captain.";

        public override string ItemLore => "Sometimes... new recruits just aren't enough. Deploying unit, codename:  C4PTN";

        public override ItemTier Tier => ItemTier.Tier3;

        public override string ItemModelPath => "@PlayerBotsItems:Assets/Models/Prefabs/Item/Captain/CaptainBody.prefab";

        public override string ItemIconPath => "@PlayerBotsItems:Assets/Textures/Icons/Item/Captain.png";

        public int NumC4PTNBotsSpawned;

        public static GameObjectFactory ItemBodyModelPrefab;

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

        public void SummonBots(CharacterMaster owner, SurvivorIndex index, int NumBotsSpawned) {

            PlayerBots.PlayerBotManager.SpawnPlayerbots(owner, index, NumBotsSpawned);
        
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[] {
            

            
            });

            return rules;

        }

        public override void Hooks()
        {

            On.RoR2.CharacterMaster.OnInventoryChanged += SummonBotsHook;

        }

        private void SummonBotsHook(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            bool flag = self.inventory;

            if (flag) {

                int botCount = new CaptainBotSpawner().GetCount(self);

                bool flag2 = botCount > 0;
                if (flag2) {

                    SummonBots(self, SurvivorIndex.Captain, NumC4PTNBotsSpawned);

                }

            }

            orig(self);
            
        }

    }

}
