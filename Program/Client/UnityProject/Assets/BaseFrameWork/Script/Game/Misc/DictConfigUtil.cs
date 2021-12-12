public class DictConfigUtil
{
    public static string GetString(string key,string def="")
    {
        var model = DictDataManager.Instance.dictSystemConfig.GetModel(key);
        if (model == null)
        {
            return def;
        }
        else
        {
            return model.val;
        }
    }
}