﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
            Debug.Log("currentMargin" + currentMargin);
            touchStart = currentMargin;
            touchEnd = Screen.width - currentMargin;
            float wd = Screen.width - (currentMargin * 2f);
            itemWidth = wd / (float)COLUMN;
            Debug.Log("itemWidth" + itemWidth);
            itemHeight = 384f * heightRatio / (float)ROW;
            Debug.Log("itemHeight" + itemHeight);

            itemIdList = new List<int>();
        }


        /// <summary>
        /// アイテムバーに画像を追加
        /// </summary>
        /// <param name="itemId"></param>
        public void AddItemImage(int itemId)
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
                Debug.Log("item " + itemNum + " is Registered");
                return;
            }
            
            Sprite itemSprite = ItemManager.Instance.GetItemImage(itemId);
            if (itemSprite != null)
            {
                SetItemImage(itemNum, itemSprite);
            }

            itemIdList.Add(itemId);
            maxPage = itemIdList.Count / ItemImages.Length + 1;
        }

        // 
        public void OnPointerUp(PointerEventData eventData)
        {
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
                UIManager.Instance.ShowItemGetDailog(itemIdList[itemIndex]);
            }
            else
            {
                // アイテムを使うモード
                Debug.Log("Use Item");
                selectedIndex = itemIndex;
                GimmickManager.Instance.SelectedItem = itemIdList[itemIndex];
                cursor.name = itemIndex.ToString();
                cursor.localPosition = ItemImages[lastTouchItem].rectTransform.localPosition;
                if (!cursor.gameObject.activeSelf) { cursor.gameObject.SetActive(true); }

            }
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // throw new NotImplementedException();
            lastTouchItem = GetPressedItemIndex(eventData.pressPosition);
            Debug.Log("Now Pressed : " + lastTouchItem);
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
            if (lastTouchItem < 0) { Debug.Log("lastTouchItem < 0"); return; }
            if (itemIdList[lastTouchItem + currentPage * ItemImages.Length] == itemId)
            {
                cursor.gameObject.SetActive(false);
                GimmickManager.Instance.SelectedItem = 0;
                selectedIndex = -1;
            }
            
            itemIdList.Remove(itemId);
            lastTouchItem = -1;
            RefreshItem();
        }

        /// <summary>
        /// アイテム変化
        /// </summary>
        /// <param name="beforeItem"></param>
        /// <param name="afterItem"></param>
        public bool ChangeItem(int beforeItem, int afterItem)
        {
            if (lastTouchItem < 0) { Debug.Log("lastTouchItem < 0"); return false; }
            if (itemIdList[lastTouchItem] != beforeItem) { Debug.Log("Change Item Failed"); return false; }

            itemIdList[lastTouchItem + ItemImages.Length * currentPage] = afterItem;
            GimmickManager.Instance.SelectedItem = afterItem;
            Sprite itemSprite = ItemManager.Instance.GetItemImage(afterItem);
            if (itemSprite != null)
            {
                ItemImages[lastTouchItem].sprite = itemSprite;
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
                Debug.Log("←が押された");
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
                    // →押された
                    Debug.Log("→が押された");
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
                    CheatManager.Instance.AddAllItem();
                    Debug.Log("メニューボタン押された");
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