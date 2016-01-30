using System;
using System.Linq;
using Autofac;
using CommandLine.Text;
using HueConsole.App_Start;
using HueConsole.CommandExecutors;
using HueConsole.CommandParser;

namespace HueConsole
{
	class Program
	{
		static IContainer Container;

		static void Main(string[] args)
		{
			Container = AutofacConfig.Configure();
			WaitForCommands(Container);
		}

		private static void WaitForCommands(IContainer container)
		{
			Console.WriteLine("Welcome to Hue!");
			Console.WriteLine(HelpText.AutoBuild(new CommandOptions(), null));

			while (true)
			{
				var inputArgs = Console.ReadLine().Split(' ');
				var options = new CommandOptions();

				ParsedCommand parsedCommand;
				if (TryParseCommand(inputArgs, options, out parsedCommand))
				{
					using (var scope = container.BeginLifetimeScope())
					{
						// Execute the command if we have an executor for it
						var commandExecutor = scope.ResolveKeyed<ICommandExecutor>(parsedCommand.Verb);
						var commandResult = commandExecutor.ExecuteCommand(parsedCommand.VerbOptions);
						foreach (var msg in commandResult.Messages.Where(m => m != null))
						{
							Console.WriteLine(msg);
						}
					}
				}
				Console.WriteLine();
			}
		}

		private static bool TryParseCommand(string[] inputArgs, CommandOptions commandOptions, out ParsedCommand parsedCommand)
		{
			ParsedCommand command = null;
			var success = CommandLine.Parser.Default
				.ParseArguments(inputArgs, commandOptions, (verb, subOptions) =>
				{
					command = new ParsedCommand { Verb = verb, VerbOptions = subOptions };
				});
			parsedCommand = command;
			return success;
		}

		private class ParsedCommand
		{
			public string Verb { get; set; }
			public object VerbOptions { get; set; }
		}
	}
}
