using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Komugi
{
    public class ItemManager
    {

        /** クラスのユニークなインスタンス */
        private static ItemManager mInstance;

        /** 各画面のステージデータ */
        public Dictionary<int, ItemData> itemDictionary { get; private set; }

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
    }
}
