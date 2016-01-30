using System;
using System.Configuration;
using System.Linq;
using Autofac;
using HueConsole.CommandExecutors;
using Q42.HueApi;

namespace HueConsole.App_Start
{
	static class AutofacConfig
	{
		public static IContainer Configure()
		{
			var builder = new ContainerBuilder();

			RegisterHueClient(builder);
			RegisterCommandExecutors(builder);

			return builder.Build();
		}

		private static void RegisterHueClient(ContainerBuilder builder)
		{
			var appBridgeIP = ConfigurationManager.AppSettings["bridgeIP"];
			var appKey = ConfigurationManager.AppSettings["appKey"];
			var client = new LocalHueClient(appBridgeIP);
			client.Initialize(appKey);
			builder.Register(cc => client).SingleInstance().AsSelf();
		}

		private static void RegisterCommandExecutors(ContainerBuilder builder)
		{
			var registrations = typeof(CommandExecutorAttribute).Assembly.GetTypes()
				.SelectMany(t =>
					t.GetCustomAttributes(typeof(CommandExecutorAttribute), true)
						.Cast<CommandExecutorAttribute>()
						.Select(att => new { Type = t, Key = att.CommandVerb }));
			foreach (var registration in registrations)
			{
				builder.RegisterType(registration.Type).Keyed<ICommandExecutor>(registration.Key);
			}
		}
	}
}
