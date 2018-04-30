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

        /// <summary>
        ///  アイテムの情報を更新
        /// </summary>
        /// <param name="itemImage"></param>
        /// <param name="itemName"></param>
        public void UpdateItem(ItemData idata)
        {
            itemData = idata;
            ItemImage.sprite = ItemManager.Instance.GetItemImage(itemData.itemId, 1);
            ItemName.text = itemData.itemName;
            ItemImage.enabled = true;
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

            ItemImage.sprite = itemManager.GetItemImage(itemData.changeItem, 1);
            ItemName.text = itemManager.GetItemName(itemData.changeItem);
            //itemManager.DeleteItem(itemData.itemId);

            //itemManager.AddItem(itemData.changeItem, false);
            itemManager.ChangeItem(itemData.itemId, itemData.changeItem);
            itemData = itemManager.itemDictionary[itemData.changeItem];

            // 変化後のアイテムが自動変化アイテムならもう一度呼び出す
            if (itemData.autoChange == 1)
            {
                ItemImage.raycastTarget = false;
                Invoke("AutoChangeItem", 1.0f);
            }
        }


        /// <summary>
        /// アイテム自動変化
        /// </summary>
        private void AutoChangeItem()
        {
            ItemImage.raycastTarget = true;
            if (itemData.changeItem == 0) { return; }

            UIManager.Instance.OpenAlert("アイテムが変化しました");

            // ほかのアイテムが必要としない前提で変化
            var itemManager = ItemManager.Instance;
            ItemImage.sprite = itemManager.GetItemImage(itemData.changeItem, 1);
            ItemName.text = itemManager.GetItemName(itemData.changeItem);
            //itemManager.DeleteItem(itemData.itemId);

            //itemManager.AddItem(itemData.changeItem, false);
            itemManager.ChangeItem(itemData.itemId, itemData.changeItem);
            itemData = itemManager.itemDictionary[itemData.changeItem];

            GameManager.Instance.PlaySE(AudioConst.SE_ITEM_GET);
        }
    }
}