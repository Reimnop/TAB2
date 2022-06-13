namespace TAB2.Utilities;

public class DeferredRunner
{
    private readonly List<Action> functions;

    public DeferredRunner()
    {
        functions = new List<Action>();
    }

    public void QueueFunction(Action action)
    {
        functions.Add(action);
    }

    public void RunAll()
    {
        foreach (Action action in functions)
        {
            action();
        }
    }
}