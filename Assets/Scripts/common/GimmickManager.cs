using Komugi.Gimmick;
using LitJson;
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
                Debug.Log("selectedItem : " + selectedItem);
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
            Debug.Log("Create GimmickManager instance.");
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

        public void Deserialization()
        {
            TextAsset json = Resources.Load("Data/GimmickData") as TextAsset;
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
            
            Debug.Log("GimmickData OpenBinary " + Time.time.ToString());
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

            if (progress < currentGimmick.Data.clearStep) { return; }

            UIManager.Instance.OpenAlert(currentGimmick.Data.clearMessage, false, () =>
            {
                if (currentGimmick.Data.clearJump != 0)
                {
                    GameMain.Instance.JumpView(currentGimmick.Data.clearJump);
                }
            });

            if (currentGimmick.Data.clearSe.Length > 0)
            {
                GameManager.Instance.PlaySE(currentGimmick.Data.clearSe);
            }
        }

        public int GetClearProgress(int gimmickId = 0)
        {
            if (gimmickId == 0) { gimmickId = gimmickDictionary.Count; }

            return clearFlagDictionary.ContainsKey(gimmickId) ? clearFlagDictionary[gimmickId] : 0;
        }

        public void Load()
        {
            clearFlagDictionary = DataManager.Instance.LoadGimmickSaveData();
            hintPayDictionary = DataManager.Instance.LoadHintSaveData();
        }
    }
}