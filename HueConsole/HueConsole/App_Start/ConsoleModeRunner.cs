using System.Collections.Generic;
using System.IO;
using Autofac;
using ManyConsole;
using ManyConsole.Internal;

namespace HueConsole.App_Start
{
    public class ConsoleModeRunner
    {
        private readonly ILifetimeScope _scope;
        private readonly TextReader _inputStream;
        private readonly TextWriter _outputStream;

        private static readonly string ExitCommand = "x";
        private static readonly string HelpCommand = "?";

        public ConsoleModeRunner(ILifetimeScope scope, TextWriter outputStream, TextReader inputStream)
        {
            this._scope = scope;
            this._outputStream = outputStream;
            this._inputStream = inputStream;
        }

        public void Run()
        {
            var prompt = string.Format("Enter a command or '{0}' to exit or '{1}' for help", ExitCommand, HelpCommand);
            this._outputStream.WriteLine(prompt);

            var input = this._inputStream.ReadLine();
            while (!input.Trim().Equals(ExitCommand))
            {
                this.ProcessCommand(input);
                input = this._inputStream.ReadLine();
            }
        }

        private void ProcessCommand(string input)
        {
            using (var scope = this._scope.BeginLifetimeScope())
            {
                var commands = scope.Resolve<IEnumerable<ConsoleCommand>>();
                if (input.Trim() == HelpCommand)
                {
                    ConsoleHelp.ShowSummaryOfCommands(commands, this._outputStream);
                }
                else
                {
                    var args = CommandLineParser.Parse(input);
                    var result = ConsoleCommandDispatcher.DispatchCommand(commands, args, this._outputStream, true);
                }

                this._outputStream.WriteLine();
            }
        }
    }
}
