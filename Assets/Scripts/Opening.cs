using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour {

    private string[] SYNOPSIS = {"ここは御徒町にあるオフィス、",
                                "「Space Lab」",
                                "目が覚めると誰もいない",
                                "オフィスに閉じ込められていた。",
                                "もうすぐ終電の時間だ。",
                                "早くここから脱出しなければ"};

    private Image _bg;

    private Text _synopsis;

    private int _count = 0;
    private int _line = 0;
    private string completeText = "";

	// Use this for initialization
	void Start () {
        _bg = GetComponent<Image>();
        _synopsis = GetComponentInChildren<Text>();
        _bg.color = new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f);
        _count = 0;
        _line = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_bg.color.a < 1f)
        {
            float alpha = Mathf.Lerp(0f, 100f, Time.time) / 100f;
            _bg.color = new Color(_bg.color.r, _bg.color.g, _bg.color.b, alpha);
        }
        else
        {
            if (_line < SYNOPSIS.Length)
            {
                string nowText = SYNOPSIS[_line];
                _count++;
                int index = _count / 6;

                if (index <= nowText.Length)
                {
                    _synopsis.text = completeText + nowText.Substring(0, index);
                }
                else
                {
                    _synopsis.text = _synopsis.text + "\n";
                    _line++;
                    _count = 0;
                    completeText = _synopsis.text;
                }
            }
            else
            {
                this.enabled = false;
                SceneManager.LoadScene("GameScene");
            }
        }
	}
}
