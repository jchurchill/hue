using System.Linq;
using System.Text.RegularExpressions;
using HueConsole.CommandExecutors;
using ManyConsole;
using NDesk.Options;
using Q42.HueApi;

namespace HueConsole.CommandParser
{
    [RegisterConsoleCommand]
    class LightConsoleCommand : ConsoleCommand
    {
        public const string LightVerb = "light";

        private readonly LightCommandExecutor _executor;

        private static readonly int[] ValidLightNumbers = new[] { 1, 2, 3 };

        public LightConsoleCommand(LightCommandExecutor executor)
        {
            this._executor = executor;
            this.IsCommand(LightVerb, "Change properties for one or more lights.")
                .AllowsAnyAdditionalArguments("<light #s>")
                .HasOption<string>("c|color=", "Hex color to apply to the light(s).", c => { this.Color = c; });
        }

        private string Color { get; set; }

        public override int Run(string[] remainingArguments)
        {
            var parsedRemainingArgs = remainingArguments.Select(a =>
            {
                int i;
                var parsed = int.TryParse(a, out i);
                return new { Success = parsed, Value = i };
            });

            if (parsedRemainingArgs.Any(a => !a.Success) || parsedRemainingArgs.Select(a => a.Value).Except(ValidLightNumbers).Any())
            {
                var message = string.Format("Invalid light number specified. Valid lights are: [{0}]", string.Join(",", ValidLightNumbers));
                throw new ConsoleHelpAsException(message);
            }
            var lightNumbers = parsedRemainingArgs.Select(a => a.Value).ToArray();

            var affectedLights = lightNumbers.Any()
                ? lightNumbers.Select(ln => ln.ToString())
                : null;

            var command = new LightCommand { On = true };
            command.SetColor(this.Color ?? "FFFFFF");

            this._executor.ExecuteCommand(command);

            return 0;
        }

        public override int? OverrideAfterHandlingArgumentsBeforeRun(string[] remainingArguments)
        {
            if (!this.IsColorValid())
            {
                var message = string.Format("Color {0} is an invalid hex value.", this.Color);
                throw new ConsoleHelpAsException(message);
            }
            return base.OverrideAfterHandlingArgumentsBeforeRun(remainingArguments);
        }

        private bool IsColorValid()
        {
            return this.Color == null || Regex.IsMatch(this.Color, "[0-9A-F]{6}", RegexOptions.IgnoreCase);
        }
    }
}
