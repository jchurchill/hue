using System.Configuration;
using System.Linq;
using Autofac;
using HueConsole.CommandExecutors;
using HueConsole.CommandParser;
using ManyConsole;
using Q42.HueApi;

namespace HueConsole.App_Start
{
    static class AutofacConfig
	{
		public static IContainer Configure()
		{
			var builder = new ContainerBuilder();

			RegisterHueClient(builder);
            RegisterCommands(builder);

            builder.RegisterType<LightCommandExecutor>().AsSelf();

			return builder.Build();
		}

		private static void RegisterHueClient(ContainerBuilder builder)
		{
			var appBridgeIP = ConfigurationManager.AppSettings["bridgeIP"];
			var appKey = ConfigurationManager.AppSettings["appKey"];
			var client = new LocalHueClient(appBridgeIP);
			client.Initialize(appKey);
			builder.RegisterInstance(client).ExternallyOwned();
		}

        private static void RegisterCommands(ContainerBuilder builder)
        {
            var consoleCommandTypes = typeof(RegisterConsoleCommandAttribute).Assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(RegisterConsoleCommandAttribute), true).Any());
            foreach (var consoleCommandType in consoleCommandTypes)
            {
                builder.RegisterType(consoleCommandType).As<ConsoleCommand>();
            }
        }
	}
}
