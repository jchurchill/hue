using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueConsole.CommandExecutors;
using ManyConsole;
using Q42.HueApi;

namespace HueConsole.CommandParser
{
    [RegisterConsoleCommand()]
    class OffConsoleCommand : ConsoleCommand
    {
        private readonly LightCommandExecutor _executor;

        public OffConsoleCommand(LightCommandExecutor executor)
        {
            this._executor = executor;
            this.IsCommand("off", "Turn the light(s) off");
        }

        public override int Run(string[] remainingArguments)
        {
            var command = new LightCommand { On = false };
            this._executor.ExecuteCommand(command);
            return 0;
        }
    }
}
