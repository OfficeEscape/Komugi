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
            HasItemList = DataManager.Instance.LoadItemSaveData();
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

        public void AddItemSaveData()
        {
            foreach(KeyValuePair<int, bool> kvp in HasItemList)
            {
                if (!kvp.Value)
                {
                    UIManager.Instance.AddItemToItemBar(kvp.Key, false);
                }
            }
        }
        
        // 所持アイテムを追加
        public bool AddItem(int itemId, bool showDialog = true)
        {
            if (HasItemList.ContainsKey(itemId))
            {
                HasItemList[itemId] = false;
            }
            else
            {
                HasItemList.Add(itemId, false);
            }
            

            UIManager.Instance.AddItemToItemBar(itemId, showDialog);

            // セーブデータ
            DataManager.Instance.SaveData();

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

            if (HasItemList[itemId])
            {
                Debug.Log("Item Id " + itemId + " is used");
                return false;
            }

            HasItemList[itemId] = true;
            UIManager.Instance.RemoveItemFromItemBar(itemId);

            // セーブデータ
            DataManager.Instance.SaveData();
            return true;
        }

        /// <summary>
        /// アイテムの変化
        /// </summary>
        /// <param name="beforeId"></param>
        /// <param name="afterId"></param>
        /// <returns></returns>
        public bool ChangeItem(int beforeId, int afterId, int itemIndex)
        {
            HasItemList[beforeId] = true;

            if (HasItemList.ContainsKey(afterId))
            {
                HasItemList[afterId] = false;
            }
            else
            {
                HasItemList.Add(afterId, false);
            }

            UIManager.Instance.ChangeItem(beforeId, afterId, itemIndex);

            // セーブデータ
            DataManager.Instance.SaveData();
            
            return true;
        }

        /// <summary>
        /// アイテム進化
        /// </summary>
        /// <param name="afterId"></param>
        /// <returns></returns>
        public bool ItemUpgrade(int afterId)
        {
            if (!itemDictionary.ContainsKey(afterId)) { return false; }

            int beforeId = itemDictionary[afterId].triggerItem;
            if (beforeId == 0) { return false; }

            if (!HasItemList.ContainsKey(beforeId)) { return false; }
            HasItemList[beforeId] = true;
            //if (GimmickManager.Instance.SelectedItem != beforeId) { return false; }

            if (HasItemList.ContainsKey(afterId)) { HasItemList[afterId] = false; }
            else { HasItemList.Add(afterId, false); }

            bool ret = UIManager.Instance.ChangeItem(beforeId, afterId);

            if (ret)
            {
                UIManager.Instance.OpenAlert(string.Format("{0} が {1} になりました", itemDictionary[beforeId].itemName, itemDictionary[afterId].itemName), true);
            }

            // セーブデータ
            DataManager.Instance.SaveData();

            return ret;
        }

        /// <summary>
        /// アイテムの所持をチェック
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public bool CheckItem(int itemid)
        {
            return HasItemList.ContainsKey(itemid) && !HasItemList[itemid];
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
