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

	    // Use this for initialization
	    void Start ()
        {
            AdsButton.onClick.AddListener(() => { ShowRewardedAd(); });
            ReturnButton.onClick.AddListener(() => { Destroy(gameObject); });

        }
	
	    private void ShowRewardedAd()
        {
            Advertisement.Show();
        }
    }
}