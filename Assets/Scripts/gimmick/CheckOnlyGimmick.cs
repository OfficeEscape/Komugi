using System;

namespace Komugi.Gimmick
{
    public class CheckOnlyGimmick : GimmickBase, IGimmick
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
            }
        }

        public void RescissionGimmick()
        {
            closeObject.SetActive(false);
            openObject.SetActive(true);
        }
        #endregion
    }
}
