using System.Collections.Generic;
using System.Linq;
using Q42.HueApi;

namespace HueConsole.CommandExecutors
{
    class LightCommandExecutor
	{
		private readonly LocalHueClient _hueClient;

		public LightCommandExecutor(LocalHueClient hueClient)
		{
			this._hueClient = hueClient;
		}

		public void ExecuteCommand(LightCommand lightCommand, IList<int> affectedLights = null)
		{
            var lights = affectedLights != null ? affectedLights.Select(l => l.ToString()) : null;
			this._hueClient.SendCommandAsync(lightCommand, lights).Wait();
        }
    }
}
