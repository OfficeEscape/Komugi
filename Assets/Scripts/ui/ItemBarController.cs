using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

namespace Komugi.UI
{ 
    public class ItemBarController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float MARGIN = 115f;

        private const float USE_TIME = 0.5f;

        private const int COLUMN = 5;

        private const int ROW = 2;

        [SerializeField]
        // アイテム表示領域
        private Image[] ItemImages;

        [SerializeField]
        // カーソル
        private RectTransform cursor;
        
        // アイテムIDの配列
        private List<int> itemIdList;

        // 現在のページ数
        private int currentPage = 0;

        // 最大ページ数
        private int maxPage = 1;

        // 最後にタッチしたアイテム
        private int lastTouchItem = 0;

        // 選択中のアイテムのインデックス
        private int selectedIndex = -1;

        /// <summary>
        /// メニューボタン押されたら
        /// </summary>
        public Action MenuButtonHandle { get; set; }

        public bool TouchEnable { get; set; }

        #region =============================== タップ領域計算用フィールド ===============================

        private float touchStart = 0f;

        private float touchEnd = 0f;

        private float itemWidth = 0f;

        private float itemHeight = 0f;

        #endregion

        // Use this for initialization
        void Start ()
        {
            float widthRatio = Screen.width / 1080f;
            float heightRatio = Screen.height / 1920f;

            float currentMargin = MARGIN * widthRatio;
            DebugLogger.Log("currentMargin" + currentMargin);
            touchStart = currentMargin;
            touchEnd = Screen.width - currentMargin;
            float wd = Screen.width - (currentMargin * 2f);
            itemWidth = wd / (float)COLUMN;
            DebugLogger.Log("itemWidth" + itemWidth);
            itemHeight = 384f * heightRatio / (float)ROW;
            DebugLogger.Log("itemHeight" + itemHeight);

            itemIdList = new List<int>();

            TouchEnable = true;
        }


        /// <summary>
        /// アイテムバーに画像を追加
        /// </summary>
        /// <param name="itemId"></param>
        public int AddItemImage(int itemId)
        {
            // ページが埋まったら自動で次のページへ
            if (ItemImages[ItemImages.Length - 1].enabled)
            {
                currentPage++;
                ResetPage();
            }

            int itemNum = itemIdList.Count - (currentPage * ItemImages.Length);
            if (ItemImages[itemNum].enabled)
            {
                DebugLogger.Log("item " + itemNum + " is Registered");
                return -1;
            }
            
            Sprite itemSprite = ItemManager.Instance.GetItemImage(itemId);
            if (itemSprite != null)
            {
                SetItemImage(itemNum, itemSprite);
            }

            itemIdList.Add(itemId);
            maxPage = itemIdList.Count / ItemImages.Length + 1;

            return itemNum;
        }

        // 
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!TouchEnable) { return; }

            int pressIndex = GetPressedItemIndex(eventData.pressPosition);
            if (pressIndex < 0)
            {
                OnTouchOptionButton(eventData.pressPosition);
                return;
            }
            
            if (pressIndex != lastTouchItem) { return; }
            if (!ItemImages[pressIndex].enabled) { return; }

            int itemIndex = lastTouchItem + currentPage * ItemImages.Length;
            int itemIdIndex = itemIdList[itemIndex];

            if (selectedIndex == itemIndex)
            {
                // アイテムダイアログを出す
                if (!UIManager.Instance.IsItemDialogOpen())
                {
                    UIManager.Instance.ShowItemGetDailog(itemIdList[itemIndex], itemIndex);
                }
            }
            else
            {
                // アイテムを使うモード
                DebugLogger.Log("Use Item");
                selectedIndex = itemIndex;
                GimmickManager.Instance.SelectedItem = itemIdList[itemIndex];
                cursor.name = itemIndex.ToString();
                cursor.localPosition = ItemImages[lastTouchItem].rectTransform.localPosition;
                if (!cursor.gameObject.activeSelf) { cursor.gameObject.SetActive(true); }

                // アイテムダイアログを出す
                if (UIManager.Instance.IsItemDialogOpen())
                {
                    UIManager.Instance.ShowItemGetDailog(itemIdList[itemIndex], itemIndex);
                }

            }
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!TouchEnable) { return; }
            // throw new NotImplementedException();
            lastTouchItem = GetPressedItemIndex(eventData.pressPosition);
            DebugLogger.Log("Now Pressed : " + lastTouchItem);
        }

        #region =============================== C# private ===============================

        private void ResetPage()
        {
            foreach (Image img in ItemImages)
            {
                img.sprite = null;
                img.enabled = false;
            }

            cursor.gameObject.SetActive(false);
        }

        private int GetPressedItemIndex(Vector2 pressPosition)
        {
            int index = -1;

            if ( pressPosition.x >= touchStart && pressPosition.x <= touchEnd)
            {
                int x = Mathf.FloorToInt((pressPosition.x - touchStart) / itemWidth);
                float pressY = Screen.height - pressPosition.y;
                int y = Mathf.FloorToInt(pressY / itemHeight);
                index = x + y * COLUMN;
            }
            
            // ページ分もプラス
            return index;
        }

        /// <summary>
        /// アイテムバーからアイテムを消す
        /// </summary>
        public void DeleteItemFromItemBar(int itemId)
        {
            if (lastTouchItem != -1 && itemIdList[lastTouchItem + currentPage * ItemImages.Length] == itemId)
            {
                cursor.gameObject.SetActive(false);
                GimmickManager.Instance.SelectedItem = 0;
            }

            int oldSelectedId = itemIdList[selectedIndex];
            itemIdList.Remove(itemId);
            int newSelectedId = selectedIndex < itemIdList.Count ? itemIdList[selectedIndex] : -1;

            if (oldSelectedId != newSelectedId)
            {
                // アイテムダイアログを出す
                if (UIManager.Instance.IsItemDialogOpen())
                {
                    selectedIndex--;
                    UIManager.Instance.UpdateItemDailog(selectedIndex);
                }
                else
                {
                    selectedIndex = -1;
                }
            }

            lastTouchItem = -1;
            RefreshItem();
        }

        /// <summary>
        /// アイテム変化
        /// </summary>
        /// <param name="beforeItem"></param>
        /// <param name="afterItem"></param>
        public bool ChangeItem(int beforeItem, int afterItem, int itemIndex)
        {
            int index = 0 <= itemIndex ? itemIndex : (lastTouchItem + ItemImages.Length * currentPage);
            if (index < 0) { return false; }
            if (!ItemImages[index].enabled) { return false; }

            itemIdList[index] = afterItem;
            GimmickManager.Instance.SelectedItem = afterItem;
            Sprite itemSprite = ItemManager.Instance.GetItemImage(afterItem);
            if (itemSprite != null)
            {
                ItemImages[index].sprite = itemSprite;
            }

            return true;
        }

        /// <summary>
        /// アイテムバーを並び替え
        /// </summary>
        private void RefreshItem()
        {
            ResetPage();
            
            int start = currentPage * ItemImages.Length;
            int end = start + ItemImages.Length;
            end = Mathf.Clamp(end, 0, itemIdList.Count);

            for (int i = start; i < end; i++)
            {
                // カーソルの位置を更新
                if (GimmickManager.Instance.SelectedItem == itemIdList[i])
                {
                    int imgIndex = i - start;
                    cursor.localPosition = ItemImages[imgIndex].rectTransform.localPosition;
                    selectedIndex = i;
                    cursor.gameObject.SetActive(true);
                }

                SetItemImage(i, ItemManager.Instance.GetItemImage(itemIdList[i]));
            }
        }

        /// <summary>
        /// ページ切替
        /// </summary>
        private void ChangePage(int nextPage)
        {
            ResetPage();

            int start = nextPage * ItemImages.Length;
            int end = start + ItemImages.Length;
            end = Mathf.Clamp(end, 0, itemIdList.Count);

            int imageIndex = 0;
            for (int i = start; i < end; i++)
            {
                // カーソルの位置を更新
                if (GimmickManager.Instance.SelectedItem == itemIdList[i])
                {
                    int imgIndex = i - start;
                    cursor.localPosition = ItemImages[imgIndex].rectTransform.localPosition;
                    cursor.gameObject.SetActive(true);
                }

                SetItemImage(imageIndex, ItemManager.Instance.GetItemImage(itemIdList[i]));

                imageIndex++;
            }

            currentPage = nextPage;
        }

        private void OnTouchOptionButton(Vector2 pressPosition)
        {
            float pressY = Screen.height - pressPosition.y;
            int y = Mathf.FloorToInt(pressY / itemHeight);
            
            if (pressPosition.x < touchStart && y >= 1)
            {
                // ←が押された
                int nextPage = currentPage - 1;
                nextPage = Mathf.Clamp(nextPage, 0, maxPage);
                if (nextPage != currentPage)
                {
                    ChangePage(nextPage);
                }
            }
            else if (pressPosition.x > touchEnd)
            {
                if (y >= 1)
                {
                    int nextPage = currentPage + 1;
                    nextPage = Mathf.Clamp(nextPage, 0, maxPage);
                    if (nextPage != currentPage)
                    {
                        ChangePage(nextPage);
                    }
                }
                else
                {
                    // メニューボタン押された
                    //CheatManager.Instance.AddAllItem();
                    DebugLogger.Log("メニューボタン押された");

                    if (MenuButtonHandle != null)
                    {
                        MenuButtonHandle.Invoke();
                    }
                }
            }
        }

        private void SetItemImage(int index, Sprite img)
        {
            ItemImages[index].sprite = img;
            ItemImages[index].enabled = true;
            ItemImages[index].SetNativeSize();
        }

        #endregion
    }
}