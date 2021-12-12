using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class UGUISpriteAnimation : MonoBehaviour
{
	private Image _image;
	public Image image{
		get{
			if (_image == null) {
				_image = GetComponent<Image>();
			}
			return _image;
		}
	}

	private CanvasGroup _group;
	public CanvasGroup group{
		get{
			if (_group == null) {
				_group = GetComponent<CanvasGroup>();
			}
			return _group;
		}
	}

//	private Image ImageSource{
//		get{
//			if (image == null) {
//				image = GetComponent<Image>();
//			}
//			return image;
//		}
//	}

	private int mCurFrame = 0;
	private float mDelta = 0;

	public const float FPS = 15;
	public List<Sprite> SpriteFrames = new List<Sprite> ();
	private bool IsPlaying = false;
	public bool Foward = true;
	public bool AutoPlay = false;
	public WrapMode wrapMode;

	public int FrameCount
	{
		get
		{
			return SpriteFrames.Count;
		}
	}


	void OnEnable(){
		if (AutoPlay)
		{
			mCurFrame = -1;
			Play ();
		}
		else
		{
			Stop ();
		}
	}

	void OnDisable(){
		Stop ();
	}

	private void SetSprite(int idx)
	{
		if (idx >= SpriteFrames.Count) {
			return;
		}
		image.sprite = SpriteFrames[idx];
		image.SetNativeSize ();
	}

	public void Play()
	{
		mDelta = 1;
		IsPlaying = true;
		Foward = true;
//		ImageSource.enabled = true;
		group.alpha = 1;
	}

	public void PlayReverse()
	{
		mDelta = 1;
		IsPlaying = true;
		Foward = false;
//		ImageSource.enabled = true;
		group.alpha = 1;
	}


	void Update()
	{
		if (0 == FrameCount)
			return;
		
		if (wrapMode != WrapMode.Loop) {
			if (!IsPlaying) {
				return;
			}
		} else {
			IsPlaying = true;
		}

		mDelta += Time.deltaTime;
		if (mDelta > 1 / FPS)
		{
			mDelta = 0;
			if(Foward)
			{
				mCurFrame++;
			}
			else
			{
				mCurFrame--;
			}

			if (mCurFrame >= FrameCount)
			{
				if (wrapMode == WrapMode.Loop)
				{
					mCurFrame = 0;
				}
				else
				{
					Stop ();
					return;
				}
			}
			else if (mCurFrame<0)
			{
				if (wrapMode == WrapMode.Loop)
				{
					mCurFrame = FrameCount-1;
				}
				else
				{
					Stop ();
					return;
				}          
			}

			SetSprite(mCurFrame);
		}
	}

	public void Pause()
	{
		IsPlaying = false;
	}

	public void Resume()
	{
		if (!IsPlaying)
		{
			IsPlaying = true;
		}
	}

	public void Stop()
	{
		mCurFrame = 0;
		IsPlaying = false;
//		ImageSource.enabled = false;
		group.alpha = 0;
	}

	public void Rewind()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		Play();
	}
}