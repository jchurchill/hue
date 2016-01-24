using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace HueConsole.CommandParser
{
	class Options
	{
		public const string HelpVerb = "help";
		public const string LightVerb = "light";

		[VerbOption(LightVerb, HelpText = "Gets or changes properties about one or more lights.")]
		public LightOptions LightVerbOptions { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var help = new HelpText
			{
				Heading = new HeadingInfo("HueConsole", "0.1"),
			};
			help.AddOptions(this);
			return help;
		}
	}
}
