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

        private bool clearFlg = false;

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
            if (clearFlg) { return; }

            for (int i = 0; i < data.gimmickAnswer.Length; i++)
            {
                if (data.gimmickAnswer[i] == GimmickManager.Instance.SelectedItem)
                {
                    openObjects[i].SetActive(true);
                    clearNumber += (1 << i);

                    ItemManager.Instance.DeleteItem(data.gimmickAnswer[i]);
                }
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
                for (int i = 0; i < data.gimmickAnswer.Length; i++)
                {
                    if ((clearNumber & (1 << i)) == 0)
                    {
                        return 0;
                    }
                }
                return 1;
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
            clearFlg = true;

            for (int i = 0; i < data.gimmickAnswer.Length; i++)
            {
                if ((clearNumber & (1 << i)) > 0)
                {
                    openObjects[i].SetActive(true);
                }
                else
                {
                    clearFlg = false;
                }
            }
        }

        #endregion
    }
}