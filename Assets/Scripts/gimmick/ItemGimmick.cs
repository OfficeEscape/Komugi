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

        public int ClearFlag
        {
            get
            {
                return clearflag ? 1 : 0;
            }

            set
            {
                clearflag = value == 1;
                if (clearflag) { RescissionGimmick(); }
            }
        }

        private Action<int> openAction;

        public Action<int> OpenAction
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
            if (data.gimmickAnswer[0] == GimmickManager.Instance.SelectedItem)
            {
                RescissionGimmick();
                openAction.Invoke(1);
                ItemManager.Instance.DeleteItem(data.gimmickAnswer[0]);
            }
        }
    }
}