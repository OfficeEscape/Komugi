using System;

namespace Komugi.Gimmick
{
    /// <summary>
    /// クリア状態をチェックのみのギミック
    /// </summary>
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
            }
        }

        public void ReleaseGimmick()
        {
            if (closeObject) closeObject.SetActive(false);
            if (openObject) openObject.SetActive(true);
        }
        #endregion
    }
}
