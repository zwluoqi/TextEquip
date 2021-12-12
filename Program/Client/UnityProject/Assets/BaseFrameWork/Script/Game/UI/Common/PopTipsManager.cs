using UnityEngine;

namespace XZXD.UI
{
    public class PopTipsManager
    {
        public MulityPopMessage mulityPopMessage;
        public void ShowTips(string desc)
        {
            if (mulityPopMessage == null)
            {
                var go = UITools.LoadUIObject("ui/pages/MulityPopMessage",UIManager.Instance.GetLayerRoot(UILayerEnum.ATTENTION));
                mulityPopMessage = go.GetComponent<MulityPopMessage>();
            }
            mulityPopMessage.ShowMessage(desc);
        }
    }
}