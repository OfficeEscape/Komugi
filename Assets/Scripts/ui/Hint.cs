using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

namespace Komugi.UI
{ 
    public class Hint : MonoBehaviour
    {
        [SerializeField]
        Button AdsButton = null;

        [SerializeField]
        Button ReturnButton = null;

        [SerializeField]
        Transform ScrollContent = null;

        [SerializeField]
        Text HintDetail = null;

        [SerializeField]
        Text CandyNum = null;

        private const string HORIZONTAL_GROUP_PATH = "Prefabs/ui/horizontal_group";

        private const string HINT_BUTTON_PATH = "Prefabs/ui/hint_btn";

        private const string AD_KEY = "rewardedVideo";

        private const int ROW_BUTTON_MAX = 3;

        bool isLoading = false;

        List<HintButton> hintList = new List<HintButton>();

        // Use this for initialization
        void Start ()
        {
            AdsButton.onClick.AddListener(() => { ShowRewardedAd(); });
            ReturnButton.onClick.AddListener(() => 
            {
                if (isLoading) { return; }
                Destroy(gameObject);
            });

            CandyNum.text = DataManager.Instance.UserSaveData.candyNum.ToString();

            StartCoroutine(LoadAsyncPrefab());
        }

        // リソース非同期読み込み
        private IEnumerator LoadAsyncPrefab()
        {
            isLoading = true;

            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync(HORIZONTAL_GROUP_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading " + HORIZONTAL_GROUP_PATH + " progress:" + resReq.progress.ToString());
                yield return 0;
            }

            Debug.Log("Loading " + HORIZONTAL_GROUP_PATH + " End  " + Time.time.ToString());

            //メインメニューを出す
            //GameObject rowObj = Instantiate(resReq.asset as GameObject, gameObject.transform);
            GameObject rowObj = resReq.asset as GameObject;

            // ボタンのプレハブを読み込む
            resReq = Resources.LoadAsync(HINT_BUTTON_PATH);
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                Debug.Log("Loading " + HINT_BUTTON_PATH + " progress:" + resReq.progress.ToString());
                yield return 0;
            }

            GameObject buttonObj = resReq.asset as GameObject;

            int row = Mathf.CeilToInt(GimmickManager.Instance.hintDictionary.Count / (float)ROW_BUTTON_MAX);
            
            for (int i = 0; i < row; i++)
            {
                GameObject rowClone = Instantiate(rowObj, ScrollContent, false);

                for (int j = 0; j < ROW_BUTTON_MAX; j ++)
                {
                    int index = i * ROW_BUTTON_MAX + j + 1;
                    if (!GimmickManager.Instance.hintDictionary.ContainsKey(index)) { continue; }
                    if (GimmickManager.Instance.hintDictionary.Count < index) { break; }
                    HintData data = GimmickManager.Instance.hintDictionary[index];

                    GameObject btnClone = Instantiate(buttonObj, rowClone.transform, false);
                    HintButton script = btnClone.GetComponent<HintButton>();
                    script.SetData(GimmickManager.Instance.hintDictionary[index], 
                        GimmickManager.Instance.GetClearProgress(data.gimmickId) > 0,
                        GimmickManager.Instance.hintPayDictionary[data.hintId],
                        DataManager.Instance.UserSaveData.candyNum);
                    script.clickHandle = OnHintClick;

                    hintList.Add(script);
                }
                
            }
            
            isLoading = false;
        }

        // アメを使う
        private void OnHintClick(HintData data)
        {
            if (!GimmickManager.Instance.hintDictionary.ContainsKey(data.hintId)) { return; }
            
            if (GimmickManager.Instance.hintPayDictionary[data.hintId] || GimmickManager.Instance.clearFlagDictionary[data.gimmickId] > 0)
            {
                HintDetail.text = data.detail;

                return;
            }

            int needNum = GimmickManager.Instance.hintDictionary[data.hintId].candyNum;

            if (DataManager.Instance.UserSaveData.candyNum < needNum) { return; }
            
            DataManager.Instance.AddCandy(needNum * -1);
            GimmickManager.Instance.hintPayDictionary[data.hintId] = true;

            DataManager.Instance.SaveHintData();

            HintDetail.text = data.detail;
            CandyNum.text = DataManager.Instance.UserSaveData.candyNum.ToString();
            RefushHintButton();
        }

        private void RefushHintButton()
        {
            foreach (HintButton hint in hintList)
            {
                hint.SetActive(GimmickManager.Instance.GetClearProgress(hint.Data.gimmickId) > 0,
                    GimmickManager.Instance.hintPayDictionary[hint.Data.hintId], 
                    DataManager.Instance.UserSaveData.candyNum);
            }
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
                    DataManager.Instance.AddCandy(1);
                    CandyNum.text = DataManager.Instance.UserSaveData.candyNum.ToString();
                    RefushHintButton();
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