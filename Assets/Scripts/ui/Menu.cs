using UnityEngine;
using System;

namespace Komugi.UI
{ 
    public class Menu : MonoBehaviour
    {
        public Action<int> CallBack { get; set; }

        /// <summary>
        /// ボタンがクリックされた
        /// </summary>
	    public void OnButtonClick(int index)
        {
            if (CallBack != null)
            {
                CallBack.Invoke(index);
            }
        }
    }
}