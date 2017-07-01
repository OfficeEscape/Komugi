using System;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class ItemGimmick : GimmickBase, IGimmick
    {
        #region -------------------------------------インターフェースメソッド-------------------------------------
        public GimmickData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public bool ClearFlag
        {
            get
            {
                return clearflag;
            }

            set
            {
                clearflag = value;
                if (clearflag) { RescissionGimmick(); }
            }
        }

        private Action openAction;

        public Action OpenAction
        {
            get
            {
                return openAction;
            }

            set
            {
                openAction = value;
                SetClickHandler();
            }
        }

        public void RescissionGimmick()
        {
            closeObject.SetActive(false);
            openObject.SetActive(true);
        }
        #endregion

        private void SetClickHandler()
        {
            Button btn = closeObject.GetComponent<Button>();
            if (btn == null) { return; }

            btn.onClick.AddListener(() =>
            {
                CheckCanOpenGimmick();
            });
        }

        private void CheckCanOpenGimmick()
        {
            if (data.gimmickAnswer == GimmickManager.Instance.selectedItem)
            {
                RescissionGimmick();
                openAction.Invoke();
                ItemManager.Instance.DeleteItem(data.gimmickAnswer);
            }
        }
    }
}