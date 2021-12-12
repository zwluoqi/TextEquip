using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ColorUtil{
	public static readonly string Color_White = "FFFFFF";
	public static readonly string Color_Yellow = "FFFF00";
	public static readonly string Color_Green = "64e181";
	public static readonly string Color_Blue = "57a6ff";
	public static readonly string Color_Purple = "ae73fe";
	public static readonly string Color_GoldRed = "ff0000";
	public static readonly string Color_Red = "d24141";
	public static readonly string Color_RedPurple = "FF0099";

	public static readonly Color Color_Alph = new Color (0, 0, 0, 0);
	public static readonly Color Color_Whit = new Color (1, 1, 1, 1);


	public static string GetQulityColor(int qulity){
		string color = "";
		switch (qulity)
		{
		case 10:
			color = ColorUtil.Color_White;
			break;
		case 11:
			color = ColorUtil.Color_Green;
			break;
		case 12:
			color = ColorUtil.Color_Blue;
			break;
		case 13:
			color = ColorUtil.Color_Purple;
			break;
		case 14:
			color = ColorUtil.Color_Yellow;
			break;
		case 15:
			color = ColorUtil.Color_Red;
			break;
		case 16:
			color = ColorUtil.Color_GoldRed;
			break;
		case 17:
			color = ColorUtil.Color_RedPurple;
			break;
		default:
			color = ColorUtil.Color_Yellow;
			break;
		}
		return color;
	}
}

public class RichTextUtil  {


	public static string AddColor(string source,string colorCode ){
		StringBuilder sb = new StringBuilder ();
		sb.Append ("<color=#");
		sb.Append (colorCode);
		sb.Append (">");
		sb.Append (source);
		sb.Append ("</color>");
		return sb.ToString ();
	}

	public static string AddColor(string source,Color color ){
		return AddColor (source, ColorUtility.ToHtmlStringRGB (color));
	}

	public static string AddColor(string source,int qulity){

		return AddColor(source,ColorUtil.GetQulityColor(qulity));
	
	}

	public static string Blom(string source){
		StringBuilder sb = new StringBuilder ();
		sb.Append ("<b");
		sb.Append (">");
		sb.Append (source);
		sb.Append ("</b>");
		return sb.ToString ();
	}





}
