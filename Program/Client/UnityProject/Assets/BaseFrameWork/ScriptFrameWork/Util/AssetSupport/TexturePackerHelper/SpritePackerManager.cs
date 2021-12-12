using System;
using System.Collections.Generic;
using UnityEngine;




public class SpritePackerManager
{
	private static SpritePackerManager _instance;

	public static SpritePackerManager Instance{
		get{
			if (_instance == null) {
				_instance = new SpritePackerManager ();
			}
			return _instance;
		}
	}


	public Dictionary<string,SpritePacker> spritePackers = new Dictionary<string, SpritePacker>();


	public Sprite GetSprite(string atlasName,string spriteName){
		var sprite =  GetSprite0 (atlasName, spriteName);
		if (sprite == null) {
			return GetSprite0 ("0", "0");
		} else {
			return sprite;
		}
	}

	Sprite GetSprite0 (string atlasName, string spriteName)
	{
		LoadTex (atlasName);
		return spritePackers [atlasName].GetSprite (spriteName);
	}

	private void LoadTex(string atlasName){
		if (!spritePackers.ContainsKey (atlasName)) {
			var tp = new SpritePacker ();
			tp.Load (atlasName);
			spritePackers [atlasName] = tp;
		}
	}


	public List<Sprite> GetSprites (string atlasName)
	{
		LoadTex (atlasName);
		return spritePackers [atlasName].GetCacheSprites();
	}

	public void UnLoadAtlas(string atlasName){
		spritePackers [atlasName].Destory ();
		spritePackers.Remove (atlasName);	
	}

	public void UnLoadSkillAtlas ()
	{
		List<string> unloadAtlas = new List<string> ();
		foreach (var spritePacker in spritePackers) {
			string key = spritePacker.Key.ToLower ();
			if (key.Contains ("skill_release")||
				key.Contains("skill_hit") ||
				key.Contains("fly") ||
				key.Contains("buff")) {
				spritePacker.Value.Destory ();
				unloadAtlas.Add (spritePacker.Key);
			}
		}

	}

	public void Release ()
	{
		foreach (var spritePacker in spritePackers) {
			spritePacker.Value.Destory ();
		}
		spritePackers.Clear ();
	}
}
