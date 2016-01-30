using System.Linq;
using System.Text.RegularExpressions;
using HueConsole.CommandParser;
using Q42.HueApi;

namespace HueConsole.CommandExecutors
{
	[CommandExecutor(CommandOptions.LightVerb)]
	class LightCommandExecutor : CommandExecutorBase<LightOptions>
	{
		public override string CommandVerb { get { return CommandOptions.LightVerb; } }

		private readonly LocalHueClient _hueClient;

		public LightCommandExecutor(LocalHueClient hueClient)
		{
			this._hueClient = hueClient;
		}

		protected override CommandResult ExecuteCommand(LightOptions options)
		{
			CommandResult invalidCommandResult;
			if (!this.IsValid(options, out invalidCommandResult))
			{
				return invalidCommandResult;
			}

			var affectedLights = options.LightNumbers != null
				? options.LightNumbers.Select(ln => ln.ToString())
				: null;

			var command = new LightCommand
			{
				On = !options.TurnOff
			};
			command.SetColor(options.Color);

			this._hueClient.SendCommandAsync(command, affectedLights).Wait();

			return CommandResult.SingleMessageResult("Light command successfully executed.");
		}

		private bool IsValid(LightOptions options, out CommandResult invalidResult)
		{
			invalidResult = null;
			var isValid = true;
			if (!Regex.IsMatch(options.Color, "[0-9A-F]{6}", RegexOptions.IgnoreCase))
			{
				var message = string.Format("Color {0} is an invalid hex value.", options.Color);
				invalidResult = CommandResult.SingleMessageResult(message);
				isValid = false;
			}

			return isValid;
		}
	}
}
