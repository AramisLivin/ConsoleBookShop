namespace BookStoreConsole.Commands;

public static class CommandParser
{
    public static (string command, Dictionary<string, string> flags) Parse(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException("No command provided. Available commands: get, buy, restock");
        }

        var command = args[0].ToLower();
        var flags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 1; i < args.Length; i++)
        {
            if (!args[i].StartsWith("--")) continue;
            var parts = args[i][2..].Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                flags[parts[0].Trim()] = parts[1].Trim().Trim('"');
            }
            else
            {
                flags[parts[0].Trim()] = string.Empty;
            }
        }

        return (command, flags);
    }
}