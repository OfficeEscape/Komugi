using Komugi.Community;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi
{
    public class Ending : MonoBehaviour
    {

        [SerializeField]
        Text resultText;

        [SerializeField]
        Text buttonText;

        [SerializeField]
        Button shareButton;

        private SNSShare share;

        private const int LAST_GIMMICK = 14;

        private const string SHARE_IMAGE_FILE = "share.png";

        private readonly string[] RESULT = {"エレベーターで1階に降り\n外に出ることができた。\n\n暗号を社長の名字にするなんて\n不用心だなぁ…\n明日変えておこう。\n\n終電にも間に合ったし、\n無事、家に帰れるぞ！",
                                "エレベーターのドアが開いたが\nボタンを押しても反応がない…\n\n非常ボタンを押したら\n警備員さんが来てくれて\n外に出ることができた。\n\n終電には間に合わなかったけど、\nとりあえず脱出できて良かった…。"};

        private readonly string[] RESULT_BUTTON = {"Title",
                                "タイトルに戻る"};

        private readonly string[] TWEET = {"トゥルーエンド",
                                "ノーマルエンド"};

        private readonly string TWEET_FORMAT = "実在するオフィスをモデルとした脱出ゲーム\n『スペースラボからの脱出』{0}をクリアしました！\n▼ダウンロードはこちら\nIOS\nhttps://apple.co/2JHneK7　\nAndroid\nhttp://bit.ly/2y2aysI\n#脱出ゲーム";

        private string tweetText = string.Empty;

        // Use this for initialization
        void Start()
        {
            int index = GimmickManager.Instance.GetClearProgress(LAST_GIMMICK) - 1;
            index = Mathf.Clamp(index, 0, RESULT.Length - 1);

            resultText.text = RESULT[index];
            buttonText.text = RESULT_BUTTON[index];
            tweetText = string.Format(TWEET_FORMAT, TWEET[index]);

            share = GetComponent<SNSShare>();

            shareButton.onClick.AddListener(() => Share());

            SoundManger.Instance.PlayBgm(AudioConst.BGM_ENDING);
        }
      
        public void Share()
        {
            share.Share(tweetText, "", SHARE_IMAGE_FILE);
        }
    }
}