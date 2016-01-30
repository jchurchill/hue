using CommandLine;

namespace HueConsole.CommandParser
{
	class LightOptions
	{
		[OptionArray('n', "num", HelpText = "The number(s) of the light(s) to modify (1-indexed). If not specified, affects all lights.")]
		public int[] LightNumbers { get; set; }

		[Option("off", DefaultValue = false, HelpText = "Turn the light(s) off", MutuallyExclusiveSet = "off")]
		public bool TurnOff { get; set; }

		[Option('c', "color", DefaultValue = "FFFFFF", HelpText = "Hex color to apply to the light(s).", MutuallyExclusiveSet = "on")]
		public string Color { get; set; }
	}
}
