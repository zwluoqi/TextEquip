using TMPro;
using UnityEngine;
using XZXD.UI;

public class TipsPage:UIPage
{
        public TMP_Text tex;

        public RectTransform back;
        
        public void SetTip(string getLanguage)
        {
                tex.text = getLanguage;
                back.sizeDelta = new Vector2(back.sizeDelta.x, 100 + tex.preferredHeight);
        }
}