using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{ 
    public class ItemDialog : MonoBehaviour
    {
        [SerializeField]
        Button CloseButton = null;

        [SerializeField]
        Image ItemImage = null;

        [SerializeField]
        Text ItemName = null;

        private ItemData itemData;

        private int originItemId = 0;

        private int itemBarIndex = 0;

        public static int ItemId = 0;

        public System.Action CloseCallBack = null;
        
        private void Awake()
        {
            CloseButton.onClick.AddListener(() =>
            {
                Destroy(gameObject);
                GameManager.Instance.PlaySE(AudioConst.SE_BUTTON);
                if (CloseCallBack != null) { CloseCallBack.Invoke(); }
            });
            ItemImage.GetComponent<Button>().onClick.AddListener(() => ChangeItem());
        }

        private void SetItem(int itemid, string itemname)
        {
            ItemImage.sprite = ItemManager.Instance.GetItemImage(itemid, 1);
            ItemName.text = itemname;
            ItemImage.SetNativeSize();
        }

        /// <summary>
        ///  アイテムの情報を更新
        /// </summary>
        /// <param name="itemImage"></param>
        /// <param name="itemName"></param>
        public void UpdateItem(ItemData idata, int itemIndex)
        {
            itemData = idata;
            SetItem(itemData.itemId, itemData.itemName);
            ItemImage.enabled = true;
            itemBarIndex = itemIndex;
            originItemId = itemData.itemId;
            ItemId = itemData.itemId;
        }

        public void UpdateItem(int itemIndex)
        {
            itemBarIndex = itemIndex;
        }

        /// <summary>
        /// アイテムを変化させる
        /// </summary>
        private void ChangeItem()
        {
            if (itemData.changeItem == 0) { return; }

            // ほかのアイテムが必要な場合
            int triggerItem = itemData.triggerItem;
            var itemManager = ItemManager.Instance;
            if (triggerItem != 0)
            {
                if (triggerItem > 0)
                {
                    itemManager.AddItem(triggerItem, false);
                }
                else
                {
                    if (!itemManager.DeleteItem( Mathf.Abs(triggerItem)) ) { return; }
                }
            }
            
            SetItem(itemData.changeItem, itemManager.GetItemName(itemData.changeItem));

            itemData = itemManager.itemDictionary[itemData.changeItem];
            
            // 変化後のアイテムが自動変化アイテムならもう一度呼び出す
            if (itemData.autoChange == 1)
            {
                UIManager.Instance.SetItemBarTouchEnable(false);
                CloseButton.enabled = false;
                ItemImage.raycastTarget = false;
                Invoke("AutoChangeItem", 1.0f);
            }
        }


        /// <summary>
        /// アイテム自動変化
        /// </summary>
        private void AutoChangeItem()
        {
            if (itemData.changeItem == 0) { return; }

            UIManager.Instance.OpenAlert("アイテムが変化しました");

            // ほかのアイテムが必要としない前提で変化
            var itemManager = ItemManager.Instance;
            //SetItem(itemData.changeItem, itemManager.GetItemName(itemData.changeItem));

            itemManager.ChangeItem(originItemId, itemData.changeItem, itemBarIndex);
            //itemData = itemManager.itemDictionary[itemData.changeItem];

            UpdateItem(itemManager.itemDictionary[itemData.changeItem], itemBarIndex);

            ItemImage.raycastTarget = true;
            UIManager.Instance.SetItemBarTouchEnable(true);

            GameManager.Instance.PlaySE(AudioConst.SE_ITEM_GET);

            CloseButton.enabled = true;
        }
    }
}