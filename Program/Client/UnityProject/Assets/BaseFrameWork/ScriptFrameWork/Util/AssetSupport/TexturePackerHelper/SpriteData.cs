using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public struct SpriteRect{
	public float x;
	public float y;
	public float width;
	public float height;

	public SpriteRect(Rect rect){
		this.x = rect.x;
		this.y = rect.y;
		this.width = rect.width;
		this.height = rect.height;

	}


	public Rect ToUnity(){
		return new Rect (this.x, this.y, this.width, this.height);
	}

}
[Serializable]
public struct SpriteVector2{
	public float x;
	public float y;

	public SpriteVector2(Vector2 rect){
		this.x = rect.x;
		this.y = rect.y;


	}


	public Vector2 ToUnity(){
		return new Vector2 (this.x, this.y);
	}
}
[Serializable]
public struct SpriteVector4{
	public float x;
	public float y;
	public float z;
	public float w;


	public SpriteVector4(Vector4 rect){
		this.x = rect.x;
		this.y = rect.y;
		this.z = rect.z;
		this.w = rect.w;

	}


	public Vector4 ToUnity(){
		return new Vector4 (this.x, this.y, this.z, this.w);
	}
}

[Serializable]
public struct SpriteData
{
	//
	// Fields
	//
	public string name;

	public Rect rect;

	public int alignment;

	public Vector2 pivot;

	public Vector4 border;
}

