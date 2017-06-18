using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Komugi
{
    public class ItemManager
    {
        private const string itemPath = "Office/";

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

        public void OpenBinary()
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
        public void AddItem(int itemId)
        {
            if (HasItemList.ContainsKey(itemId))
            {
                Debug.Log("Item Id " + itemId + " is Dubbed");
            }

            HasItemList.Add(itemId, false);
        }

        // アイテムの画像を返す
        public Sprite GetItemImage(int itemId)
        {
            if (!itemDictionary.ContainsKey(itemId))
            {
                Debug.Log("Item Load error");
            }

            string path = itemDictionary[itemId].itemImage;
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
