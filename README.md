# PlayerBotsItems
Mod that utilizes the Player Bots Mod to create in game items that function similar to the Queen's Gland boss tier item.

Mod currently has items for all vanilla survivors. All items are Legendary Tier items.

# Config options
Each item has a config option that allows you to enable or disable the item, as well as change the number of each bot that spawns upon picking up the item for that survivor.

In addition to this, the mod also inherits Config Changes from PlayerBots, so be sure to look over those config options for further customization.

# Known Issues
- Currently there is a visual bug where the bots do not appear as "owned" by the player that picks up the item, but rather appears that the host of the game owns them.
- Until the host of the game dies, the other players may see the bots on the "Minion Display" on the HUD as Beetle Guards, however this appears to be fixed upon death of the host.
- Due to the nature of the AI from PlayerBots, the survivor bots occasionally glitch and will run in place or get stuck on terrain.
- The bots use similar AI pathing to drones, so the first priority is to kill enemies before following player. As such, they can get scattered and lost easily.
- By default, the bots spawn as Players, and therefore may bypass the restriction that AI cannot pick up the bot items. This will result in a wasted purchase from the bot, as the PlayerBots mod handles them. This will not spawn a new bot, the one picking up the item MUST be a player for a bot to spawn.

# Planned Updates
- Make bots teleport to their owner upon activation of the teleporter event

# Changelog
- v1.1.0: Fixed issue where bots were spawning on every item pickup.

- v1.0.0: Initial release of mod.