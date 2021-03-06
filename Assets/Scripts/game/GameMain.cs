﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Komugi
{ 
    public class GameMain : SingletonMonoBehaviour<GameMain>
    {
        [SerializeField]
        GameObject LoadingAnime = null;

        private const string TUTORIAL_PATH = "Prefabs/ui/infomation_hint";

        #region =============================== フィールド ===============================
        /** ゲームマネージャー */
        private GameManager gameManager;

        /** アイテムマネージャー */
        private ItemManager itemManager;

        /** ギミックマネージャー */
        private GimmickManager gimmickManager;

	    /** 関数ディクショナリー */
	    Dictionary<string, UnityAction<int, string>> functionDictionary = new Dictionary<string, UnityAction<int, string>>();
        
	    /** 現在表示しているビュー */
	    private GameObject currentViewObject;
        
        #endregion

        #region =============================== Unityメソッド ===============================

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
        }

        // Use this for initialization
        private IEnumerator Start () 
	    {
            gameManager = GameManager.Instance;
            itemManager = ItemManager.Instance;
            gimmickManager = GimmickManager.Instance;

            itemManager.Load();
            gimmickManager.Load();

            // Jasonデータデシリアライズ
            yield return gameManager.Deserialization();
            yield return itemManager.Deserialization();
            yield return gimmickManager.Deserialization();
            
            //リソースフォルダのデータを非同期に読み込む
            yield return LoadAsyncStageCoroutine("Prefabs");

            InitButtonFunction();
            itemManager.AddItemSaveData();

            gameManager.PlayBGM(AudioConst.BGM_MAIN);

            // ローディング画面を消す
            LoadingAnime.SetActive(false);
        }

	    #endregion

	    #region =============================== イベントハンドラ ===============================

	    // 次のステージへ
	    public void nextView(int next)
        {
            int nextId = gameManager.GetNextStageId(next);
            if (nextId == 0) { return; }

            if (gameManager.currentView != nextId) 
		    {
			    if (ChangeView (nextId))
                {
                    gameManager.currentView = nextId;
                }
            }
        }

	    // 特定のステージへジャンプ
	    public void JumpView(int buttonIndex, string buttonName)
	    {
            DebugLogger.Log(" Click Button " + buttonName);
            int jumpTo = gameManager.GetJumpToStageId(buttonIndex);

		    if (gameManager.stageDictionary.ContainsKey (jumpTo)) 
		    {
			    if (ChangeView (jumpTo))
                {
                    gameManager.currentView = jumpTo;
                }
		    }
	    }

        /// <summary>
        /// 行先を指定してジャンプ
        /// </summary>
        /// <param name="jumpTo">目的地</param>
        public void JumpView(int jumpTo)
        {
            if (gameManager.stageDictionary.ContainsKey(jumpTo))
            {
                if (ChangeView(jumpTo))
                {
                    gameManager.currentView = jumpTo;
                }
            }
        }

        // アイテムゲット
        public void GetItem(int itemIndex, string ItemName)
        {
            DebugLogger.Log(" Click Item Name : " + ItemName);

            int itemId = gameManager.GetStageItemId(itemIndex);
            if (itemId == 0) { return; }

            itemManager.AddItem(itemId);

            Transform itemObject = currentViewObject.transform.Find(ItemName);
            if (itemObject == null)
            {
                Button[] ary = currentViewObject.GetComponentsInChildren<Button>();
                foreach (Button b in ary)
                {
                    if (b.name == ItemName)
                    {
                        itemObject = b.transform;
                    }
                }
            }

            if (itemObject != null)
            {
                itemObject.gameObject.SetActive(false);
            }

            gameManager.PlaySE(AudioConst.SE_ITEM_GET);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="ItemName"></param>
        private void ChangeItem(int itemIndex, string ItemName)
        {
            DebugLogger.Log(" Click Item Name : " + ItemName);

            int itemId = gameManager.GetStageItemId(itemIndex);
            if (itemId == 0) { return; }
            
            if (ItemManager.Instance.ItemUpgrade(itemId))
            {
                Transform itemObject = currentViewObject.transform.Find(ItemName);

                if (itemObject != null)
                {
                    itemObject.gameObject.SetActive(false);
                }


            }
        }

        private void ShowTotorial()
        {
            GameObject obj = Resources.Load(TUTORIAL_PATH, typeof(GameObject)) as GameObject;
            GameObject tutorial = Instantiate(obj) as GameObject;

            Button btn = tutorial.GetComponentInChildren<Button>();
            if (btn != null)
            {
                UIManager.Instance.AddContentToMainCanvas(tutorial);
                btn.onClick.AddListener(() =>
                {
                    Destroy(tutorial);
                    DataManager.Instance.SaveTutorialStep(1);
                }
                );
            }
        }
        
	    #endregion

	    #region =============================== C# private ===============================

	    // リソース非同期読み込み
	    private IEnumerator LoadAsyncStageCoroutine(string filePath)
	    {
            // リソースの非同期読込開始
            /*ResourceRequest resReq = Resources.LoadAsync(filePath);
		    // 終わるまで待つ
		    while (resReq.isDone == false)
		    {
			    yield return 0;
		    }
            
		    // 終了時間を表示
		    DebugLogger.Log("End  " + Time.time.ToString());*/

            // ユーザーデータロード
            DataManager.Instance.LoadUserData();
            gameManager.currentView = DataManager.Instance.UserSaveData.currentStageId;

		    //最初の画面を出す
		    ChangeView(gameManager.currentView);
            
            int tutorialId = DataManager.Instance.LoadTutorialStep();
            if (tutorialId == 0)
            {
                ShowTotorial();
            }

            yield break;
        }

        // ステージ変更
        // @param 変更するステージのiD
	    private bool ChangeView(int sceneId)
	    {
		    if (currentViewObject != null) { Destroy (currentViewObject);}
            // ステージのオブジェクトを生成
            GameObject prefab = gameManager.GetStagePrefab(sceneId);

            currentViewObject = Instantiate(prefab);

            if (currentViewObject == null)
            {
                DebugLogger.Log ("Failed to Create View Object. ID : " + sceneId);
                return false;
            }

            UIManager.Instance.ResetStage();
            GimmickManager.Instance.ResetGimmick();
            
            // ギミックのチェックおよびセットアップ
            int gid = gameManager.GetStageGimmickId(sceneId);
            if (gid > 0)
            {
                gimmickManager.SetupGimmick(ref currentViewObject, gid);
            }

            // ボタンに適切なイベントハンドラを設定する
            Button[] buttons = currentViewObject.GetComponentsInChildren<Button>();

            // 該当Tagは何個目の情報を入れるDictionary
            Dictionary<string, int> indexDic = new Dictionary<string, int>();

            for (int i = 0; i < buttons.Length; i++)
		    {
                if (indexDic.ContainsKey(buttons[i].tag))
                {
                    indexDic[buttons[i].tag]++;
                }
                else
                {
                    indexDic.Add(buttons[i].tag, 0);
                }

			    SetButtonAction (buttons[i].tag, ref buttons[i], indexDic[buttons[i].tag], sceneId);
                SetObjectVisible (buttons[i].tag, ref buttons[i], indexDic[buttons[i].tag], sceneId);

            }

            // ルートキャンパスへ追加
            UIManager.Instance.AddContentToMainCanvas(currentViewObject, gameManager.GetNextStageId(1, sceneId), gameManager.GetNextStageId(-1, sceneId));
            DebugLogger.Log("Change Scene to " + prefab.name + "   SceneID : " + sceneId);
            DataManager.Instance.SetStageId(sceneId);

            return true;
	    }

        // ボタンに可視性を設定する
        private void SetObjectVisible(string tag, ref Button button, int index, int sceneId)
        {
            switch (tag)
            {
                case "Jump":
                    break;
                case "Item":
                case "Change":
                    int[] itemList = gameManager.stageDictionary[sceneId].getItem;
                    if (index >= itemList.Length) { return; }

                    bool visible = !itemManager.HasItemList.ContainsKey(itemList[index]);
                    button.gameObject.SetActive(visible);
                    DebugLogger.Log("button.gameObject.SetActive " + visible);
                    break;
                case "Switch":

                    UIManager.Instance.AddChangeableObject(button.gameObject, index);
                    break;
            }
        }

        // ボタンにクリックアクションを設定する
        private void SetButtonAction(string tag, ref Button button, int index, int sceneid)
	    {
		    if (!functionDictionary.ContainsKey (tag)) 
		    {
                DebugLogger.Log ("Tag Function : " + tag + " is Unregistered");
			    return;
		    }

            int uid = sceneid * 1000 + index;
            var builder = new StringBuilder();
            builder.AppendFormat("{0}To{1}_{2}", tag, index, uid);
            button.name = builder.ToString();

            UnityAction<int, string> action = functionDictionary[tag];
            int arg1 = index;
            string arg2 = button.name;
            button.onClick.AddListener (() =>
            {
                action(arg1, arg2);
            }
            );
        }

	    // なんの名前のボタンになんの処理を走らせる関数
	    // ちょっとやり方見苦しいが。。これ以上いい方法思いつかない。。
	    private void InitButtonFunction()
	    {
		    functionDictionary.Add("Jump", JumpView);
            functionDictionary.Add("Item", GetItem);
            functionDictionary.Add("Change", ChangeItem);
	    }

        #endregion

        private void OnApplicationQuit()
        {
            DataManager.Instance.SetStageId(gameManager.currentView);   
            DataManager.Instance.SaveUserData();
        }

    }
}

