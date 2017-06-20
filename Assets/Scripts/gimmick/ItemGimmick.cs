using UnityEngine;

namespace Komugi.Gimmick
{
    public class ItemGimmick : MonoBehaviour, IGimmick
    {
        [SerializeField]
        GameObject closeObject;

        [SerializeField]
        GameObject openObject;

        public bool ClearFlag { get; set; }

        public int ClearItem { get; set; }

        public bool CheckClearConditions(int itemId)
        {
            return ClearItem == itemId;
        }

        public void RescissionGimmick()
        {
            closeObject.SetActive(false);
            openObject.SetActive(true);
        }
    }
}