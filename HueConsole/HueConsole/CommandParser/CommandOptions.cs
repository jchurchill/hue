using CommandLine;
using CommandLine.Text;

namespace HueConsole.CommandParser
{
	class CommandOptions
	{
		public const string LightVerb = "light";

		[VerbOption(LightVerb, HelpText = "Gets or changes properties about one or more lights.")]
		public LightOptions LightVerbOptions { get; set; }

		[HelpVerbOption]
		public string GetUsage(string verb)
		{
			return HelpText.AutoBuild(this, verb);
		}
	}
}
