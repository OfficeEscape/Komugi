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
        public int selectedItem { get; set; }

        /** 各ギミックのクリア状況 */
        private Dictionary<int, bool> clearFlagDictionart;

        /** 現在のステージのギミック */
        private IGimmick currentGimmick;

        // Private Constructor
        private GimmickManager()
        {
            gimmickDictionary = new Dictionary<int, GimmickData>();
            clearFlagDictionart = new Dictionary<int, bool>();
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
                clearFlagDictionart.Add(data.gimmickId, false);
            }

            Debug.Log("GimmickData OpenBinary " + Time.time.ToString());
        }

        public void SetupGimmick(ref GameObject stageObject, int gimmickId)
        {
            if (!gimmickDictionary.ContainsKey(gimmickId)) { return; }

            currentGimmick = stageObject.GetComponentInChildren<IGimmick>();
            currentGimmick.Data = gimmickDictionary[gimmickId];
            currentGimmick.ClearFlag = clearFlagDictionart[gimmickId];
            currentGimmick.OpenAction = () => { SetGimmickClearFlg(); };
        }

        // ギミックを解除できるかを確認
        public void SetGimmickClearFlg()
        {
            clearFlagDictionart[currentGimmick.Data.gimmickId] = true;
        }
    }
}