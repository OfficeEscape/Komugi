using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Collections;

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

        public IEnumerator Deserialization()
        {
            if (itemDictionary.Count > 0) { yield break; }

            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync("Data/ItemData");
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading Dialog progress:" + resReq.progress.ToString());
                yield return 0;
            }

            TextAsset json = resReq.asset as TextAsset;
            ItemData[] itemList = JsonMapper.ToObject<ItemData[]>(json.text);

            foreach (ItemData data in itemList)
            {
                itemDictionary.Add(data.itemId, data);
            }
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

            if (itemDictionary[itemId].changeItem > 0) { return false; }

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
        public bool CheckItem(int itemid, bool used)
        {
            return HasItemList.ContainsKey(itemid) && (HasItemList[itemid] == used);
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

        public void Load()
        {
            HasItemList = DataManager.Instance.LoadItemSaveData();
        }
    }
}
