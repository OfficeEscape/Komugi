﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Komugi.UI;

namespace Komugi
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        // アイテムダイアログのパス
        private const string itemDialogPath = "Prefabs/ui/ItemDialog";

        // 画像切り替え可能オブジェクト最大数
        private const int CHANGEABLE_MAX = 10;

        [SerializeField]
        ItemBarController itemBar;

        [SerializeField]
        private GameObject LeftRightPanel;

        [SerializeField]
        private GameObject ReturnPanel;
        
        // オブジェクトの表示非表示切替
        private GameObject[][] changeableObjectList;

        private int[] changeableObjectIndex;

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
        }

        #region =============================== C# private ===============================

        // アイテムゲットのダイアログ
        public void ShowItemGetDailog(int itemId)
        {
            StartCoroutine(LoadAsyncDialogCoroutine(itemId));
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncDialogCoroutine(int itemId)
        {
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(itemDialogPath);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading Dialog progress:" + resReq.progress.ToString());
                yield return 0;
            }
            // テクスチャ表示
            Debug.Log("Loading Dialog End  " + Time.time.ToString());

            //ダイアログを出す
            GameObject dialog = Instantiate(resReq.asset as GameObject, gameObject.transform);
            ItemDialog script = dialog.GetComponent<ItemDialog>();
            
            if (script != null)
            {
                script.UpdateItem(ItemManager.Instance.itemDictionary[itemId]);
            }

            dialog.transform.SetParent(gameObject.transform);
        }

        #endregion

        #region =============================== C# public ===============================

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

        // リセット
        public void ResetStage()
        {
            changeableObjectList = new GameObject[CHANGEABLE_MAX][];
            changeableObjectIndex = new int[10];
        }

        #endregion

    }
}