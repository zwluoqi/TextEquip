using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipAttrItemUI:MonoBehaviour
{
        public TMP_Text text;
        public Image line;

        public void SetText(string val)
        {
                text.enabled = true;
                text.text = val;
                line.enabled = false;  
        }

        public void SetLine()
        {
                text.enabled = false;
                line.enabled = true;
        }
}