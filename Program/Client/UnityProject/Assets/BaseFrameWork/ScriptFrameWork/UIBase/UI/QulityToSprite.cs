using System;

public class QulityToSprite
{
	public static string GetFrameByQuality(int qulity){
		string spriteName = "";
		switch (qulity)
		{
		case 10:
			spriteName = "UI_kuang_big_bian_white";
			break;
		case 11:
			spriteName = "UI_kuang_big_bian_green";
			break;
		case 12:
			spriteName = "UI_kuang_big_bian_blue";
			break;
		case 13:
			spriteName = "UI_kuang_big_bian_zi";
			break;
		case 14:
			spriteName = "UI_kuang_big_bian_yellow";
			break;
		case 15:
			spriteName = "UI_kuang_big_bian_red";
			break;
		case 16:
			spriteName = "UI_kuang_big_bian_gold";
			break;
		default:
			spriteName = "UI_kuang_big_bian_white";
			break;
		}
		return spriteName;
	}

	public static string GetFrameSuipianByQuality(int qulity){
		string spriteName = "";
		switch (qulity)
		{
		case 10:
			spriteName = "UI_kuang_sui_white";
			break;
		case 11:
			spriteName = "UI_kuang_sui_green";
			break;
		case 12:
			spriteName = "UI_kuang_sui_blue";
			break;
		case 13:
			spriteName = "UI_kuang_sui_zi";
			break;
		case 14:
			spriteName = "UI_kuang_sui_yellow";
			break;
		case 15:
			spriteName = "UI_kuang_sui_red";
			break;
		case 16:
			spriteName = "UI_kuang_sui_gold";
			break;
		default:
			spriteName = "UI_kuang_sui_white";
			break;
		}
		return spriteName;
	}

	public static string GetDiByQuality(int qulity){
		string spriteName = "";
		switch (qulity)
		{
		case 10:
			spriteName = "UI_kuang_big_di_white";
			break;
		case 11:
			spriteName = "UI_kuang_big_di_green";
			break;
		case 12:
			spriteName = "UI_kuang_big_di_blue";
			break;
		case 13:
			spriteName = "UI_kuang_big_di_zi";
			break;
		case 14:
			spriteName = "UI_kuang_big_di_yellow";
			break;
		case 15:
			spriteName = "UI_kuang_big_di_red";
			break;
		case 16:
			spriteName = "UI_kuang_big_di_gold";
			break;
		default:
			spriteName = "UI_kuang_big_di_white";
			break;
		}
		return spriteName;
	}


	public static string GetSuiPianByQuality(int qulity){
		string spriteName = "";
		switch (qulity)
		{
		case 10:
			spriteName = "UI_kuang_sui_white";
			break;
		case 11:
			spriteName = "UI_kuang_sui_green";
			break;
		case 12:
			spriteName = "UI_kuang_sui_blue";
			break;
		case 13:
			spriteName = "UI_kuang_sui_zi";
			break;
		case 14:
			spriteName = "UI_kuang_sui_yellow";
			break;
		case 15:
			spriteName = "UI_kuang_sui_red";
			break;
		case 16:
			spriteName = "UI_kuang_sui_gold";
			break;
		default:
			spriteName = "UI_kuang_sui_white";
			break;
		}
		return spriteName;
	}
}

