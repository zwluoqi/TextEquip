using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using XZXD;

public class SpritePacker {

	private string atlasPath;
	private Texture2D mianTexture;
	private Dictionary<string,Sprite> spriteDict = new Dictionary<string, Sprite>();

	private Dictionary<string,SpriteData> spriteDataDict = new Dictionary<string, SpriteData>();

	public List<Sprite> GetCacheSprites(){
		if (spriteDict.Count != spriteDataDict.Count) {
			foreach (var kv in spriteDataDict) {
				if (spriteDict.ContainsKey (kv.Key)) {
					continue;
				}
				var sprite = Sprite.Create (this.mianTexture, kv.Value.rect, kv.Value.pivot);
				sprite.name = kv.Value.name;
				spriteDict [sprite.name] = sprite;
			}
		}
		var items = new List<Sprite> ();
		items.AddRange( spriteDict.Values);
		return items;
	}
	private string atlasName;

	private bool useInnerSettingTxt = true;
	public bool Load(string atlasName){
		this.atlasName = atlasName;
		atlasPath =  "atlas/" + atlasName;

		useInnerSettingTxt = false;
		var realFileName = AssetFileToolUtilManager.Instance.atlas.GetRealSettingTxtMd5FileName (atlasName+".txt", out useInnerSettingTxt);
		if (useInnerSettingTxt) {
			var objs = AssetLoaderManager.Instance.LoadAllResourceBlock(atlasPath);
			if (objs == null || objs.Length==0) {
				// Load ("0");
				return false;
			} else {
				this.mianTexture = objs [0] as Texture2D;
				for (int i = 1; i < objs.Length; i++) {
					spriteDict [objs [i].name] = objs [i] as Sprite;
				}
			}
		} else {
			Debug.LogWarning ("use update file:" + realFileName);
			if (!File.Exists (PathTool.AtlasSavePath + realFileName)) {

			} else {
				var fileRead = File.OpenRead (PathTool.AtlasSavePath + realFileName);
				byte[] all_bytes = new byte[fileRead.Length];
				fileRead.Read (all_bytes, 0, (int)fileRead.Length);
				fileRead.Close ();


				byte[] width = new byte[4];
				byte[] height = new byte[4];
				byte[] pngLength = new byte[4];
				byte[] ex2 = new byte[4];
				Array.Copy (all_bytes, 0, width, 0, 4);
				Array.Copy (all_bytes, 4, height, 0, 4);
				Array.Copy (all_bytes, 8, pngLength, 0, 4);
				Array.Copy (all_bytes, 12, ex2, 0, 4);

				var width_val = BitConverter.ToInt32 (width, 0);
				var height_val = BitConverter.ToInt32 (height, 0);
				var pngLength_val = BitConverter.ToInt32 (pngLength, 0);
				var ex2_val = BitConverter.ToInt32 (ex2, 0);

				var pngBytes = new byte[pngLength_val];
				Array.Copy (all_bytes, 16, pngBytes, 0, pngBytes.Length);
				var spriteBytes = new byte[all_bytes.Length - 16 - pngLength_val];
				Array.Copy (all_bytes, 16 + pngLength_val, spriteBytes, 0, spriteBytes.Length);

				this.mianTexture = UITools.CreateTexture (width_val, height_val, pngBytes);

				var json = System.Text.Encoding.UTF8.GetString (spriteBytes);
				var rows = json.Split (new char[]{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				char[] rowSplit = new char[]{ '\t' };
				foreach (var row in rows) {
					SpriteData sd = new SpriteData ();
					var item = row.Split (rowSplit, StringSplitOptions.RemoveEmptyEntries);
					int i = 0;
					sd.name = item [i++];
					sd.rect = new Rect (float.Parse (item [i++]), float.Parse (item [i++]), float.Parse (item [i++]), float.Parse (item [i++])); 
					sd.pivot = new Vector2 (float.Parse (item [i++]), float.Parse (item [i++]));
					sd.alignment = int.Parse (item [i++]);
					sd.border = new Vector4 (float.Parse (item [i++]), float.Parse (item [i++]), float.Parse (item [i++]), float.Parse (item [i++])); 
					spriteDataDict [sd.name] = sd;
				}
			}
		}



		return true;
	}

	public void Destory(){

		if (useInnerSettingTxt) {
			this.mianTexture = null;
			AssetLoaderManager.Instance.UnloadResource (atlasPath);

		} else {
			GameObject.Destroy (mianTexture);
		}

		spriteDict.Clear ();
		spriteDataDict.Clear ();
	}

	public Sprite GetSprite(string spriteName){
		// if (Main.simpleIconStyle
		// 	&& (this.atlasName == "HeroIcon"
		// 	|| this.atlasName == "EquipIcon"
		// 		|| this.atlasName == "GoodIcon"
		// 		|| this.atlasName == "SkillIcon"
		// 		|| this.atlasName == "PlayerIcon"
		// 	)) {
		// 	return GetSprite0 ("0", false);
		// } else 
		{
			return GetSprite0 (spriteName, true);
		}
	}

	Sprite GetSprite0 (string spriteName, bool b)
	{
		Sprite localSprite;
		if (spriteDict.TryGetValue (spriteName, out localSprite)) {
			return localSprite;
		} else {
			SpriteData sd;
			if (spriteDataDict.TryGetValue (spriteName, out sd)) {
				var sprite = Sprite.Create (this.mianTexture, sd.rect, sd.pivot,100,1, SpriteMeshType.Tight,sd.border);
				sprite.name = sd.name;
				spriteDict [sprite.name] = sprite;
				return sprite;
			} else {
				if (b) {
					return GetSprite0 ("0", false);
				} else {
					return null;
				}
			}
		}
	}
}
