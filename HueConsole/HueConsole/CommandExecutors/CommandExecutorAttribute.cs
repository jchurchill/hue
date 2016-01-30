using System;

namespace HueConsole.CommandExecutors
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	class CommandExecutorAttribute : Attribute
	{
		private readonly string _commandVerb;
		public string CommandVerb { get { return this._commandVerb; } }

		public CommandExecutorAttribute(string commandVerb)
		{
			this._commandVerb = commandVerb;
		}
	}
}
