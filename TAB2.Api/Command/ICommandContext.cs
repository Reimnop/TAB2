namespace TAB2.Api.Command;

public interface ICommandContext
{
    Task RespondAsync(string message);
}