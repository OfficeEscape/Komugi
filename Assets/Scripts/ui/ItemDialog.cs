using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{ 
    public class ItemDialog : MonoBehaviour
    {
        [SerializeField]
        Button CloseButton;

        [SerializeField]
        Image ItemImage;

        [SerializeField]
        Text ItemName;

        private void Awake()
        {
            CloseButton.onClick.AddListener(
                () => Destroy(gameObject));
        }

        // アイテムの情報を更新
        public void UpdateItem(Sprite itemImage, string itemName)
        {
            ItemImage.sprite = itemImage;
            ItemName.text = itemName;
            ItemImage.enabled = true;
        }
    }
}