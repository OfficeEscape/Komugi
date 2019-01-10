using UnityEngine;
using UnityEngine.UI;
using Komugi.UI;
using System;

namespace Komugi
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        // 画像切り替え可能オブジェクト最大数
        private const int CHANGEABLE_MAX = 10;

        private const int LAYER_SE_OVERWRITE = 8;

        private const float STANDARD_ASPECT = 0.56f;

        [SerializeField]
        private ItemBarController itemBar;

        [SerializeField]
        private GameObject LeftRightPanel;

        [SerializeField]
        private GameObject ReturnPanel;

        private MainMenu mainMenu;

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

            mainMenu = gameObject.AddComponent<MainMenu>();
            alert = gameObject.AddComponent<AlertMenu>();
            dialog = gameObject.AddComponent<DialogMenu>();

            itemBar.MenuButtonHandle = () => { mainMenu.OpenMainMenu(); };

            // iPhoneX対応
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

            if (canvasScaler != null)
            {
                float currentAspect = (float)Screen.width / Screen.height;
                // 1 Height 0 Width
                canvasScaler.matchWidthOrHeight = currentAspect < STANDARD_ASPECT ? 0.0f : 1.0f;
            }
        }

        #region =============================== C# private ===============================
        
        #endregion

        #region =============================== C# public ===============================

        public bool IsItemDialogOpen()
        {
            return dialog.IsOpen;
        }

        /// <summary>
        /// アイテムダイアログを出す
        /// </summary>
        /// <param name="itemId"></param>
        public void ShowItemGetDailog(int itemId, int itemIndex)
        {
            dialog.OpenDialog(itemId, itemIndex);
        }

        public void OpenCheckDialog(string message, Action<int> callBack = null, bool okOnly = false)
        {
            dialog.OpenCheckDialog(message, callBack, okOnly);
        }

        public void UpdateItemDailog(int itemIndex)
        {
            dialog.OpenDialog(itemIndex);
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
            content.transform.SetParent(gameObject.transform, false);
            content.transform.SetSiblingIndex(1);

            LeftRightPanel.SetActive(next != 0);
            ReturnPanel.SetActive(next == 0 && preiver != 0);
        }

        public void AddContentToMainCanvas(GameObject content)
        {
            content.transform.SetParent(gameObject.transform, false);
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
            if (obj.layer != LAYER_SE_OVERWRITE)
            {
                GameManager.Instance.PlaySE(AudioConst.SE_SWITCH_SE);
            }

            DebugLogger.Log("SwitchObject : " + index + "   Active : " + changeableObjectIndex[index]);
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
                if (btn != null)
                {
                    btn.onClick.AddListener(() => SwitchObject(index, child.name));
                }
                
                if (i != 0) { child.SetActive(false); }
            }
        }

        // アイテムバーにアイテムを追加
        public void AddItemToItemBar(int itemId, bool showDialog = true)
        {
            int itemIndex = itemBar.AddItemImage(itemId);
            if (showDialog){ ShowItemGetDailog(itemId, itemIndex); }
        }

        /// <summary>
        /// アイテムを削除
        /// </summary>
        /// <param name="itemId"></param>
        public void RemoveItemFromItemBar(int itemId)
        {
            itemBar.DeleteItemFromItemBar(itemId);
        }

        public bool ChangeItem(int before, int after, int itemIndex = -1)
        {
            return itemBar.ChangeItem(before, after, itemIndex);
        }

        // リセット
        public void ResetStage()
        {
            changeableObjectList = new GameObject[CHANGEABLE_MAX][];
            changeableObjectIndex = new int[10];
        }

        public void SetItemBarTouchEnable(bool enable)
        {
            itemBar.TouchEnable = enable;
        }

        #endregion

    }
}