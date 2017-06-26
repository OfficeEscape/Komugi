using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        // アイテムIDの配列
        private int[] itemIdList;

        // 現在持ってるアイテム数
        private int itemNum;

        // 現在のページ数
        private int currentPage = 0;

        // 最後にタッチしたアイテム
        private int lastTouchItem = 0;

        // タッチし始めた時間
        private float touchStartTime = 0f;

        #region =============================== タップ領域計算用フィールド ===============================

        private float touchStart = 0f;

        private float touchEnd = 0f;

        private float itemWidth = 0f;

        private float itemHeight = 0f;

        #endregion

        // Use this for initialization
        void Start ()
        {
            itemNum = 0;

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

            itemIdList = new int[100];
        }

        public void AddItemImage(int itemId)
        {
            if (ItemImages[itemNum].enabled)
            {
                Debug.Log("item " + itemNum + " is Registered");
                return;
            }
            
            Sprite itemSprite = ItemManager.Instance.GetItemImage(itemId);
            if (itemSprite != null)
            {
                ItemImages[itemNum].sprite = itemSprite;
                ItemImages[itemNum].enabled = true;
                ItemImages[itemNum].SetNativeSize();
            }

            itemIdList[itemNum] = itemId;
            itemNum++;
        }

        // 
        public void OnPointerUp(PointerEventData eventData)
        {
            if (GetPressedItemIndex(eventData.pressPosition) != lastTouchItem) { return; }
            if (!ItemImages[lastTouchItem].enabled) { return; }

            float elapsedTime = Time.time - touchStartTime;

            if (elapsedTime >= USE_TIME)
            {
                // アイテムダイアログを出す
                UIManager.Instance.ShowItemGetDailog(itemIdList[lastTouchItem]);
            }
            else
            {
                // アイテムを使う
                Debug.Log("Use Item");
                GimmickManager.Instance.CheckCanOpenGimmick(itemIdList[lastTouchItem]);
            }
            // throw new NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // throw new NotImplementedException();
            lastTouchItem = GetPressedItemIndex(eventData.pressPosition);
            Debug.Log("Now Pressed : " + lastTouchItem);

            touchStartTime = Time.time;
        }

        #region =============================== C# private ===============================

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

            return index;
        }

        #endregion
    }
}