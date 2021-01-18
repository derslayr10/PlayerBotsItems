using BepInEx;
using R2API;
using R2API.Utils;
using System.Reflection;
using UnityEngine;
using PlayerBotsItemsMod.Items;
using System.Collections.Generic;

namespace PlayerBotsItemsMod{

	[BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
	[BepInDependency("com.meledy.PlayerBots", BepInDependency.DependencyFlags.HardDependency)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(ModGuid, ModName, ModVer)]
	[R2APISubmoduleDependency(nameof(ResourcesAPI), nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI))]
	
	public class PlayerBotsItems : BaseUnityPlugin{

		public const string ModGuid = "com.Derslayr.PlayerBotsItems";
		public const string ModName = "PlayerBotsItems";
		public const string ModVer = "0.1.0";

		public List<ItemBase> Items = new List<ItemBase>();

		public void Awake(){

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerBotsItemsMod.playerbotsitems_assets")) {

				var bundle = AssetBundle.LoadFromStream(stream);
				var provider = new AssetBundleResourcesProvider("@PlayerBotsItems", bundle);
				ResourcesAPI.AddProvider(provider);

			}

			VerifyItem(new CaptainBotSpawner(), Items);

			foreach (ItemBase item in Items) {

				item.Init(base.Config);
			
			}

		}

		public void VerifyItem(ItemBase item, List<ItemBase> itemList)
		{

			var isEnabled = Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Enable this item to appear in game?").Value;

			if (isEnabled) {

				itemList.Add(item);
			
			}

		}
	}
}