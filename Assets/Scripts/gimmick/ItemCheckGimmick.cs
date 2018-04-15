using System;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class ItemCheckGimmick : GimmickBase, IGimmick
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
                if (clearflag) { ReleaseGimmick(); }
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
                CheckItem();
            }
        }

        public void ReleaseGimmick()
        {
            if (closeObject) { closeObject.SetActive(false); }
            if (openObject) { openObject.SetActive(true); }
        }
        #endregion


        private void CheckItem()
        {
            foreach (int item in data.gimmickAnswer)
            {
                if (!ItemManager.Instance.CheckItem(item))
                {
                    return;
                }
            }

            ReleaseGimmick();
        }
    }
}