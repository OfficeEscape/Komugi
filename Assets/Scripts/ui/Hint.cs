using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace Komugi.UI
{ 
    public class Hint : MonoBehaviour
    {
        [SerializeField]
        Button AdsButton;

        [SerializeField]
        Button ReturnButton;

        private const string AD_KEY = "rewardedVideo";

        // Use this for initialization
        void Start ()
        {
            AdsButton.onClick.AddListener(() => { ShowRewardedAd(); });
            ReturnButton.onClick.AddListener(() => { Destroy(gameObject); });

        }
	
	    private void ShowRewardedAd()
        {
            if (Advertisement.IsReady(AD_KEY))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show(AD_KEY, options);
            }
            
        }

        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("アメ恵んでやろう.");
                    
                    break;
                case ShowResult.Skipped:
                    Debug.Log("スキップしやがったなコノヤロー.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("失敗したみたい.");
                    break;
            }
        }
    }
}