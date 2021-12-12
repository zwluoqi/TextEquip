using System;
using Script.Game;
using Script.Game.System;
using Script.Game.System.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class TileMapItem : MonoBehaviour
{
        public GameObject back;
        public GameObject cur;
        public GameObject mask;
        public GameObject lockObj;

        public TMP_Text atk;
        public TMP_Text hp;
        
        public CopyEventEntity _copyEventEntity;
        RectTransform _RectTransform;

        
        private void Awake()
        {
                NotificationCenter.Default.AddObserver(this,OnCopyEventEntityStateChange,(int)GameMessageId.BattleCopyEntityStateChange);
                NotificationCenter.Default.AddObserver(this,OnCopyEventEntityChangeConfig,(int)GameMessageId.OnCopyEventEntityChangeConfig);
                _RectTransform = this.GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
                NotificationCenter.Default.RemoveObserver(this);
        }

        public void FreshLock()
        {
                
                if (this._copyEventEntity.CheckLocked())
                {
                        lockObj.SetActive(true);
                        if (this._copyEventEntity.eventState == CopyEventEntityImp.EventState.None)
                        {
                                lockObj.GetComponent<Image>().sprite = 
                                        SpritePackerManager.Instance.GetSprite("copy", "lock_init");
                                

                        }
                        else
                        {
                                lockObj.GetComponent<Image>().sprite = 
                                        SpritePackerManager.Instance.GetSprite("copy", "lock_opened"); 
                        }
                }
                else
                {
                        lockObj.SetActive(false);
                }
        }
        
        public void Init(CopyEventEntity copyEventEntity)
        {
                this.name = copyEventEntity.config.eventType + ":" + copyEventEntity.config.type;
                this._copyEventEntity = copyEventEntity;
                back.SetActive(true);

                FreshBase();
        }

        private void FreshBase()
        {
                if (this._copyEventEntity.eventState != CopyEventEntityImp.EventState.None)
                {
                        mask.SetActive(false);
                        cur.SetActive(true);
                        cur.GetComponent<Animator>().Play("idle");
                }
                else
                {
                        mask.SetActive(true);
                        mask.GetComponent<Animator>().Play("idle");
                        cur.SetActive(false);
                }

                FreshLock();
                mask.GetComponent<Image>().sprite = 
                        SpritePackerManager.Instance.GetSprite("copy", _copyEventEntity.config.maskIcon);;
                
                cur.GetComponent<Image>().sprite =
                        SpritePackerManager.Instance.GetSprite("copy", _copyEventEntity.config.icon);

                if (_copyEventEntity.config.eventType == "battle")
                {
                        atk.text = "" + (int) _copyEventEntity.config.baseAttribute[(int) DictAbilityPropEnum.ATK];
                        hp.text = "" + (int) _copyEventEntity.config.baseAttribute[(int) DictAbilityPropEnum.MAX_HP];
                }
                else
                {
                        atk.text = "";
                        hp.text = "";
                }
        }


        private void OnCopyEventEntityChangeConfig(Notification notification)
        {
                var posIndex = (int)notification.info;
                if (posIndex == _copyEventEntity.config.posIndex)
                {
                        FreshBase();
                }
        }

        private void OnCopyEventEntityStateChange(Notification notification)
        {
                var posIndex = (int)notification.info;
                if (posIndex == _copyEventEntity.config.posIndex)
                {
                        Debug.LogWarning(posIndex+ "state change :"+_copyEventEntity.eventState);
                        switch (_copyEventEntity.eventState)
                        {
                                case CopyEventEntityImp.EventState.Opening:
                                        Opening();
                                        break;
                                case CopyEventEntityImp.EventState.Opened:
                                        Opened();
                                        break;
                                case CopyEventEntityImp.EventState.Actioning:
                                        Actioning();
                                        break;
                                case CopyEventEntityImp.EventState.ActionDone:
                                        Actioned();
                                        break;
                        }
                }
        }

        private void Actioned()
        {
                //逐渐消失
                cur.GetComponent<Animator>().Play("lerp_hide");
        }

        private void Actioning()
        {
                if (_copyEventEntity.config.eventType == "battle")
                {
                        cur.GetComponent<Animator>().Play("show_battle_effect");
                }
                else if(_copyEventEntity.config.eventType == "gift")
                {
                        cur.GetComponent<Animator>().Play("show_gift_effect");
                }
                else if(_copyEventEntity.config.eventType == "blood")
                {
                        cur.GetComponent<Animator>().Play("show_blood_effect");
                }
        }

        private void Opened()
        {
                mask.SetActive(false);
        }

        private void Opening()
        {
                mask.SetActive(true);
                mask.GetComponent<Animator>().Play("lerp_hide");
                cur.SetActive(true);
                cur.GetComponent<Animator>().Play("lerp_show");
        }


}