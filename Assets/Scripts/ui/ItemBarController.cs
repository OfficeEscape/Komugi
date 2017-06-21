using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Komugi.UI
{ 
    public class ItemBarController : MonoBehaviour
    {
        [SerializeField]
        // アイテム表示領域
        private Image[] ItemImages;

        // 現在持ってるアイテム数
        private int itemNum;

        // 現在のページ数
        private int currentPage = 0;

        // Use this for initialization
        void Start ()
        {
            itemNum = 0;

            foreach(Image img in ItemImages)
            {
                //var touchup = img.GetComponent<RectTransform
                //PointerPress
                //PointerEvent
            }
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

            itemNum++;
        }

    }
}