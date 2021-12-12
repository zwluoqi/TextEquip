using System;
using System.Collections.Generic;
using System.Linq;
using TextEquip.System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class DictEquipEquip
{
	public Dictionary<int,List<Model>> typeDictCaches;
	public Dictionary<int,List<Model>> typeSpeDictCaches;
	// public Dictionary<int,List<Model>> qulityTypeDictCaches;
	// public Dictionary<int,List<Model>> qulityDictCaches;
	public Dictionary<int,List<Model>> fromDictCaches;
	public Dictionary<string,List<Model>> suit_equip_caches;

	public List<string> topEquipTypes = new List<string> ();
	public List<string> topMagicTypes = new List<string> ();


	public List<Model> defaultVal = new List<Model>();
	// public List<Model> GetModelsByQulity(int qulity){
	// 	if (qulityDictCaches == null || fromDictCaches == null) {
	// 		InitDictIndexCaches ();
	// 	}
	// 	if (qulityDictCaches.ContainsKey (qulity)) {
	// 		return qulityDictCaches [qulity];
	// 	} else {
	// 		return defaultVal;
	// 	}
	// }
	//
	// public List<Model> GetModelsByFrom (DictEquipEquipFromEnum equip_val)
	// {
	// 	if (qulityDictCaches == null || fromDictCaches == null) {
	// 		InitDictIndexCaches ();
	// 	}
	// 	if (fromDictCaches.ContainsKey ((int)equip_val)) {
	// 		return fromDictCaches [(int)equip_val];
	// 	} else {
	// 		return defaultVal;
	// 	}
	// }
	//
	// public List<Model> GetSuitEquips(string suit_equip_type){
	// 	if (qulityDictCaches == null || fromDictCaches == null) {
	// 		InitDictIndexCaches ();
	// 	}
	// 	if (suit_equip_caches.ContainsKey (suit_equip_type)) {
	// 		return suit_equip_caches [suit_equip_type];
	// 	} else {
	// 		return defaultVal;
	// 	}
	// }

	public void InitDictIndexCaches(){

		// qulityDictCaches = new Dictionary<int, List<Model>> ();
		fromDictCaches = new Dictionary<int, List<Model>> ();
		suit_equip_caches = new Dictionary<string, List<Model>> ();
		typeDictCaches = new Dictionary<int, List<Model>>();
		typeSpeDictCaches = new Dictionary<int, List<Model>>();
		// qulityTypeDictCaches = new Dictionary<int, List<Model>>();
		foreach (var d in Dict) {
			// if (!qulityDictCaches.ContainsKey (d.Value.qulity)) {
			// 	qulityDictCaches.Add (d.Value.qulity, new List<Model> ());
			// }
			// qulityDictCaches [d.Value.qulity].Add (d.Value);

			if (d.Value.spe > 0)
			{
				if (!typeSpeDictCaches.ContainsKey (d.Value.equip_type_int)) {
					typeSpeDictCaches.Add (d.Value.equip_type_int, new List<Model> ());
				}
				typeSpeDictCaches [d.Value.equip_type_int].Add (d.Value);
			}
			else
			{
				if (!typeDictCaches.ContainsKey(d.Value.equip_type_int))
				{
					typeDictCaches.Add(d.Value.equip_type_int, new List<Model>());
				}

				typeDictCaches[d.Value.equip_type_int].Add(d.Value);
			}

			// var key = (d.Value.qulity << 16) + (d.Value.equip_type_int);
			// if (!qulityTypeDictCaches.ContainsKey (key)) {
			// 	qulityTypeDictCaches.Add (key, new List<Model> ());
			// }
			// qulityTypeDictCaches [key].Add (d.Value);

			
			
			// if (!fromDictCaches.ContainsKey (d.Value.from)) {
			// 	fromDictCaches.Add (d.Value.from, new List<Model> ());
			// }
			// fromDictCaches [d.Value.from].Add (d.Value);
			//
			// if (!string.IsNullOrEmpty (d.Value.suit_equip_type)) {
			// 	if (!suit_equip_caches.ContainsKey (d.Value.suit_equip_type)) {
			// 		suit_equip_caches.Add (d.Value.suit_equip_type, new List<Model> ());
			// 	}
			// 	suit_equip_caches [d.Value.suit_equip_type].Add (d.Value);
			// }
		}

		for (int i = 0; i < DictEquipEquipTypeEnumString.vlas.Length; i++) {
			var item = DictEquipEquipTypeEnumString.vlas [i];
			if (item == DictEquipEquipTypeEnumString.magic_key
				|| item == DictEquipEquipTypeEnumString.magic_key_left
				|| item == DictEquipEquipTypeEnumString.magic_key_right) {
				topMagicTypes.Add (item);
			} else {
				topEquipTypes.Add (item);
			}
		}
	}

	public List<string> GetNormalEquipsType(){
		if ( fromDictCaches == null) {
			InitDictIndexCaches ();
		}
		return topEquipTypes;
	}

	public List<string> GetMagicEquipsType(){
		if ( fromDictCaches == null) {
			InitDictIndexCaches ();
		}
		return topMagicTypes;
	}

	public List<Model> GetModelsByType(DictEquipEquipTypeEnum equipTypeEnum,int qulity)
	{
		if (fromDictCaches == null) {
			InitDictIndexCaches ();
		}

		if (qulity > 14)
		{
			return typeSpeDictCaches[(int) equipTypeEnum];
		}
		else
		{
			return typeDictCaches[(int) equipTypeEnum];
		}

	}
	
	public Dictionary<string,List<AttributeConfig>> attributeConfigs = new Dictionary<string, List<AttributeConfig>>();

	public List<AttributeConfig> GetAttributeConfig(DictEquipEquip.Model model)
	{
		if (attributeConfigs.ContainsKey(model.id))
		{
			return attributeConfigs[model.id];
		}
		else
		{
			var configList = AttributeItemUtil.ConvertString2Struc(model.baseAbility);
			attributeConfigs[model.id] = configList;
			return configList;
		}
	}

	// public List<Model> GetModelsByTypeAndQulity(DictEquipEquipTypeEnum equipTypeEnum, int qulity)
	// {
	// 	if (qulityDictCaches == null || fromDictCaches == null) {
	// 		InitDictIndexCaches ();
	// 	}
	// 	var key = (qulity << 16) + (int)(equipTypeEnum);
	// 	return qulityTypeDictCaches[key];
	// }
}