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
        private Dictionary<int, int> clearFlagDictionary;

        /** 現在のステージのギミック */
        private IGimmick currentGimmick;

        // Private Constructor
        private GimmickManager()
        {
            gimmickDictionary = new Dictionary<int, GimmickData>();
            clearFlagDictionary = new Dictionary<int, int>();
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
            selectedItem = 0;
        }

        public void Deserialization()
        {
            if (gimmickDictionary.Count > 0) { return; }

            TextAsset json = Resources.Load("Data/GimmickData") as TextAsset;
            GimmickData[] gimmickList = JsonMapper.ToObject<GimmickData[]>(json.text);

            foreach (GimmickData data in gimmickList)
            {
                gimmickDictionary.Add(data.gimmickId, data);
                clearFlagDictionary.Add(data.gimmickId, 0);
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
            if (currentGimmick.Data.clearJump != 0)
            {
                UIManager.Instance.OpenAlert("ゴゴゴゴゴゴゴ。。。", true, () =>
                {
                    GameMain.Instance.JumpView(currentGimmick.Data.clearJump);
                });
                
            }
        }
    }
}