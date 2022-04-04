using TAB2;

using var main = new BotMain();
main.Run(File.ReadAllText("token.txt")).GetAwaiter().GetResult();