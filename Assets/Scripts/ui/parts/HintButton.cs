using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{ 
    public class HintButton : MonoBehaviour
    {
        [SerializeField]
        Image hintCircle;

        [SerializeField]
        Button button;

        [SerializeField]
        Image candy;

        [SerializeField]
        Text candyNum;

        [SerializeField]
        Text hintTitle;

        [SerializeField]
        Sprite[] circleSprite;
        
        [SerializeField]
        Color[] textColor;

        public Action<HintData> clickHandle = null;

        private HintData hintData = new HintData();

        public HintData Data { get { return hintData; } }

        /// <summary>
        /// ヒントのデータをセット
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clear"></param>
        /// <param name="hasCandyNum">所持アメの数</param>
        public void SetData(HintData data, bool clear, bool paid, int hasCandyNum)
        {
            hintTitle.text = data.title;
            candyNum.text = string.Format("{0}消費", data.candyNum);

            hintData = data;

            button.onClick.AddListener(() =>
            {
                if (clickHandle != null)
                {
                    clickHandle(hintData);
                }
            });

            SetActive(clear, paid, hasCandyNum);
        }

        public void SetActive(bool clear, bool paid, int hasCandyNum)
        {
            bool active = hintData.candyNum <= hasCandyNum;

            hintCircle.sprite = circleSprite[0];
            hintTitle.color = candyNum.color = textColor[0];
            button.enabled = true;

            // アメ支払い済みかギミッククリア済み
            if (clear || paid)
            {
                candyNum.gameObject.SetActive(false);
                return;
            }

            if (!active)
            {
                button.enabled = false;
                hintCircle.sprite = circleSprite[1];
                hintTitle.color = candyNum.color = textColor[1];
            }
        }
    }
}