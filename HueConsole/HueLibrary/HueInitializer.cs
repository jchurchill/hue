using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace HueLibrary
{
	public class HueInitializer
    {
		// Currently unused because it doesn't work! 
		// In LocateBridgeIP, https://www.meethue.com/api/nupnp returns "[]" but should be detecting the bridge on the network
		public async Task<string> Initialize()
		{
			var bridgeIP = await this.LocateBridgeIP();
			return await this.Initialize(bridgeIP);
		}

		public async Task<string> Initialize(string bridgeIP)
		{
			ILocalHueClient client = new LocalHueClient(bridgeIP);
			return await client.RegisterAsync("ConsoleApp", "Desktop");
		}

		private async Task<string> LocateBridgeIP()
		{
			IBridgeLocator bridgeLocator = new HttpBridgeLocator();
			var bridgeIPs = await bridgeLocator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
			return bridgeIPs.FirstOrDefault();
		}
    }
}
