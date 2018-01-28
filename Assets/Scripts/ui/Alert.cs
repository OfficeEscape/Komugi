using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class Alert : MonoBehaviour
    {
        [SerializeField]
        Text AlertText = null;

        public void SetAlertText(string message)
        {
            AlertText.text = message;
        }
    }
}