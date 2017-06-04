using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Komugi
{ 
    public class GameMain : SingletonMonoBehaviour<GameMain>
    {

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

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
        }

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

	    #region =============================== イベントハンドラ ===============================

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
            Debug.Log(" Click Button " + buttonName);
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

        // アイテムゲット
        public void GetItem(int itemIndex, string ItemName)
        {
            Debug.Log(" Click Item" + ItemName);
            if (itemIndex >= gameManager.stageDictionary[gameManager.currentView].getItem.Length)
            {
                Debug.Log(ItemName + "Has not Setting");
                return;
            }

            int itemId = gameManager.stageDictionary[gameManager.currentView].getItem[itemIndex];
            itemManager.AddItem(itemId);

            GameObject itemObject = currentViewObject.transform.Find(ItemName).gameObject;

            if (ItemName != null)
            {
                itemObject.SetActive(false);
            }

            UIManager.Instance.AddItemToItemBar(itemId);
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
                SetObjectVisible (buttons[i].tag, ref buttons[i], indexDic[buttons[i].tag], sceneId);

            }

            Debug.Log("Change Scene to " + gameManager.stageDictionary[sceneId].prefab + "   SceneID : " + sceneId);

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

                    int[] itemList = gameManager.stageDictionary[sceneId].getItem;
                    if (index >= itemList.Length) { return; }

                    bool visible = !itemManager.HasItemList.ContainsKey(itemList[index]);
                    button.gameObject.SetActive(visible);
                    Debug.Log("button.gameObject.SetActive " + visible);
                    break;
            }
        }

        // ボタンにクリックアクションを設定する
        private void SetButtonAction(string tag, ref Button button, int index)
	    {
		    if (!functionDictionary.ContainsKey (tag)) 
		    {
			    Debug.Log ("Tag Function : " + tag + " is Unregistered");
			    return;
		    }

            var builder = new StringBuilder();
            builder.AppendFormat("{0}To{1}", tag, index);
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
		    functionDictionary.Add ("Jump", JumpView);
            functionDictionary.Add("Item", GetItem);
	    }

	    #endregion
    }
}

