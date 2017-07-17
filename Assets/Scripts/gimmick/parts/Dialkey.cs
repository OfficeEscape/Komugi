using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class Dialkey : MonoBehaviour
    {
        [SerializeField]
        Button dialBtn;

        [SerializeField]
        Image dialImg;

        [SerializeField]
        Sprite[] dialSprite;

        public int DialNumber { get; private set; }

        public Action OnChangeDial = null;

        private const int MAX_NUM = 9;

        private void Start()
        {
            DialNumber = 0;
            dialBtn.onClick.AddListener(() => ChangeDial());
        }

        private void ChangeDial()
        {
            DialNumber++;
            if (DialNumber > MAX_NUM) { DialNumber = 0; }

            int index = DialNumber % dialSprite.Length;
            dialImg.sprite = dialSprite[index];

            if (OnChangeDial != null) { OnChangeDial.Invoke(); }
        }
    }
}
