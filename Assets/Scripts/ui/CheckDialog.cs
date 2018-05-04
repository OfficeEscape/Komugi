using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{ 
    public class CheckDialog : MonoBehaviour
    {
        public enum ResultType
        {
            OK,
            CANCEL,
        }
        
        [SerializeField]
        Text checkText;

        [SerializeField]
        Button okButton;

        [SerializeField]
        Button cancelButton;

        private Action<int> callBack;

        public Action<int> CallBack
        {
            get { return callBack; }

            set { callBack = value; }
        }


        public void SetData(string message, bool okOnly = false)
        {
            checkText.text = message;
            if (okOnly) { cancelButton.gameObject.SetActive(false); }

            okButton.onClick.AddListener(() => OnButtonClick((int)ResultType.OK));
            cancelButton.onClick.AddListener(() => OnButtonClick((int)ResultType.CANCEL));
        }

        private void OnButtonClick(int res)
        {
            Destroy(gameObject);
            if (callBack != null)
            {
                callBack(res);
            }
        }
    }
}