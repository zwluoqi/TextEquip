using System;
using System.Collections.Generic;

public partial class DictEquipQulity
{

    public Dictionary<long ,Model> qulityAndTypes = new Dictionary<long,Model>();

    public DictEquipQulity.Model GetModelByQulityAndType(int qulity,int type)
    {
        if (qulityAndTypes.Count == 0)
        {
            InitCache();
        }
        int key = (qulity << 16) + type;
        return qulityAndTypes[key];
    }

    private void InitCache()
    {
        foreach (var item in getList())
        {
            int key = (item.qulity << 16) + item.equip_type;
            qulityAndTypes[key] = item;
        }
    }
}