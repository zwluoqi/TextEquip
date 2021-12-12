//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xor;

public partial class DictEquipEquip
{

    // data model
    // [System.Serializable]
    public class Model
    {
     /// <summary>
        /// 装备ID
        /// </summary>
        public string id;
     /// <summary>
        /// 物品名称
        /// </summary>
        public string name;
     /// <summary>
        /// 装备类型
        /// </summary>
        public XorInt equip_type_int_xor;
     /// <summary>
        /// 装备类型
        /// </summary>
        public int equip_type_int{
            get{
                return equip_type_int_xor.val;
            }
        }
     /// <summary>
        /// 装备类型
        /// </summary>
        public string equip_type;
     /// <summary>
        /// 物品类型
        /// </summary>
        public string equip_sub_type;
     /// <summary>
        /// 图标
        /// </summary>
        public string equip_icon;
     /// <summary>
        /// 极品武器
        /// </summary>
        public XorInt spe_xor;
     /// <summary>
        /// 极品武器
        /// </summary>
        public int spe{
            get{
                return spe_xor.val;
            }
        }
     /// <summary>
        /// 描述
        /// </summary>
        public string desc;
     /// <summary>
        /// 属性描述
        /// </summary>
        public List<string> baseAbility;

    public virtual Model Clone()
    {
        Model mm = this.MemberwiseClone() as Model;
        return mm;
    }
}

    //
    private Dictionary<string, Model> m_dict;

    // Get The Dictionary
    public Dictionary<string, Model> Dict
    {
        get
        {
            return m_dict;
        }
    }

    private List<Model> m_list;
	public List<Model> getList()
	{
		return m_list;
	}

    // Get The Model By Key
    public Model GetModel(string id)
    {
        if (m_dict == null)
        {
            Debug.LogError("DictEquipEquip m_dict Is Null");
            return null;
        }
        else
        {
            if (m_dict.ContainsKey(id))
            {
                return m_dict[id];
            }
            else
            {
                Debug.LogError ("error id:"+id);
                return null;
            }
        }
    }

    private DictString_LoadFinishedCallBack m_loadFinishedCallBack;
    // load the json file
    public void Load(string path,DictString_LoadFinishedCallBack callBack)
    {
        m_loadFinishedCallBack = callBack;

        //
        string filePath = path;
        if(!filePath.EndsWith("/"))
        {
            filePath += "/";
        }

        filePath += GetFileName();
        DictFileReader fileReader = new DictFileReader(filePath, DoParse);
    }

    // get file name
    public string GetFileName()
    {
        return "dict_equip_equip.txt";
    }

    // parse the json data
    private void DoParse(DictFileReader fileReader)
    {
        m_dict = new Dictionary<string,Model>();
        m_list = new List<Model>();
        //
        do
        {
            string[] str = fileReader.ReadRow();
            if (str == null || str.Length == 0)
            {
                break;
            }

            //
            Model model = new Model();
            model.id = DictTypeConvert.ParseString(str[fileReader.typeName2Index["id"]]);
            model.name = DictTypeConvert.ParseString(str[fileReader.typeName2Index["name"]]);
            model.equip_type_int_xor = new XorInt( DictTypeConvert.ParseInt(str[fileReader.typeName2Index["equip_type_int"]]),fileReader.randomUtil);
            model.equip_type = DictTypeConvert.ParseString(str[fileReader.typeName2Index["equip_type"]]);
            model.equip_sub_type = DictTypeConvert.ParseString(str[fileReader.typeName2Index["equip_sub_type"]]);
            model.equip_icon = DictTypeConvert.ParseString(str[fileReader.typeName2Index["equip_icon"]]);
            model.spe_xor = new XorInt( DictTypeConvert.ParseInt(str[fileReader.typeName2Index["spe"]]),fileReader.randomUtil);
            model.desc = DictTypeConvert.ParseString(str[fileReader.typeName2Index["desc"]]);
            model.baseAbility = DictTypeConvert.ParseArrayString(str[fileReader.typeName2Index["baseAbility"]]);

            if (m_dict.ContainsKey(model.id) == false)
            {
                m_dict.Add(model.id, model);
                m_list.Add(model);
            }
            else
            {
                Debug.LogError("DictEquipEquip Parse:Same Key = " + model.id);
            }

        } while (true);

        if(m_loadFinishedCallBack != null)
        {
            m_loadFinishedCallBack(GetFileName());
        }
    }

}