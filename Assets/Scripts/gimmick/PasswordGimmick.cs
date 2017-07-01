using System;
using System.Collections;
using System.Collections.Generic;
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

        private const int MAXNUM = 3;

        private int[] currentNum = new int[MAXNUM];

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
            if (inputPosition >= MAXNUM) { return; }
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
            int pw = currentNum[0] * 100 + currentNum[1] * 10 + currentNum[2];
            if (data.gimmickAnswer == pw)
            {
                RescissionGimmick();
                openAction.Invoke();
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
            }
        }

        public void RescissionGimmick()
        {
            
        }
        #endregion

    }
}
