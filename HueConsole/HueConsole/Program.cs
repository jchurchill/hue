using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueConsole.CommandParser;
using Q42.HueApi;

namespace HueConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var appBridgeIP = ConfigurationManager.AppSettings["bridgeIP"];
			var appKey = ConfigurationManager.AppSettings["appKey"];
			Console.WriteLine("Welcome to hue! Enter a command or type \"help\" to see the list of commands.");
			var client = new LocalHueClient(appBridgeIP);
			client.Initialize(appKey);

			while (true)
			{
				var inputArgs = Console.ReadLine().Split(' ');

				var options = new Options();
				string invokedVerb = null;
				object invokedVerbOptions = null;
				var parseSuccess = CommandLine.Parser.Default
					.ParseArguments(inputArgs, options, (verb, subOptions) =>
						{
							invokedVerb = verb;
							invokedVerbOptions = subOptions;
						});

				if (parseSuccess)
				{
					if (invokedVerb == Options.LightVerb)
					{
						var lightSubOptions = invokedVerbOptions as LightOptions;

						var affectedLights = lightSubOptions.LightNumbers != null
							? lightSubOptions.LightNumbers.Select(ln => ln.ToString())
							: null;

						var command = new LightCommand
						{
							On = !lightSubOptions.TurnOff
						};
						// TODO: validate that string is hex color
						command.SetColor(lightSubOptions.Color);
						
						client.SendCommandAsync(command, affectedLights).Wait();
						Console.WriteLine("Light command successfully executed.");
					}
				}
				else
				{
					// Hacky... "help" should be parsed succesfully
					if (invokedVerb != Options.HelpVerb)
					{
						Console.WriteLine("Unrecognized command.");
					}
					Console.WriteLine(options.GetUsage());
				}
			}
		}
	}
}
