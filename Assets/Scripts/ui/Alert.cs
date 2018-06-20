using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class Alert : MonoBehaviour
    {
        [SerializeField]
        Text AlertText = null;

        [SerializeField]
        private Button closeButton;

        public Action closeAction;

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                if (closeAction != null) closeAction();
            });
        }

        public void SetAlertText(string message)
        {
            AlertText.text = message;
        }
    }
}