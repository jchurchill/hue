using System;
using System.Collections.Generic;

namespace HueConsole.CommandExecutors
{
	abstract class CommandExecutorBase<TOptions> : ICommandExecutor
		where TOptions : class
	{
		public abstract string CommandVerb { get; }

		ICommandResult ICommandExecutor.ExecuteCommand(object verbOptions)
		{
			var options = verbOptions as TOptions;
			if (options == null)
			{
				var exceptionMessage = string.Format("Options passed to {0} were not of type {1}", this.GetType().Name, typeof(TOptions).Name);
				throw new InvalidCastException(exceptionMessage);
			}
			return this.ExecuteCommand(options);
		}

		protected abstract CommandResult ExecuteCommand(TOptions options);
	}

	class CommandResult : ICommandResult
	{
		public IEnumerable<string> Messages { get; set; }

		public static CommandResult SingleMessageResult(string message)
		{
			return new CommandResult { Messages = new[] { message } };
		}
	}
}
