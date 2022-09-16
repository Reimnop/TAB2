namespace TAB2.Api.Command;

public interface ICommandContext
{
    bool GetArgument<T>(string name, out T value);
    Task RespondAsync(string message);
}