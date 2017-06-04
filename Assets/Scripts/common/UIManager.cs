using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace Komugi
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [SerializeField]
        // アイテム表示領域
        private Image[] ItemImages;

        // 現在持ってるアイテム数
        private int itemNum;

        private string itemPath = "Office/";

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
        }

        // Use this for initialization
        void Start()
        {
            itemNum = 0;
        }

        public void AddItemToItemBar(int itemId)
        {
            if (ItemImages[itemNum].enabled)
            {
                Debug.Log("item " + itemNum + " is Registered");
                return;
            }

            string path = ItemManager.Instance.itemDictionary[itemId].itemImage;
            var builder = new StringBuilder();
            builder.AppendFormat("{0}{1}", itemPath, path);
            Debug.Log("Item Path = " + builder.ToString());

            Sprite itemSprite = Resources.Load<Sprite>(builder.ToString());
            ItemImages[itemNum].sprite = itemSprite;
            ItemImages[itemNum].enabled = true;
            ItemImages[itemNum].SetNativeSize();
        }
    }
}