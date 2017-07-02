using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class PasswordGimmick : GimmickBase, IGimmick
    {

        [SerializeField]
        Button[] panelButton;

        [SerializeField]
        Text display;

        private const string SPACE = "  ";
        
        private int[] currentNum;

        int inputPosition = 0;

        private void Start()
        {
            if (panelButton.Length < 9) { return; }

            int i = 0;
            foreach(Button btn in panelButton)
            {
                btn.name = (i + 1).ToString();
                btn.onClick.AddListener(() =>
                {
                    OnInputPanel(btn);
                });
                i++;
                if (i == 9) { break; }
            }
                

            panelButton[9].onClick.AddListener(() => DeleteNumber());
            panelButton[10].onClick.AddListener(() => CheckPassWord());
        }

        private void OnInputPanel(Button btn)
        {
            if (inputPosition >= currentNum.Length) { return; }
            string num = btn.name;
            Debug.Log("Push " + num);
            currentNum[inputPosition] = int.Parse(num);

            display.text = string.Format("{0}{1}{2}", display.text, num, SPACE);
            inputPosition++;
        }

        /// <summary>
        /// 現在入力した数字を確認
        /// </summary>
        private void CheckPassWord()
        {
            if (inputPosition < 3) { return; }
            if (clearflag) { return; }

            bool clearFlg = true;

            for (int i = 0; i <data.gimmickAnswer.Length; i ++)
            {
                if (data.gimmickAnswer[i] != currentNum[i])
                {
                    clearFlg = false;
                }
            }
            if (clearFlg)
            {
                RescissionGimmick();
                openAction.Invoke(1);
            }

        }

        private void DeleteNumber()
        {
            if (inputPosition <= 0) { return; }

            inputPosition--;
            currentNum[inputPosition] = 0;
            display.text = display.text.Substring(0, inputPosition * (SPACE.Length + 1));
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
                currentNum = new int[data.gimmickAnswer.Length];
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
            
        }
        #endregion

    }
}
