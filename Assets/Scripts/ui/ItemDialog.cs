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

        private void Awake()
        {
            CloseButton.onClick.AddListener(() => Destroy(gameObject));

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
            itemManager.DeleteItem(itemData.itemId);

            itemManager.AddItem(itemData.changeItem, false);
            itemData = itemManager.itemDictionary[itemData.changeItem];
        }
    }
}