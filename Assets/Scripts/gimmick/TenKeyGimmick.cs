using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class TenKeyGimmick : GimmickBase, IGimmick
    {
        [SerializeField]
        private Image[] keyArray = null;

        [SerializeField]
        private Vector3[] colorArray = null;

        private int[] answerdata = null;

        // Use this for initialization
        void Start()
        {
            answerdata = new int[keyArray.Length];
            int index = 0;
            foreach(Image img in keyArray)
            {
                Button btn = img.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.AddListener(() => ColorChanage(img));
                }
                img.name = index.ToString();
                index++;
            }
        }

        /// <summary>
        /// カラーチェンジ
        /// </summary>
        /// <param name="img"></param>
        private void ColorChanage(Image img)
        {
            int index = int.Parse(img.name);
            if (index < 0 || index >= keyArray.Length) { return; }

            int colorindex = answerdata[index];
            colorindex++;
            if (colorindex >= colorArray.Length) { colorindex = 0; }

            img.color = new Color(colorArray[colorindex].x, colorArray[colorindex].y, colorArray[colorindex].z);
            answerdata[index] = colorindex;

            CheckAnswer();
        }

        private void CheckAnswer()
        {
            int len = answerdata.Length;
            if (len != data.gimmickAnswer.Length)
            {
                Debug.LogWarning("Tenkey Length Error");
                return;
            }

            bool clearflg = true;

            for (int i = 0; i < len; i++)
            {
                if (answerdata[i] != data.gimmickAnswer[i])
                {
                    clearflg = false;
                    break;
                }
            }

            if (clearflg && !clearflag) { ReleaseGimmick(); }
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