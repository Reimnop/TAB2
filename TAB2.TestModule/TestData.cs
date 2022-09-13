using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace TAB2.TestModule;

public class TestData : IPersistentData
{
    private int testInt = 0;
    
    public void WriteData(TagCompound compound)
    {
        compound.Value.Add("testint", testInt);
    }

    public void ReadData(TagCompound compound)
    {
        testInt = compound.GetIntValue("testint");
    }
}