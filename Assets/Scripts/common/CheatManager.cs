using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Komugi
{
    public class CheatManager : SingletonMonoBehaviour<CheatManager>
    {
        
        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();

            DataManager.Instance.SaveDataReset();
        }

        public void AddAllItem()
        {
            foreach (ItemData item in ItemManager.Instance.itemDictionary.Values)
            {
                ItemManager.Instance.AddItem(item.itemId, false);
            }
        }
        

    }
}