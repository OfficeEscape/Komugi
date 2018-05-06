using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class Help : MonoBehaviour
    {
        [SerializeField]
        Button leftArrow;

        [SerializeField]
        Button rightArrow;

        [SerializeField]
        Text message;

        [SerializeField]
        GameObject[] helpContent;

        [SerializeField]
        string[] helpMessage;

        [SerializeField]
        Button ReturnButton = null;

        /** 現在選んでいる画面 */
        int selected = 0;
        
        // Use this for initialization
        void Start()
        {
            leftArrow.onClick.AddListener(() => ChangeHelpContent(-1));
            rightArrow.onClick.AddListener(() => ChangeHelpContent(1));
            ReturnButton.onClick.AddListener(() => Destroy(gameObject));
        }
        
        void ChangeHelpContent(int index)
        {
            selected += index;

            if (selected < 0) { selected = helpContent.Length - 1; }
            if (helpContent.Length <= selected) { selected = 0; }

            for (int i = 0; i < helpContent.Length; i++)
            {
                helpContent[i].SetActive(selected == i);
            }

            if (helpMessage.Length <= selected) { return; }
            message.text = helpMessage[selected];
        }
    }
}