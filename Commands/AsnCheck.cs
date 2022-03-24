using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace UnbanHelper.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AsnCheck : ICommand
    {
        public string Command { get; } = "asncheck";

        public string[] Aliases { get; } = new[] { "checkasn" };

        public string Description { get; } = "Check Player's ASN.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("unbanhelper.asncheck"))
            {
                response = "<color=red> No permissions!</color>";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <RA ID> </color>";
                return false;
            }

            Player player = Player.Get(arguments.Array[1].ToString());
            if (player == null)
            {
                response = $"<color=yellow>Usage: {Command} <RA ID> </color>";
                return false;
            }

            response = $"<color=green>{player.Nickname}'s ASN is</color> <color=lightblue>{player.ReferenceHub.characterClassManager.Asn}</color>";
            return true;
           
        }
    }
}
