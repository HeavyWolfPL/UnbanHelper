using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnbanHelper.Objects;

namespace UnbanHelper
{
    public class PlayerEvents
    {
        public Plugin plugin;
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;

        private static readonly string pluginsPath = Path.Combine(Paths.Plugins, "UnbanHelper");
        private readonly string bansPath = Path.Combine(path1: pluginsPath, path2: "BanList.json");

        public void OnPlayerBanned(BannedEventArgs ev)
        {
            if (ev.Type == BanHandler.BanType.UserId)
            {
                try
                {
                    if (!Directory.Exists(pluginsPath))
                        Directory.CreateDirectory(pluginsPath);

                    if (!File.Exists(bansPath))
                        File.Create(bansPath).Close();

                    // Read existing json data
                    var jsonData = File.ReadAllText(bansPath);
                    // De-serialize to object or create new list
                    var bansList = JsonConvert.DeserializeObject<List<PlayerBan>>(jsonData)
                                            ?? new List<PlayerBan>();

                    if (jsonData.Contains(ev.Target.IPAddress))
                    {
                        Log.Debug("IP Adress already in database, canceling.", Plugin.Singleton.Config.Debug);
                        return;
                    }


                    // Add new ban
                    bansList.Add(new PlayerBan()
                    {
                        Name = ev.Details.OriginalName,
                        ID = ev.Target.UserId,
                        IP = ev.Target.IPAddress
                    });

                    // Update json data string
                    jsonData = JsonConvert.SerializeObject(bansList, Formatting.Indented);
                    File.WriteAllText(bansPath, jsonData);

                    Log.Info($"Saving a new ban to the list. \nName: {ev.Details.OriginalName} \nID: {ev.Target.UserId} \nIP: {ev.Target.IPAddress}");

                }
                catch (Exception e)
                {
                    Log.Error($"Saving new ban to the list failed. Please contact the author with this error: \n{e}");
                }
            }
        }

        public void OnPlayerBanning(BanningEventArgs ev)
        {
            if (ev.Issuer == ev.Target)
            {
                ev.IsAllowed = false;
                ev.Issuer.Broadcast(5, "<color=red><b>Co ty robisz? Próbujesz się zbanować?</b></color>", Broadcast.BroadcastFlags.Normal, true);
                Log.Info($"Prevented a selfban by {ev.Issuer.Nickname}.");
            }
            else if (Plugin.Singleton.Config.AsnAntiBan.Contains(ev.Target.ReferenceHub.characterClassManager.Asn))
            {
                ev.IsAllowed = false;
                Log.Info(ev.Duration);
                ev.Issuer.Broadcast(5, "<color=red><b>IP Ban canceled - Whitelisted ASN.</b></color>\nUserID ban issued instead", Broadcast.BroadcastFlags.Normal, true);
                Log.Info($"IP Ban for {ev.Target.Nickname} canceled. ASN is whitelisted. UserID ban issued instead.");
                ev.Target.Disconnect(reason: $"[ThemePark] Zbanowano. \nPowód: {ev.Reason } \nWygaśnięcie bana: {DateTime.UtcNow.AddSeconds(ev.Duration)} \nMożesz odwołać się na discordzie \ndiscord.gg/rAFYcWX");
                Log.Info(DateTime.UtcNow.Ticks);
                Log.Info(DateTime.UtcNow.AddSeconds(ev.Duration).Ticks);
                Log.Info(DateTime.UtcNow);
                Log.Info(DateTime.UtcNow.AddSeconds(ev.Duration));
                BanHandler.IssueBan(new BanDetails()
                {
                    Expires = DateTime.UtcNow.AddSeconds(ev.Duration).Ticks,
                    Id = ev.Target.UserId,
                    IssuanceTime = DateTime.UtcNow.Ticks,
                    Reason = ev.Reason,
                    Issuer = ev.Issuer.Nickname,
                    OriginalName = ev.Target.Nickname,  
                }, BanHandler.BanType.UserId);

                try
                {
                    if (!Directory.Exists(pluginsPath))
                        Directory.CreateDirectory(pluginsPath);

                    if (!File.Exists(bansPath))
                        File.Create(bansPath).Close();

                    // Read existing json data
                    var jsonData = File.ReadAllText(bansPath);
                    // De-serialize to object or create new list
                    var bansList = JsonConvert.DeserializeObject<List<PlayerBan>>(jsonData)
                                            ?? new List<PlayerBan>();

                    if (jsonData.Contains(ev.Target.IPAddress))
                    {
                        Log.Debug("IP Adress already in database, canceling.", Plugin.Singleton.Config.Debug);
                        return;
                    }


                    // Add new ban
                    bansList.Add(new PlayerBan()
                    {
                        Name = ev.Target.Nickname,
                        ID = ev.Target.UserId,
                        IP = ev.Target.IPAddress
                    });

                    // Update json data string
                    jsonData = JsonConvert.SerializeObject(bansList, Formatting.Indented);
                    File.WriteAllText(bansPath, jsonData);

                    Log.Info($"Saving a new ban to the list. \nName: {ev.Target.Nickname} \nID: {ev.Target.UserId} \nIP: {ev.Target.IPAddress}");
                }
                catch (Exception e)
                {
                    Log.Error($"Saving new ban to the list failed. Please contact the author with this error: \n{e}");
                }
            }
        }

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            //g
        }

        public void OnMute(ChangingMuteStatusEventArgs ev)
        {
            //G
        }

        public void OnIcomMute(ChangingIntercomMuteStatusEventArgs ev)
        {
            //ggg
        }
    }
}
