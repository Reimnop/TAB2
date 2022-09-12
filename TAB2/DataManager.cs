using TAB2.Api;
using TAB2.Api.Data;

namespace TAB2;

public class DataManager : IDataManager
{
    private readonly Dictionary<string, IPersistentData> datas = new Dictionary<string, IPersistentData>();

    public void RegisterData(string id, IPersistentData data)
    {
        datas.Add(id, data);
    }

    public IPersistentData GetData(string id)
    {
        return datas[id];
    }

    public void SaveData(string id)
    {
    }
}