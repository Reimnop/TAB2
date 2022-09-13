namespace TAB2.Api.Command;

public class CommandBuilder
{
    public string Name { get; }
    public string Description { get; set; }
    public Task? RunTask { get; set; }

    public CommandBuilder(string name)
    {
        Name = name;
    }

    public CommandBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public CommandBuilder Executes(Task task)
    {
        RunTask = task;
        return this;
    }
}