using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Gimmick
{
    public class LinkageGimmick : MonoBehaviour, IGimmick
    {
        [SerializeField]
        GameObject[] openObjects = null;

        private int clearNumber = 0;

        protected GimmickData data;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => CheckOpenItem());
        }

        /// <summary>
        /// 開けるか確認
        /// </summary>
        private void CheckOpenItem()
        {
            if (clearNumber >= data.gimmickAnswer.Length) { return; }
            if (data.gimmickAnswer[clearNumber] == GimmickManager.Instance.SelectedItem)
            {
                openObjects[clearNumber].SetActive(true);
                clearNumber++;
            }
            openAction.Invoke(clearNumber); 
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
                return clearNumber == data.gimmickAnswer.Length ? 1 : 0;
            }
            set
            {
                clearNumber = value;
                //bool clearflag = clearNumber == data.gimmickAnswer.Length;
                ReleaseGimmick();
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

        /// <summary>
        /// ギミック解除
        /// </summary>
        public void ReleaseGimmick()
        {
            for (int i = 0; i < clearNumber; i ++)
            {
                openObjects[i].SetActive(true);
            }
        }

        #endregion
    }
}