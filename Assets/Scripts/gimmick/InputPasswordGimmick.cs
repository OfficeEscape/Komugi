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

        [SerializeField]
        bool IsAlphabet = true;

        bool autoCheck = true;

        private const string SPACE = "     ";
        private const string ALPHABET = "ABCDEFG";
        
        private void Start()
        {
            inputField.contentType = InputField.ContentType.EmailAddress;
            inputField.caretBlinkRate = 1;
            inputField.onEndEdit.AddListener((t) => OnEditEnd(t));

            inputButton.onClick.AddListener(() => inputField.ActivateInputField());
            if (openButton)
            {
                openButton.onClick.AddListener(() => CheckPassWord());
                autoCheck = false;
            }
            else
            {
                inputField.onValueChanged.AddListener((pw) => CheckPassWord());
            }
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
            if (inputField.text.Length != data.gimmickAnswer.Length) { return; }

            bool clearFlg = true;

            for (int i = 0; i < data.gimmickAnswer.Length; i++)
            {
                if (inputField.text.Length <= i) { return; }
                char target = IsAlphabet ? ALPHABET[data.gimmickAnswer[i]] : char.Parse(data.gimmickAnswer[i].ToString());
                if (target != inputField.text[i])
                {
                    clearFlg = false;
                }
            }
            if (clearFlg)
            {
                ReleaseGimmick();
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
                inputField.characterLimit = data.gimmickAnswer.Length;
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

        }
        #endregion
    }
}