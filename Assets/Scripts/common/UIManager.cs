using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Komugi.UI;

namespace Komugi
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        
        // 画像切り替え可能オブジェクト最大数
        private const int CHANGEABLE_MAX = 10;

        [SerializeField]
        ItemBarController itemBar;

        [SerializeField]
        private GameObject LeftRightPanel;

        [SerializeField]
        private GameObject ReturnPanel;

        private AlertMenu alert;

        private DialogMenu dialog;
        
        // オブジェクトの表示非表示切替
        private GameObject[][] changeableObjectList;

        private int[] changeableObjectIndex;

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
            alert = gameObject.AddComponent<AlertMenu>();
            dialog = gameObject.AddComponent<DialogMenu>();
        }

        #region =============================== C# private ===============================
        
        

        #endregion

        #region =============================== C# public ===============================

        /// <summary>
        /// アイテムダイアログを出す
        /// </summary>
        /// <param name="itemId"></param>
        public void ShowItemGetDailog(int itemId)
        {
            dialog.OpenDialog(itemId);
        }

        /// <summary>
        /// アラートを出す
        /// </summary>
        /// <param name="message">アラートの本文</param>
        /// <param name="destoryFlg">自動で消えるかどうか</param>
        public void OpenAlert(string message, bool destoryFlg = true, System.Action callback = null)
        {
            alert.OpenAlert(message, destoryFlg, callback);
        }

        public void AddContentToMainCanvas(GameObject content, int next, int preiver)
        {
            content.transform.SetParent(gameObject.transform);
            content.transform.SetAsFirstSibling();
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;

            bool flag = next == 0;

            LeftRightPanel.SetActive(!flag);
            ReturnPanel.SetActive(flag);
            
        }

        // 画像の切り替え
        public void SwitchObject(int index, string name)
        {
            changeableObjectIndex[index]++;
            if (changeableObjectIndex[index] >= changeableObjectList[index].Length) changeableObjectIndex[index] = 0;

            foreach(GameObject o in changeableObjectList[index])
            {
                o.SetActive(false);
            }
            GameObject obj = changeableObjectList[index][changeableObjectIndex[index]];
            obj.SetActive(true);

            Debug.Log("SwitchObject : " + index + "   Active : " + changeableObjectIndex[index]);
        }

        // 切り替えるゲームオブジェクトをリストに登録
        public void AddChangeableObject(GameObject obj, int index)
        {
            int max = obj.transform.childCount;
            changeableObjectList[index] = new GameObject[max];
            changeableObjectIndex[index] = 0;
            for (int i = 0; i < max; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;
                changeableObjectList[index][i] = child;

                Button btn = child.GetComponent<Button>();
                if (btn == null)
                {
                    Debug.LogError("Button Component is not attach!!");
                }

                btn.onClick.AddListener(() => SwitchObject(index, child.name));
            }
        }

        // アイテムバーにアイテムを追加
        public void AddItemToItemBar(int itemId, bool showDialog = true)
        {
            itemBar.AddItemImage(itemId);
            if (showDialog){ ShowItemGetDailog(itemId); }
        }

        /// <summary>
        /// アイテムを削除
        /// </summary>
        /// <param name="itemId"></param>
        public void RemoveItemFromItemBar(int itemId)
        {
            itemBar.DeleteItemFromItemBar(itemId);
        }

        public void ChangeItem(int before, int after)
        {
            itemBar.ChangeItem(before, after);
        }

        // リセット
        public void ResetStage()
        {
            changeableObjectList = new GameObject[CHANGEABLE_MAX][];
            changeableObjectIndex = new int[10];
        }

        #endregion

    }
}