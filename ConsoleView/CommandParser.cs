using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ConsoleView
{
	public interface IConsoleCommand
	{
		string ConsoleGroupName { get; }
		string Name { get; }
		string Description { get; }
		Task<string> Execute(params string[] args);
	}
	internal class ConsoleCommandParser
	{
		public const char SEPARATOR = ' ';
		private readonly Dictionary<string, IConsoleCommand> commands;

		public string[] CommandNames => [.. commands.Keys];
		public ConsoleCommandParser()
		{
			commands = [];
			var parent = typeof(IConsoleCommand);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes()
					.Where(t => t.IsClass
						&& !t.IsAbstract
						&& parent.IsAssignableFrom(t)));

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type) as IConsoleCommand;
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
				commands.Add(instance.Name, instance);
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
			}
		}

		public async Task<string> Proccess(string providedString)
		{

			if (string.IsNullOrEmpty(providedString))
				return string.Empty;

			var splitedStrings = providedString
				.Trim()
				.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

			var commandKey = splitedStrings[0];
			var args = splitedStrings.Skip(1).ToArray();

			if (!commands.TryGetValue(commandKey, out IConsoleCommand? value))
				return string.Format($"command {commandKey} does not exist");

			string result;
			try
			{
				result = await value.Execute(args);
			}
			catch (Exception e)
			{
				result = e.StackTrace ?? "An undefined error has occurred";
				result = $"[ERROR] {result}";
			}
			return result;
		}
	}
}
