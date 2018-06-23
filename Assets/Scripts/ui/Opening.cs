using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Komugi
{
    public class Opening : MonoBehaviour
    {
        [SerializeField]
        private Transform ScenarioContent = null;

        [SerializeField]
        private Color TextColor;

        private const float SPEED = 0.03f;

        private readonly string[] SYNOPSIS = {"ここは御徒町にあるオフィス、",
                                "「Space Lab」",
                                "目が覚めると誰もいない",
                                "オフィスに閉じ込められていた。",
                                "もうすぐ終電の時間だ。",
                                "早くここから脱出しなければ"};

        private Image _bg;
        

        // Use this for initialization
        void Start()
        {
            _bg = GetComponent<Image>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_bg.color.a < 1f)
            {
                float alpha = _bg.color.a + SPEED;
                _bg.color = new Color(_bg.color.r, _bg.color.g, _bg.color.b, alpha);
            }
            else
            {
                enabled = false;
                UI.ScenarioPlayer.PlayScenario(SYNOPSIS, ScenarioContent, TextColor, () => SceneManager.LoadScene("GameScene"));
            }
        }
    }
}
