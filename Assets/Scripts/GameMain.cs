using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Komugi
{ 
    public class GameMain : MonoBehaviour {

        #region =============================== フィールド ===============================
        /** ゲームマネージャー */
        private GameManager gameManager;

        /** アイテムマネージャー */
        private ItemManager itemManager;

	    /** 関数ディクショナリー */
	    Dictionary<string, UnityAction<int, string>> functionDictionary = new Dictionary<string, UnityAction<int, string>>();

        [SerializeField]
        /** メインキャンパス */
        private Canvas mainCanvas;

	    /** 現在表示しているビュー */
	    private GameObject currentViewObject;

	    #endregion

	    #region =============================== Unityメソッド ===============================

	    // Use this for initialization
	    void Start () 
	    {
            gameManager = GameManager.Instance;
            itemManager = ItemManager.Instance;

            gameManager.OpenBinary ();
            itemManager.OpenBinary();

		    InitButtonFunction ();
            //リソースフォルダのデータを非同期に読み込む
            StartCoroutine(LoadAsyncStageCoroutine(""));
	    }
	
	    // Update is called once per frame
	    void Update () 
	    {
	
	    }

	    #endregion

	    #region =============================== C# public ===============================

	    // 次のステージへ
	    public void nextView(int next)
        {
		    int current = gameManager.currentView;
            int nextId = next > 0 ? gameManager.stageDictionary[current].nextStage : gameManager.stageDictionary[current].preiverStage;

            if (nextId == 0)
            {
                Debug.Log(gameManager.stageDictionary[current].prefab + " Has Not Next Stage ID");
                return; 
            }

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
            Debug.Log(" click " + buttonName);
            if (buttonIndex >= gameManager.stageDictionary[gameManager.currentView].jumpToStage.Length)
            {
                Debug.Log(buttonName + "Has not jumpIndex");
                return;
            }
		    int jumpTo = gameManager.stageDictionary[gameManager.currentView].jumpToStage[buttonIndex];

		    if (gameManager.stageDictionary.ContainsKey (jumpTo)) 
		    {
			    if (ChangeView (jumpTo))
                {
                    gameManager.currentView = jumpTo;
                }
		    }
	    }

        public void openDoor()
        {

        }

	    #endregion

	    #region =============================== C# private ===============================

	    // リソース非同期読み込み
	    private IEnumerator LoadAsyncStageCoroutine(string filePath)
	    {
		    // リソースの非同期読込開始
		    ResourceRequest resReq = Resources.LoadAsync(filePath);
		    // 終わるまで待つ
		    while (resReq.isDone == false)
		    {
			    Debug.Log("Loading progress:" + resReq.progress.ToString());
			    yield return 0;
		    }
		    // テクスチャ表示
		    Debug.Log("End  " + Time.time.ToString());

		    //最初の画面を出す
		    ChangeView(gameManager.currentView);
	    }

        // ステージ変更
        // @param 変更するステージのiD
	    private bool ChangeView(int sceneId)
	    {
		    if (currentViewObject != null) { Destroy (currentViewObject);}
            // ステージのオブジェクトを生成
		    currentViewObject = Instantiate(Resources.Load("Prefabs/" + gameManager.stageDictionary[sceneId].prefab, typeof(GameObject))) as GameObject;

            if (currentViewObject == null)
            {
                Debug.Log ("Failed to Create View Object. ID : " + gameManager.currentView);
                return false;
            }

            // ルートキャンパスへ追加
            currentViewObject.transform.SetParent(mainCanvas.transform);
		    currentViewObject.transform.SetAsFirstSibling();
		    currentViewObject.transform.localPosition = Vector3.zero;
		    currentViewObject.transform.localScale = Vector3.one;

            // ボタンに適切なイベントハンドラを設定する
		    Button[] buttons = currentViewObject.GetComponentsInChildren<Button> ();

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

			    SetButtonAction (buttons[i].tag, ref buttons[i], indexDic[buttons[i].tag]);
		    }

            Debug.Log("Change Scene to " + gameManager.stageDictionary[sceneId].prefab + "   SceneID : " + sceneId);

            return true;
	    }

	    // ボタンにクリックアクションを設定する
	    private void SetButtonAction(string tag, ref Button button, int index)
	    {
		    if (!functionDictionary.ContainsKey (tag)) 
		    {
			    Debug.Log ("Tag Function : " + tag + " is Unregistered");
			    return;
		    }

            UnityAction<int, string> action = functionDictionary[tag];
            int arg1 = index;
            string arg2 = button.name;
            button.onClick.AddListener (() =>
            {
                action(arg1, arg2);
            }
            );
            var builder = new StringBuilder();
            builder.AppendFormat("{0}To{1}", tag, index);
            button.name = builder.ToString();
        }

	    // なんの名前のボタンになんの処理を走らせる関数
	    // ちょっとやり方見苦しいが。。これ以上いい方法思いつかない。。
	    private void InitButtonFunction()
	    {
		    functionDictionary.Add ("Jump", JumpView);
	    }

	    #endregion
    }
}

