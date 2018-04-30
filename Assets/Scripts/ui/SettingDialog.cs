using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{ 
    public class SettingDialog : MonoBehaviour
    {
        [SerializeField]
        Button BgmButton;

        [SerializeField]
        Button SeButton;

        [SerializeField]
        Button BatuButton;

        [SerializeField]
        GameObject[] BgmSwitch;

        [SerializeField]
        GameObject[] SeSwitch;

        private int bgmOff = 0;

        private int seOff = 0;

        private void Start()
        {
            if (SoundManger.Instance != null)
            {
                bgmOff = SoundManger.Instance.BgmOff;
                seOff = SoundManger.Instance.SeOff;
            }

            SwitchingBGM();
            SwitchingSE();

            BgmButton.onClick.AddListener(() =>
            {
                bgmOff = bgmOff == 0 ? 1 : 0;
                SwitchingBGM();
                if (SoundManger.Instance != null) { SoundManger.Instance.BgmOff = bgmOff; }
            });

            SeButton.onClick.AddListener(() =>
            {
                seOff = seOff == 0 ? 1 : 0;
                SwitchingSE();
                if (SoundManger.Instance != null) { SoundManger.Instance.SeOff = seOff; }
            });

            BatuButton.onClick.AddListener(() => { Destroy(gameObject); });
        }

        private void SwitchingBGM()
        {
            if (2 <= BgmSwitch.Length)
            {
                BgmSwitch[0].SetActive(bgmOff == 0);
                BgmSwitch[1].SetActive(bgmOff == 1);
            }
        }

        private void SwitchingSE()
        {
            if (2 <= SeSwitch.Length)
            {
                SeSwitch[0].SetActive(seOff == 0);
                SeSwitch[1].SetActive(seOff == 1);
            }
        }
    }
}