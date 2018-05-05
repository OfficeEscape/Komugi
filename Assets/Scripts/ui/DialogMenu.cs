using System;
using System.Collections;
using UnityEngine;

namespace Komugi.UI
{
    public class DialogMenu : MonoBehaviour
    {
        // アイテムダイアログのパス
        private const string ITEM_DIALOG_PATH = "Prefabs/ui/ItemDialog";

        // 確認ダイアログのパス
        private const string CHECK_DIALOG_PATH = "Prefabs/ui/OkCancelDialog";

        private GameObject content = null;

        private bool isLoading = false;
        
        public bool IsOpen { get; private set; }
        
        /// <summary>
        /// アイテムダイアログを出す
        /// </summary>
        /// <param name="itemId"></param>
        public void OpenDialog(int itemId, int itemIndex)
        {
            if (isLoading) { return; }
            
            if (IsOpen)
            {
                content.GetComponent<ItemDialog>().UpdateItem(ItemManager.Instance.itemDictionary[itemId], itemIndex);
            }
            else
            {
                StartCoroutine(LoadAsyncDialogCoroutine(itemId, itemIndex));
            }
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncDialogCoroutine(int itemId, int itemIndex)
        {
            isLoading = true;
            
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(ITEM_DIALOG_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading Dialog progress:" + resReq.progress.ToString());
                yield return 0;
            }
            // テクスチャ表示
            Debug.Log("Loading Dialog End  " + Time.time.ToString());

            IsOpen = true;

            //ダイアログを出す
            content = Instantiate(resReq.asset as GameObject, gameObject.transform);
            ItemDialog script = content.GetComponent<ItemDialog>();

            if (script != null)
            {
                script.CloseCallBack = () => { IsOpen = false; };
                script.UpdateItem(ItemManager.Instance.itemDictionary[itemId], itemIndex);
            }

            content.transform.SetParent(gameObject.transform);
            isLoading = false;
        }

        /// <summary>
        /// アイテムダイアログを出す
        /// </summary>
        /// <param name="msg"></param>
        public void OpenCheckDialog(string msg, Action<int> resultCallBack = null, bool okOnly = false)
        {
            if (isLoading) { return; }

            if (!IsOpen)
            {
                StartCoroutine(LoadAsyncCheckDialogCoroutine(msg, resultCallBack, okOnly));
            }
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncCheckDialogCoroutine(string msg, Action<int> resultCallBack = null, bool okOnly = false)
        {
            isLoading = true;
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(CHECK_DIALOG_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading Dialog progress:" + resReq.progress.ToString());
                yield return 0;
            }
            // テクスチャ表示
            Debug.Log("Loading Dialog End  " + Time.time.ToString());

            //ダイアログを出す
            content = Instantiate(resReq.asset as GameObject, gameObject.transform, false);
            CheckDialog script = content.GetComponent<CheckDialog>();

            if (script != null)
            {
                script.CallBack = resultCallBack;
                script.SetData(msg, okOnly);
            }

            isLoading = false;
        }
    }
}
