using Script.Game.Grow;
using XZXD.UI;

public class EquipInfoPage:UIPage
{
        public EquipInfoCtrl equipInfoCtrl;

        protected override void DoOpen()
        {
                var equip_guid = this.GetLongOptionValue("equip_guid");
                var equip = GrowFun.Instance.growData.GetEquipByGuid(equip_guid);
                equipInfoCtrl.Open(equip);
        }
}