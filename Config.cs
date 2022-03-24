using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnbanHelper
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("List of ASN Addresses that are whitelisted from being bans. Check the readme for more info")]
		public List<string> AsnAntiBan { get; set; } = new List<string>() { "50889" };

		public bool Debug { get; set; } = false;
	}
}
