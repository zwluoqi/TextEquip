public class LanguageUtil
{
    public static string GetLanguage(string id)
    {
        var model = DictDataManager.Instance.dictSystemLanguage.GetModel(id);
        if (model != null)
        {
            return model.chinese;
        }
        else
        {
            return "";
        }
    }
}