using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Komugi.Gimmick;

namespace Komugi
{
    public class GimmickManager
    {
        /** クラスのユニークなインスタンス */
        private static GimmickManager mInstance;

        /** 各ギミックデータ */
        public Dictionary<int, GimmickData> gimmickDictionary { get; private set; }

        // Private Constructor
        private GimmickManager()
        {
            gimmickDictionary = new Dictionary<int, GimmickData>();
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

        public void Deserialization()
        {
            if (gimmickDictionary.Count > 0) { return; }

            TextAsset json = Resources.Load("Data/GimmickData") as TextAsset;
            GimmickData[] gimmickList = JsonMapper.ToObject<GimmickData[]>(json.text);

            foreach (GimmickData data in gimmickList)
            {
                gimmickDictionary.Add(data.gimmickId, data);
            }

            Debug.Log("GimmickData OpenBinary " + Time.time.ToString());
        }

        public void SetupGimmick(ref GameObject stageObject, int gimmickId)
        {
            if (!gimmickDictionary.ContainsKey(gimmickId)) { return; }

            IGimmick gimmick = stageObject.GetComponentInChildren<IGimmick>();
            gimmick.ClearItem = gimmickDictionary[gimmickId].gimmickAnswer;
            gimmick.ClearFlag = false;
        }
    }
}