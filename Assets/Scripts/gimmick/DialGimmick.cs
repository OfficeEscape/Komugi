using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class DialGimmick : GimmickBase, IGimmick
    { 

        [SerializeField]
        Dialkey[] dialkey;

        [SerializeField]
        Text display;

        private const string SPACE = " ";

        private void Start()
        {
            foreach (Dialkey key in dialkey)
            {
                key.OnChangeDial = SetDialNumber;
            }
        }

        private void SetDialNumber()
        {
            var strBuilder = new System.Text.StringBuilder(dialkey.Length * 2);

            bool correct = true;
            int i = 0;
            foreach(Dialkey key in dialkey)
            {
                strBuilder.Append(key.DialNumber.ToString());
                strBuilder.Append(SPACE);

                if (data.gimmickAnswer.Length <= i || key.DialNumber != data.gimmickAnswer[i])
                {
                    correct = false;
                }
                i++;
            }
            display.text = strBuilder.ToString();

            if (correct)
            {
                if (clearflag) { return; }
                ReleaseGimmick();
            }
        }

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
            if (openAction != null)
            {
                openAction.Invoke(1);
            }
        }
        #endregion
    }
}