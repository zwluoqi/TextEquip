using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DictTypeConvert
{
    //
    public static int ParseInt(string str)
    {
		if (str.IndexOf ('.') != -1) {
			str = str.Substring (0, str.IndexOf ('.'));
		}
        int num = -2;
        int.TryParse(str, out num);
        if (num == -2)
        {
            Debug.LogError("错误表信息：" + str);
            return -2;
        }
		return Convert.ToInt32(str);
    }

    //
    public static float ParseFloat(string str)
    {
		return Convert.ToSingle(str);
    }

    //
    public static double ParseDouble(string str)
    {
        return Convert.ToDouble(str);
    }

    //
    public static string ParseString(string str)
    {
		if (string.IsNullOrEmpty (str)) {
			return "";
		}
        return str.Replace("\\n", "\n").Replace("\\r", "\r");
    }

    public static string[] SplitArray(string str)
    {
		return str.Split(new string[] { "&&" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    //
    public static List<int> ParseArrayInt(string str)
    {
        List<int> list = new List<int>();

        if (str.Length == 0)
        {
            return list;
        }

        string[] strArray = SplitArray(str);

        for (int i = 0; i < strArray.Length; i++)
        {
			int ret = ParseInt(strArray[i]);
            list.Add(ret);
        }

        return list;
    }

    //
    public static List<float> ParseArrayFloat(string str)
    {
        List<float> list = new List<float>();

        if (str.Length == 0)
        {
            return list;
        }

        string[] strArray = SplitArray(str);

        for (int i = 0; i < strArray.Length; i++)
        {
			float ret = Convert.ToSingle(strArray[i]);
            list.Add(ret);
        }

        return list;
    }

    //
    public static List<double> ParseArrayDouble(string str)
    {
        List<double> list = new List<double>();

        if (str.Length == 0)
        {
            return list;
        }

        string[] strArray = SplitArray(str);
		try{
        for (int i = 0; i < strArray.Length; i++)
        {
			double ret = Convert.ToDouble(strArray[i]);
            list.Add(ret);
        }
		}catch(Exception e){
            Debug.LogError (e.Message);
		}

        return list;
    }

    //
    public static List<string> ParseArrayString(string str)
    {
        List<string> list = new List<string>();

        if (str.Length == 0)
        {
            return list;
        }

        string[] strArray = SplitArray(str);

        for (int i = 0; i < strArray.Length; i++)
        {
            list.Add(strArray[i]);
        }

        return list;
    }

	public static long ParseLong (string str)
	{
		if (str.IndexOf ('.') != -1) {
			str = str.Substring (0, str.IndexOf ('.'));
		}
		long num = -2;
		long.TryParse(str, out num);
		if (num == -2)
		{
			Debug.LogError("错误表信息：" + str);
			return -2;
		}
		return Convert.ToInt64(str);
	}
}
