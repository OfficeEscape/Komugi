using System;
using System.Collections;
using UnityEngine;

namespace Komugi.UI
{
    public class MainMenu : MonoBehaviour
    {
        private enum ButtonType
        {
            Help,
            Setting,
            Hint,
            Return,
            Close,
        };

        // メインメニューのパス
        private const string MAIN_MENU_PATH = "Prefabs/ui/menu_bar";

        private const string HELP_WINDOW_PATH = "Prefabs/ui/help_window";

        private const string SETTING_WINDOW_PATH = "Prefabs/ui/setting_dialog";

        private const string HINT_WINDOW_PATH = "Prefabs/ui/hint_window";

        private const string RETURN_DIALOG = "タイトルに戻ります。\nよろしいですか";

        private GameObject content = null;

        private GameObject childcontent = null;

        public bool IsOpen { get; private set; }

        private bool isLoading = false;

        private int selected = -1;

        /// <summary>
        /// メインメニューを開く
        /// </summary>
        public void OpenMainMenu()
        {
            if (IsOpen) { return; }
            StartCoroutine(LoadAsyncMainMenuCoroutine());
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncMainMenuCoroutine(System.Action callback = null)
        {
            IsOpen = true;
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(MAIN_MENU_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                DebugLogger.Log("Loading MainMenu progress:" + resReq.progress.ToString());
                yield return 0;
            }
            // テクスチャ表示
            DebugLogger.Log("Loading MainMenu End  " + Time.time.ToString());

            //メインメニューを出す
            content = Instantiate(resReq.asset as GameObject, gameObject.transform);

            content.transform.SetParent(gameObject.transform, false);

            Menu script = content.GetComponent<Menu>();
            if (script != null)
            {
                script.CallBack = (index) => { OnMenuButtonClick(index); };
            }
            
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncPrefab(string path)
        {
            isLoading = true;

            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(path);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                DebugLogger.Log("Loading " + path + " progress:" + resReq.progress.ToString());
                yield return 0;
            }

            DebugLogger.Log("Loading " + path + " End  " + Time.time.ToString());

            //メインメニューを出す
            childcontent = Instantiate(resReq.asset as GameObject, gameObject.transform);

            childcontent.transform.SetParent(gameObject.transform, false);

            isLoading = false;
        }

        /// <summary>
        ///  メニューのボタンのどれかが押された
        /// </summary>
        /// <param name="buttonIndex">押されたボタン番号</param>
        private void OnMenuButtonClick(int buttonIndex)
        {
            if (childcontent != null && buttonIndex == selected) { return; }
            if (isLoading) { return; }
            if (childcontent != null) { Destroy(childcontent); }
            
            IsOpen = false;

            switch ((ButtonType)buttonIndex)
            {
                case ButtonType.Help:
                    StartCoroutine(LoadAsyncPrefab(HELP_WINDOW_PATH));
                    //Destroy(content);
                    break;
                case ButtonType.Setting:
                    StartCoroutine(LoadAsyncPrefab(SETTING_WINDOW_PATH));
                    //Destroy(content);
                    break;
                case ButtonType.Hint:
                    StartCoroutine(LoadAsyncPrefab(HINT_WINDOW_PATH));
                    //Destroy(content);
                    break;
                case ButtonType.Return:
                    UIManager.Instance.OpenCheckDialog(RETURN_DIALOG, ReturnTitleCallBack);
                    break;
                case ButtonType.Close:
                    Destroy(content);
                    break;
            }

            selected = buttonIndex;
        }

        private void ReturnTitleCallBack(int res)
        {
            if (res == (int)CheckDialog.ResultType.OK)
            {
                GameManager.Instance.OnReturnToTitle();
                Destroy(content);
            }
        }
    }
}