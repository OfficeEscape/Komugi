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

        [SerializeField]
        int MultiAnswerCount = 1;

        [SerializeField]
        InputField.ContentType type = InputField.ContentType.Alphanumeric;

        //bool autoCheck = true;

        private const string SPACE = "     ";
        private const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
        //private const string ALPHABET1 = "abc def ghi jkl mno pqr stu vwx yz";

        private void Start()
        {
            inputField.contentType = type;
            inputField.caretBlinkRate = 1;
            inputField.onEndEdit.AddListener((t) => OnEditEnd(t));

            inputButton.onClick.AddListener(() => inputField.ActivateInputField());
            if (openButton)
            {
                openButton.onClick.AddListener(() => CheckPassWord());
                //autoCheck = false;
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

            var formatstr = new System.Text.StringBuilder(10);
            for (int i = 0; i < inputField.characterLimit; i++)
            {
                if (result.Length <= i) { break; }
                formatstr = formatstr.Append(result[i]).Append(" ");
            }

            display.text = formatstr.ToString();
        }

        /// <summary>
        /// 現在入力した数字を確認
        /// </summary>
        private void CheckPassWord()
        {
            if (clearflag) { return; }

            int len = data.gimmickAnswer.Length / MultiAnswerCount;
            if (inputField.text.Length != len) { return; }

            int clearFlg = 0;

            for (int i = 0; i < MultiAnswerCount; i++)
            {
                int[] ans = new int[len];
                Array.Copy(data.gimmickAnswer, i * len, ans, 0, len);
                if (StringCompare(ans))
                {
                    clearFlg = i + 1;
                }
            }
            
            if (clearFlg > 0)
            {
                ReleaseGimmick();
                openAction.Invoke(clearFlg);
            }
            else
            {
                SoundManger.Instance.PlaySe(AudioConst.SE_ELEVATOR_FAIL);
                UIManager.Instance.OpenAlert("パスワードが違います", true, () => {
                    inputField.text = string.Empty;
                    display.text = string.Empty;
                });
            }

        }

        private bool StringCompare(int[] correct)
        {
            string inputAnswer = inputField.text.ToLower();
            for (int i = 0; i < correct.Length; i++)
            {
                if (inputAnswer.Length <= i) { return false; }
                char target = IsAlphabet ? ALPHABET[correct[i]] : char.Parse(correct[i].ToString());
                if (target != inputAnswer[i])
                {
                    return false;
                }
            }

            return true;
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
                inputField.characterLimit = data.gimmickAnswer.Length / MultiAnswerCount;
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