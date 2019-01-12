using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class Alert : MonoBehaviour
    {
        [SerializeField]
        private Text AlertText = null;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private RectTransform textBgTransform;

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

            if (UIManager.Instance.NewAspect)
            {
                textBgTransform.anchorMin = new Vector2(0f, 0.75f);
                textBgTransform.anchorMax = new Vector2(1f, 0.75f);
            }
        }
    }
}