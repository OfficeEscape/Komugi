﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace Komugi
{

    public class GameManager
    {
        
        /** クラスのユニークなインスタンス */
        private static GameManager mInstance;

        /** 現在表示している画面 */
        public int currentView = 1;

        /** 現在表示している画面の何番目の画像 */
        public int currentAngle = 0;

        /** 各画面のステージデータ */
        public Dictionary<int, StageData> stageDictionary { get; private set; }

        // Private Constructor
        private GameManager()
        {
            stageDictionary = new Dictionary<int, StageData>();
            Debug.Log("Create GameManager instance.");
        }

        public static GameManager Instance
        {
            get
            {

                if (mInstance == null) mInstance = new GameManager();

                return mInstance;
            }
        }


        // JsonDataをデシリアライズ
        public void OpenBinary()
        {
            if (stageDictionary.Count > 0) { return; }

            TextAsset json = Resources.Load("Data/StageData") as TextAsset;
            StageData[] stageList = JsonMapper.ToObject<StageData[]>(json.text);

            foreach (StageData data in stageList)
            {
                stageDictionary.Add(data.id, data);
            }

            Debug.Log("StageData OpenBinary " + Time.time.ToString());
        }
    }
}
