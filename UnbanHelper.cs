using System;
using Exiled.API.Features;
using Handlers = Exiled.Events.Handlers;
using Newtonsoft.Json;
using System.IO;
using HarmonyLib;

namespace UnbanHelper
{
	public class Plugin : Plugin<Config>
	{
		public override string Author { get; } = "Wafel";
		public override string Name { get; } = "UnbanHelper";
		public override string Prefix { get; } = "UH";
		public override Version Version { get; } = new Version(1, 0, 0);
		public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);


		public static Plugin Singleton;
		public PlayerEvents PlayerEvents;
		public Harmony Instance;

		public override void OnEnabled()
		{

			base.OnEnabled();

			Singleton = this;
			PlayerEvents = new PlayerEvents(this);

			Handlers.Player.Banned += PlayerEvents.OnPlayerBanned;
			Handlers.Player.Banning += PlayerEvents.OnPlayerBanning;
			Handlers.Player.Joined += PlayerEvents.OnPlayerJoined;
			Handlers.Player.ChangingMuteStatus += PlayerEvents.OnMute;
			Handlers.Player.ChangingIntercomMuteStatus += PlayerEvents.OnIcomMute;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Handlers.Player.Banned -= PlayerEvents.OnPlayerBanned;
			Handlers.Player.Banning -= PlayerEvents.OnPlayerBanning;
			Handlers.Player.Joined -= PlayerEvents.OnPlayerJoined;
			Handlers.Player.ChangingMuteStatus -= PlayerEvents.OnMute;
			Handlers.Player.ChangingIntercomMuteStatus -= PlayerEvents.OnIcomMute;

			PlayerEvents = null;
		}
	}
}
