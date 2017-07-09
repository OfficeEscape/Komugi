using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Komugi.UI
{
    public class DialogMenu : MonoBehaviour
    {
        // アイテムダイアログのパス
        private const string ITEM_DIALOG_PATH = "Prefabs/ui/ItemDialog";

        private GameObject content = null;

        public bool IsOpen { get; private set; }


        /// <summary>
        /// ダイアログを出す
        /// </summary>
        /// <param name="itemId"></param>
        public void OpenDialog(int itemId)
        {
            if (IsOpen)
            {
                content.GetComponent<ItemDialog>().UpdateItem(ItemManager.Instance.itemDictionary[itemId]);
            }
            else
            {
                StartCoroutine(LoadAsyncDialogCoroutine(itemId));
            }
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncDialogCoroutine(int itemId)
        {
            IsOpen = true;
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

            //ダイアログを出す
            content = Instantiate(resReq.asset as GameObject, gameObject.transform);
            ItemDialog script = content.GetComponent<ItemDialog>();

            if (script != null)
            {
                script.CloseCallBack = () => { IsOpen = false; };
                script.UpdateItem(ItemManager.Instance.itemDictionary[itemId]);
            }

            content.transform.SetParent(gameObject.transform);
        }
    }
}
