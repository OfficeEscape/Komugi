using System.Collections;
using UnityEngine;

namespace Komugi.UI
{
    public class AlertMenu : MonoBehaviour
    {
        // アラートのパス
        private const string ITEM_ALERT_PATH = "Prefabs/ui/Alert";

        private const float LIFT_TIME = 1.5f;

        private GameObject content = null;

        public bool IsOpen { get; private set; }

        /// <summary>
        /// アラートを読み込んで出す
        /// </summary>
        /// <param name="message">アラートの本文</param>
        /// <param name="autoDestoryFlg">自動で消えるかどうか</param>
        public void OpenAlert(string message, bool autoDestoryFlg = true, System.Action callback = null)
        {
            if (IsOpen) { return; }
            StartCoroutine(LoadAsyncAlertCoroutine(message, autoDestoryFlg, callback));
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncAlertCoroutine(string message, bool autoDestoryFlg = true, System.Action callback = null)
        {
            IsOpen = true;
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(ITEM_ALERT_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading Alert progress:" + resReq.progress.ToString());
                yield return 0;
            }
            // テクスチャ表示
            Debug.Log("Loading Alert End  " + Time.time.ToString());

            //アラートを出す
            content = Instantiate(resReq.asset as GameObject, gameObject.transform);
            Alert script = content.GetComponent<Alert>();

            if (script != null)
            {
                script.SetAlertText(message);
            }

            if (message.Length == 0)
            {
                content.transform.GetChild(0).gameObject.SetActive(false);
            }

            content.transform.SetParent(gameObject.transform);

            if (autoDestoryFlg)
            {
                yield return new WaitForSeconds(LIFT_TIME);
                Destroy(content);
                content = null;
                IsOpen = false;

                if (callback != null) { callback.Invoke(); }
            }
        }
    }
}