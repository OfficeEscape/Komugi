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

        [SerializeField]
        // アイテム表示領域
        private Image[] ItemImages;

        // 現在持ってるアイテム数
        private int itemNum;

        // オブジェクトの表示非表示切替
        private GameObject[] changeableObjectList;

        private int changeableObjectIndex = 0;

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
        }

        // Use this for initialization
        void Start()
        {
            itemNum = 0;
        }

        #region =============================== C# private ===============================

        // アイテムゲットのダイアログ
        private void ShowItemGetDailog(int itemId)
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
            GameObject dialog = Instantiate(resReq.asset as GameObject);
            ItemDialog script = dialog.GetComponent<ItemDialog>();
            
            if (script != null)
            {
                script.UpdateItem(ItemManager.Instance.GetItemImage(itemId), ItemManager.Instance.GetItemName(itemId));
            }

            dialog.transform.SetParent(gameObject.transform);
            dialog.transform.localPosition = Vector3.zero;
            dialog.transform.localScale = Vector3.one;
        }

        #endregion

        #region =============================== C# public ===============================

        public void SwitchObject(int index, string name)
        {
            changeableObjectIndex++;
            if (changeableObjectIndex >= changeableObjectList.Length) changeableObjectIndex = 0;

            foreach(GameObject o in changeableObjectList)
            {
                o.SetActive(false);
            }
            GameObject obj = changeableObjectList[changeableObjectIndex];
            obj.SetActive(true);

            Vector2 size = obj.GetComponent<RectTransform>().sizeDelta;
        }

        // 切り替えるゲームオブジェクトをリストに登録
        public void AddChangeableObject(GameObject obj)
        {
            changeableObjectList = null;
            changeableObjectIndex = 0;

            int max = obj.transform.childCount;
            changeableObjectList = new GameObject[max];
            for (int i = 0; i < max; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;
                changeableObjectList[i] = child;

                Button btn = child.GetComponent<Button>();
                if (btn == null)
                {
                    Debug.LogError("Button Component is not attach!!");
                }

                btn.onClick.AddListener(() => SwitchObject(0, ""));
            }
        }

        // アイテムバーにアイテムを追加
        public void AddItemToItemBar(int itemId)
        {
            if (ItemImages[itemNum].enabled)
            {
                Debug.Log("item " + itemNum + " is Registered");
                return;
            }

            ShowItemGetDailog(itemId);

            Sprite itemSprite = ItemManager.Instance.GetItemImage(itemId);
            if (itemSprite != null)
            {
                ItemImages[itemNum].sprite = itemSprite;
                ItemImages[itemNum].enabled = true;
                ItemImages[itemNum].SetNativeSize();
            }
        }

        #endregion

    }
}