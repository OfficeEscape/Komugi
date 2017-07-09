using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Komugi
{
    public class ItemManager
    {
        private const string itemPath = "UI/item/";

        /** クラスのユニークなインスタンス */
        private static ItemManager mInstance;

        /** 各画面のステージデータ */
        public Dictionary<int, ItemData> itemDictionary { get; private set; }

        /** 所持アイテムのマップ */
        public Dictionary<int, bool> HasItemList { get; private set; }

        // Private Constructor
        private ItemManager()
        {
            itemDictionary = new Dictionary<int, ItemData>();
            HasItemList = new Dictionary<int, bool>();
            Debug.Log("Create ItemManager instance.");
        }

        public static ItemManager Instance
        {
            get
            {

                if (mInstance == null) mInstance = new ItemManager();

                return mInstance;
            }
        }

        public void Deserialization()
        {
            if (itemDictionary.Count > 0) { return; }

            TextAsset json = Resources.Load("Data/ItemData") as TextAsset;
            ItemData[] itemList = JsonMapper.ToObject<ItemData[]>(json.text);

            foreach (ItemData data in itemList)
            {
                itemDictionary.Add(data.itemId, data);
            }

            Debug.Log("ItemData OpenBinary " + Time.time.ToString());
        }

        // 所持アイテムを追加
        public bool AddItem(int itemId, bool showDialog = true)
        {
            if (HasItemList.ContainsKey(itemId))
            {
                Debug.Log("Item Id " + itemId + " is Dubbed");
                return false;
            }

            HasItemList.Add(itemId, false);

            UIManager.Instance.AddItemToItemBar(itemId, showDialog);
            return true;
        }

        /// <summary>
        /// 所持アイテムを削除
        /// </summary>
        /// <param name="itemId"></param>
        public bool DeleteItem(int itemId)
        {
            if (!HasItemList.ContainsKey(itemId))
            {
                Debug.Log("Item Id " + itemId + " is not Have");
                return false;
            }

            HasItemList.Remove(itemId);
            UIManager.Instance.RemoveItemFromItemBar(itemId);
            return true;
        }

        /// <summary>
        /// アイテムの変化
        /// </summary>
        /// <param name="beforeId"></param>
        /// <param name="afterId"></param>
        /// <returns></returns>
        public bool ChangeItem(int beforeId, int afterId)
        {
            if (!HasItemList.ContainsKey(beforeId) || HasItemList.ContainsKey(afterId))
            {
                Debug.Log("ChangeItem " + beforeId + "To " + afterId + "Failed");
                return false;
            }

            HasItemList.Remove(beforeId);
            HasItemList.Add(afterId, false);

            UIManager.Instance.ChangeItem(beforeId, afterId);
            return true;
        }

        // アイテムの画像を返す
        // size 0 = small 1 = large
        public Sprite GetItemImage(int itemId, int size = 0)
        {
            if (!itemDictionary.ContainsKey(itemId))
            {
                Debug.Log("Item Load error");
            }

            string path = size == 0 ? itemDictionary[itemId].itemIcon : itemDictionary[itemId].itemImage;
            var builder = new System.Text.StringBuilder();
            builder.AppendFormat("{0}{1}", itemPath, path);
            Debug.Log("Item Path = " + builder.ToString());

            Sprite itemSprite = Resources.Load<Sprite>(builder.ToString());

            if (itemSprite == null) { Debug.Log("ItemId :" + itemId +" Road Failed"); }
            return itemSprite;
        }

        // アイテム名を返す
        public string GetItemName(int itemId)
        {
            if (!itemDictionary.ContainsKey(itemId))
            {
                return "Item Load error";
            }
            string name = itemDictionary[itemId].itemName;
            Debug.Log("ItemName : " + name);
            return name;
        }
    }
}
