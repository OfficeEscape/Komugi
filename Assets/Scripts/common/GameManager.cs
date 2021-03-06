﻿using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Collections;

namespace Komugi
{
    public class GameManager
    {
        /** クラスのユニークなインスタンス */
        private static GameManager mInstance;

        /** 現在表示している画面 */
        //public int currentView = 40;
        public int currentView = 1;

        /** 現在表示している画面の何番目の画像 */
        public int currentAngle = 0;

        /** 各画面のステージデータ */
        public Dictionary<int, StageData> stageDictionary { get; private set; }

        // Private Constructor
        private GameManager()
        {
            stageDictionary = new Dictionary<int, StageData>();
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
        public IEnumerator Deserialization()
        {
            if (stageDictionary.Count > 0) { yield break; }

            // リソースの非同期読込開始
            ResourceRequest resReq = Resources.LoadAsync("Data/StageData");
            // 終わるまで待つ
            while (resReq.isDone == false)
            {
                yield return 0;
            }

            TextAsset json = resReq.asset as TextAsset;
            StageData[] stageList = JsonMapper.ToObject<StageData[]>(json.text);

            foreach (StageData data in stageList)
            {
                stageDictionary.Add(data.id, data);
            }
        }

        // ステージのプレハブを取得
        public GameObject GetStagePrefab(int sceneId)
        {
            GameObject obj = Resources.Load("Prefabs/" + stageDictionary[sceneId].prefab, typeof(GameObject)) as GameObject;
            return obj;
         }

        // 次のステージIDを取得
        public int GetNextStageId(int next, int stageId = 0)
        {
            int id = stageId == 0 ? currentView : stageId;

            int nextId = next > 0 ? stageDictionary[id].nextStage : stageDictionary[id].preiverStage;

            if (nextId == 0)
            {
                DebugLogger.Log(stageDictionary[id].prefab + " Has Not Next Stage ID");
            }

            return nextId;
        }

        // ジャンプ目的地のIDを取得
        public int GetJumpToStageId(int index)
        {
            if (index >= stageDictionary[currentView].jumpToStage.Length)
            {
                DebugLogger.Log("Has not jumpIndex");
                return 0;
            }
            int jumpTo = stageDictionary[currentView].jumpToStage[index];

            return jumpTo;
        }

        // ゲットするアイテムのIDを取得
        public int GetStageItemId(int index, int stageId = 0)
        {
            int id = stageId == 0 ? currentView : stageId;

            if (index >= stageDictionary[id].getItem.Length)
            {
                DebugLogger.Log("Item Get error");
                return 0;
            }

            int itemId = stageDictionary[id].getItem[index];

            return itemId;
        }

        // ステージのギミックIDをゲット
        public int GetStageGimmickId(int stageId = 0)
        {
            int sid = stageId == 0 ? currentView : stageId;
            return stageDictionary[sid].gimmickId;
        }

        // アイテム使用
        public bool UseItem(int itemId)
        {
            return false;
        }

        public void PlayBGM(string bgm)
        {
            if (SoundManger.Instance != null)
            {
                SoundManger.Instance.PlayBgm(bgm);
            }
        }

        public void PlaySE(string se)
        {
            if (SoundManger.Instance != null)
            {
                SoundManger.Instance.PlaySe(se);
            }
        }

        public void OnReturnToTitle()
        {
            SoundManger.Instance.StopBgm();
            Resources.UnloadUnusedAssets();
            DataManager.Instance.SetStageId(currentView);
            DataManager.Instance.SaveUserData();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TitleScene");
        }
    }
}
