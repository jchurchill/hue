using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueConsole.CommandExecutors
{
	interface ICommandExecutor
	{
		string CommandVerb { get; }
		ICommandResult ExecuteCommand(object verbOptions);
	}

	interface ICommandResult
	{
		IEnumerable<string> Messages { get; }
	}
}
