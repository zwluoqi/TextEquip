using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;


public class CommonUtil
{
	public static string[] GetSplitString (string source, char split, System.StringSplitOptions s = System.StringSplitOptions.RemoveEmptyEntries)
	{
		char[] sp = { split };
		string[] data = source.Split (sp, s);
		return data;
	}
	public static List<string> GetSplitStringList (string source, char split, System.StringSplitOptions s = System.StringSplitOptions.RemoveEmptyEntries)
	{
		char[] sp = { split };
		string[] data = source.Split (sp, s);
		List<string> listDatas = new List<string> ();
		listDatas.AddRange (data);
		return listDatas;
	}

	public static List<int> GetIntSplitString (string source, char split, System.StringSplitOptions s = System.StringSplitOptions.RemoveEmptyEntries)
	{
		char[] sp = { split };
		string[] data = source.Split (sp, s);
		List<int> ints = new List<int> ();
		foreach (var d in data) {
			var i = int.Parse (d);
			ints.Add (i);
		}
		return ints;
	}

	public static string[] GetSplitString (string source, char[] split, System.StringSplitOptions s = System.StringSplitOptions.RemoveEmptyEntries)
	{
		string[] data = source.Split (split, s);
		return data;
	}


	public static string CombineList (List<int> datas, char split)
	{
		string heroCopyIdsStr = "";
		for (int i = 0; i < datas.Count; i++) {
			heroCopyIdsStr += datas [i];
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr; 
	}

	public static string CombineList (List<string> datas, char split)
	{
		string heroCopyIdsStr = "";
		for (int i = 0; i < datas.Count; i++) {
			heroCopyIdsStr += datas [i];
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr;
	}

	public static string CombineDict (Dictionary<string, int> data, char split)
	{
		string heroCopyIdsStr = "";
		foreach(var item in data) {
			heroCopyIdsStr += item.Key;
			heroCopyIdsStr += ":";
			heroCopyIdsStr += item.Value;
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr;
	}

	public static string CombineDict (Dictionary<string, long> data, char split)
	{
		string heroCopyIdsStr = "";
		foreach(var item in data) {
			heroCopyIdsStr += item.Key;
			heroCopyIdsStr += ":";
			heroCopyIdsStr += item.Value;
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr;
	}

	public static string CombineDict (Dictionary<string, double> data, char split)
	{
		string heroCopyIdsStr = "";
		foreach(var item in data) {
			heroCopyIdsStr += item.Key;
			heroCopyIdsStr += ":";
			heroCopyIdsStr += item.Value;
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr;
	}

	public static string CombineArray (string[] hero_array, char split)
	{
		string heroCopyIdsStr = "";
		for(int i=0;i<hero_array.Length;i++) {
			heroCopyIdsStr += i;
			heroCopyIdsStr += ":";
			heroCopyIdsStr += hero_array [i];
			heroCopyIdsStr += split;
		}
		return heroCopyIdsStr;
	}

	// public static string[] SplitArray(string arrayHeros){
	//
	// 	var _tianji_hero_arrays = new string[Grow.GrowFullPlayer.arrayMaxCount];
	// 	for (int i = 0; i < Grow.GrowFullPlayer.arrayMaxCount; i++) {
	// 		_tianji_hero_arrays [i] = "";
	// 	}
	// 	var hadRechargeIds = CommonUtil.GetSplitString (arrayHeros, '#');
	// 	foreach (var id in hadRechargeIds) {
	// 		var infos = CommonUtil.GetSplitString (id, ':',StringSplitOptions.None);
	// 		_tianji_hero_arrays [int.Parse(infos [0])] = infos [1];
	// 	}
	// 	return _tianji_hero_arrays;
	// }

	public static double GetRefVal(object o ,string val){
		var type = o.GetType ();
		var p = type.GetProperty (val);
		if (p != null) {
			return Convert.ToDouble (p.GetValue (o, null));
		}
		var f = type.GetField (val);
		if (f != null) {
			return Convert.ToDouble (f.GetValue (o));
		}
		return 0;
	}

	static StringBuilder sb = new StringBuilder();
	public static string InsertSpace (string shuxingName,string bn,string bn_add,int num,bool insertSpace)
	{
		int sadchar = 1;
		sb.Remove (0, sb.Length);
		sb.Append (shuxingName);
		sb.Append (':');
		sb.Append (bn);
		if (!string.IsNullOrEmpty (bn_add)) {
			sb.Append ("+");
			sb.Append (bn_add);
			sadchar++;
		}
		if (insertSpace) {
			var spaceNum = Mathf.Max (0, num - shuxingName.Length  - bn.Length - bn_add.Length - sadchar);
			if (spaceNum > 0) {
				sb.Append (' ', spaceNum*2);
				return sb.ToString ();
			} else {
				return sb.ToString ();
			}
		} else {
			return sb.ToString ();
		}

	}

	public static string HPConvert (long totalHP)
	{
		if(totalHP > 100000000){
			return (totalHP / 100000000) + "亿"+HPConvert(totalHP % 100000000);
		}
		else if (totalHP > 100000) {
			return (totalHP / 10000) + "万";
		}else{
			return totalHP.ToString ();
		}
	}

	// 解析操作字符串
	public static Dictionary<string,string> ParseOptString (string source)
	{
//		m_options.Clear ();
		Dictionary<string,string> options = new Dictionary<string, string>();
		string[] strArr = Regex.Split (source, "&", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
		foreach (string s in strArr) {
			if (s.Contains ("=")) {
				string[] strV = s.Split (new char[1] { '=' });

				if (options.ContainsKey (strV [0])) {
					options [strV [0]] = strV [1];
				} else {
					options.Add (strV [0], strV [1]);
				}
			}
		}

		return options;
	}

	public static bool IsEmptyStrings (string[] hero_arrays)
	{
		for (int i = 0; i < hero_arrays.Length; i++) {
			if (!string.IsNullOrEmpty(hero_arrays [i])) {
				return false;
			}
		}
		return true;
	}
}
