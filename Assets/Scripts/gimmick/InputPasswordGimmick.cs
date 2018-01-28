using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class InputPasswordGimmick : GimmickBase, IGimmick
    {

        [SerializeField]
        InputField inputField;

        [SerializeField]
        Text display;

        [SerializeField]
        Button inputButton;

        [SerializeField]
        Button openButton;

        private const string SPACE = "     ";
        private const string ALPHABET = "ABCDEFG";
        

        private void Start()
        {
            inputField.characterLimit = 3;
            inputField.contentType = InputField.ContentType.EmailAddress;
            inputField.caretBlinkRate = 1;
            inputField.onEndEdit.AddListener((t) => OnEditEnd(t));

            inputButton.onClick.AddListener(() => inputField.ActivateInputField());
            if (openButton) { openButton.onClick.AddListener(() => CheckPassWord()); }
        }

        private void OnEditEnd(string pw)
        {
            var tempText = new System.Text.StringBuilder(10);

            tempText = tempText.Append(pw).Append(SPACE);

            string result = tempText.ToString();

            display.text = string.Format("{0} {1} {2}", result[0], result[1], result[2]);
        }

        /// <summary>
        /// 現在入力した数字を確認
        /// </summary>
        private void CheckPassWord()
        {
            if (clearflag) { return; }

            bool clearFlg = true;

            for (int i = 0; i < data.gimmickAnswer.Length; i++)
            {
                if (inputField.text.Length <= i) { return; }
                if (ALPHABET[data.gimmickAnswer[i]] != inputField.text[i])
                {
                    clearFlg = false;
                }
            }
            if (clearFlg)
            {
                RescissionGimmick();
                openAction.Invoke(1);
            }
            else
            {
                UIManager.Instance.OpenAlert("パスワードが違います", true);
                inputField.text = string.Empty;
                display.text = string.Empty;
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