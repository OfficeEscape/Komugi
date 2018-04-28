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

        private readonly string[] RESULT = {"「Space Lab」で\n君も一緒に働かないか？",
                                "なんとか終電に間に合った…\n良かった！"};

        private readonly string[] RESULT_BUTTON = {"Title",
                                "タイトルに戻る"};

        // Use this for initialization
        void Start()
        {
            int index = GimmickManager.Instance.GetClearProgress() - 1;
            index = Mathf.Clamp(index, 0, RESULT.Length - 1);

            resultText.text = RESULT[index];
            buttonText.text = RESULT_BUTTON[index];
        }
      
    }
}