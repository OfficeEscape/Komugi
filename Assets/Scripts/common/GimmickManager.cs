using Komugi.Gimmick;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Komugi
{
    public class GimmickManager
    {
        /** クラスのユニークなインスタンス */
        private static GimmickManager mInstance;

        /** 各ギミックデータ */
        public Dictionary<int, GimmickData> gimmickDictionary { get; private set; }

        /** ギミックのヒントデータ */
        public Dictionary<int, HintData> hintDictionary { get; private set; }

        /** 今選んだアイテム */
        private int selectedItem;

        public int SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;
                DebugLogger.Log("selectedItem : " + selectedItem);
            }
        }

        /** 各ギミックのクリア状況 */
        public Dictionary<int, int> clearFlagDictionary { get; private set; }

        /** ヒントの支払い状況 */
        public Dictionary<int, bool> hintPayDictionary { get; set; }

        /** 現在のステージのギミック */
        private IGimmick currentGimmick;

        // Private Constructor
        private GimmickManager()
        {
            gimmickDictionary = new Dictionary<int, GimmickData>();
            hintDictionary = new Dictionary<int, HintData>();
        }

        public static GimmickManager Instance
        {
            get
            {
                if (mInstance == null) mInstance = new GimmickManager();

                return mInstance;
            }
        }

        public void ResetGimmick()
        {
            currentGimmick = null;
        }

        public IEnumerator Deserialization()
        {
            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync("Data/GimmickData");
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                yield return 0;
            }

            TextAsset json = resReq.asset as TextAsset;
            GimmickData[] gimmickList = JsonMapper.ToObject<GimmickData[]>(json.text);

            foreach (GimmickData data in gimmickList)
            {
                if (!gimmickDictionary.ContainsKey(data.gimmickId))
                {
                    gimmickDictionary.Add(data.gimmickId, data);
                }

                if (clearFlagDictionary.ContainsKey(data.gimmickId)) { continue; }
                clearFlagDictionary.Add(data.gimmickId, 0);
            }

            json = Resources.Load("Data/HintData") as TextAsset;
            HintData[] hintList = JsonMapper.ToObject<HintData[]>(json.text);

            foreach (HintData data in hintList)
            {
                if (!hintDictionary.ContainsKey(data.hintId))
                {
                    hintDictionary.Add(data.hintId, data);
                }

                if (hintPayDictionary.ContainsKey(data.hintId)) { continue; }
                hintPayDictionary.Add(data.hintId, false);
            }
        }

        public void SetupGimmick(ref GameObject stageObject, int gimmickId)
        {
            if (!gimmickDictionary.ContainsKey(gimmickId)) { return; }

            currentGimmick = stageObject.GetComponentInChildren<IGimmick>();
            currentGimmick.Data = gimmickDictionary[gimmickId];
            currentGimmick.ClearFlag = clearFlagDictionary[gimmickId];
            currentGimmick.OpenAction = (progress) => { SetGimmickClearFlg(progress); };
        }

        // ギミックのクリア状況をセット
        public void SetGimmickClearFlg(int progress = 1)
        {
            clearFlagDictionary[currentGimmick.Data.gimmickId] = progress;

            // データ保存
            DataManager.Instance.SaveData();
            DataManager.Instance.SaveUserData();

            if (progress < currentGimmick.Data.clearStep) { return; }

            if (currentGimmick.Data.clearSe.Length > 0)
            {
                GameManager.Instance.PlaySE(currentGimmick.Data.clearSe);
            }

            UIManager.Instance.OpenAlert(currentGimmick.Data.clearMessage, false, () =>
            {
                if (currentGimmick.Data.clearJump != 0)
                {
                    GameMain.Instance.JumpView(currentGimmick.Data.clearJump);
                }
            });
        }

        public int GetClearProgress(int gimmickId = 0)
        {
            if (gimmickId == 0) { gimmickId = gimmickDictionary.Count; }

            return clearFlagDictionary.ContainsKey(gimmickId) ? clearFlagDictionary[gimmickId] : 0;
        }

        public void Load()
        {
            clearFlagDictionary = DataManager.Instance.LoadGimmickSaveData();
            if (clearFlagDictionary.ContainsKey(GameDefine.LAST_GIMMICK) && clearFlagDictionary[GameDefine.LAST_GIMMICK] > 0)
            {
                clearFlagDictionary[GameDefine.LAST_GIMMICK] = 0;
            }

            hintPayDictionary = DataManager.Instance.LoadHintSaveData();
        }
    }
}