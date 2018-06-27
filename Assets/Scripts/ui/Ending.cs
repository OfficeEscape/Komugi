using Komugi.Community;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi
{
    public class Ending : MonoBehaviour
    {
        [SerializeField]
        Text buttonText;

        [SerializeField]
        Button shareButton;

        [SerializeField]
        Image MaskImage = null;

        [SerializeField]
        Transform ScenarioContent = null;

        [SerializeField]
        Image ResultImage = null;

        [SerializeField]
        Sprite[] ResultSprites;

        [SerializeField]
        Color TextColor;

        private SNSShare share;

        private const float SPEED = 0.03f;

        private const string SHARE_IMAGE_FILE = "share.png";

        private readonly string[] RESULT_BUTTON = {"Title",
                                "タイトルに戻る"};

        private readonly string[] TWEET = {"トゥルーエンド",
                                "ノーマルエンド"};

        private readonly string[][] END_SCENARIO= new string[2][]{
            new string[]
            {
                "エレベーターで1階に降り",
                "外に出ることができた。",
                " ",
                "暗号を社長の名字にするなんて",
                "不用心だなぁ…",
                "明日変えておこう。",
                " ",
                "終電にも間に合ったし、",
                "無事、家に帰れるぞ！"
            },
            new string[]
            {
                "エレベーターのドアが開いたが",
                "ボタンを押しても反応がない…",
                " ",
                "非常ボタンを押したら",
                "警備員さんが来てくれて",
                "外に出ることができた。",
                " ",
                "終電には間に合わなかったけど、",
                "とりあえず脱出できて良かった…。"
            }
        };

        private readonly string TWEET_FORMAT = "実在するオフィスをモデルとした脱出ゲーム\n『スペースラボからの脱出』{0}をクリアしました！\n▼ダウンロードはこちら\nIOS\nhttps://apple.co/2JHneK7　\nAndroid\nhttp://bit.ly/2y2aysI\n#脱出ゲーム";

        private string tweetText = string.Empty;

        // Use this for initialization
        void Start()
        {
            int index = GimmickManager.Instance.GetClearProgress(GameDefine.LAST_GIMMICK) - 1;
            index = Mathf.Clamp(index, 0, END_SCENARIO.Length - 1);
            //int index = 0;

            buttonText.text = RESULT_BUTTON[index];
            tweetText = string.Format(TWEET_FORMAT, TWEET[index]);
            ResultImage.sprite = ResultSprites[index];

            share = GetComponent<SNSShare>();

            shareButton.onClick.AddListener(() => Share());

            SoundManger.Instance.PlayBgm(AudioConst.BGM_ENDING);

            UI.ScenarioPlayer.PlayScenario(END_SCENARIO[index], ScenarioContent, TextColor, () => { enabled = true; });

            enabled = false;
        }

        private void FixedUpdate()
        {
            if (MaskImage.color.a > 0.0f)
            {
                float alpha = MaskImage.color.a - SPEED;
                MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, alpha);
            }
            else
            {
                MaskImage.raycastTarget = false;
                enabled = false;
            }
        }


        public void Share()
        {
            share.Share(tweetText, "", SHARE_IMAGE_FILE);
        }
    }
}