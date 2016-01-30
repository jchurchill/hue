using System;
using Autofac;
using HueConsole.App_Start;

namespace HueConsole
{
    class Program
	{
		static IContainer Container;

		static void Main(string[] args)
		{
			Container = AutofacConfig.Configure();

            Console.WriteLine("Welcome to Hue!");

            using (var scope = Container.BeginLifetimeScope())
            {
                var consoleModeRunner = new ConsoleModeRunner(scope, Console.Out, Console.In);
                consoleModeRunner.Run();
            }
        }
	}
}
