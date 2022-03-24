using CommandSystem;
using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnbanHelper.Objects;

namespace UnbanHelper.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class UnbanID : ICommand
    {
        public string Command { get; } = "unbanid";

        public string[] Aliases { get; } = new[] { "uid", "gunban", "unbanall"};

        public string Description { get; } = "Remove player's IP ban by ID.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <Steam/Discord ID> </color>";
                return false;
            }

            var id = arguments.Array[1].ToString();

            if (!(id.Contains("@steam") || id.Contains("@discord")))
            {
                response = $"<color=yellow>Usage: {Command} <Steam/Discord ID> </color>";
                return false;
            }

            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string pluginPath = Path.Combine(appData, "Plugins");
                string path = Path.Combine(Paths.Plugins, "UnbanHelper");
                string bansPath = Path.Combine(path, "BanList.json");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(bansPath))
                {
                    File.Create(bansPath).Close();
                    response = "<color=yellow>Bans list is empty. Command works only for bans issued after plugin installation.</color>";
                    return false;
                }
                    

                // Read existing json data
                var jsonData = File.ReadAllText(bansPath);
                // De-serialize to object or create new list
                var bansList = JsonConvert.DeserializeObject<List<PlayerBan>>(jsonData)
                                      ?? new List<PlayerBan>();

                if (jsonData == null)
                {
                    response = "<color=yellow>Bans list is empty. Command works only for bans issued after plugin installation.</color>";
                    return false;
                }
                if (!jsonData.Contains(arguments.Array[1].ToString()))
                {
                    response = $"<color=red>ID not found in bans list.</color>";
                    return false;
                }

                var banInfo = bansList.Single(r => r.ID == id);
                bansList.Remove(banInfo);
                BanHandler.RemoveBan(banInfo.ID, BanHandler.BanType.UserId);
                BanHandler.RemoveBan(banInfo.IP, BanHandler.BanType.IP);
                Log.Debug($"Removed ban from {banInfo.ID} and {banInfo.IP}. Unbanned by {sender.LogName}", Plugin.Singleton.Config.Debug);
                //bansList.RemoveAll(r => r.ID == id);

                // Update json data string
                jsonData = JsonConvert.SerializeObject(bansList, Formatting.Indented);
                File.WriteAllText(bansPath, jsonData);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            response = "Done! Removed the ban from desired ID.";
            return true;
        }
    }
}
