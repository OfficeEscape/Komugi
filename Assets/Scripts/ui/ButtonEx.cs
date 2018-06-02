using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class ButtonEx : Button
    {
        public string clickSe = string.Empty;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            onClick.AddListener(() => SoundManger.Instance.PlaySe(clickSe));
        }
        
    }
}