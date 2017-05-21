using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {

	#region =============================== フィールド ===============================
    /** ゲームマネージャー */
    private GameManager gameManager;

	/** 関数ディクショナリー */
	Dictionary<string, UnityAction> functionDictionary = new Dictionary<string, UnityAction>();

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
		gameManager.OpenBinary ();
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
		current = current + next;
		current = Mathf.Clamp (current, 0, gameManager.stageDictionary.Count-1);
		if (gameManager.currentView != current) 
		{
			gameManager.currentView = current;
			Destroy (currentViewObject);
			ChangeView (gameManager.currentView);
		}
    }

	// 特定のステージへジャンプ
	public void JumpView()
	{
		int jumpTo = gameManager.stageDictionary[gameManager.currentView].jumpToStage;

		if (gameManager.stageDictionary.ContainsKey (jumpTo)) 
		{
			ChangeView (jumpTo);
			gameManager.currentView = jumpTo;
		}
	}

    public void scaleView()
    {

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

	private void ChangeView(int sceneId)
	{
		if (currentViewObject != null) { Destroy (currentViewObject);}
		currentViewObject = Instantiate(Resources.Load("Prefabs/" + gameManager.stageDictionary[sceneId].prefab, typeof(GameObject))) as GameObject;
		if (currentViewObject == null) { Debug.Log ("Failed to Create View Object. ID : " + gameManager.currentView);}
		currentViewObject.transform.SetParent(mainCanvas.transform);
		currentViewObject.transform.SetAsFirstSibling();
		currentViewObject.transform.localPosition = Vector3.zero;
		currentViewObject.transform.localScale = Vector3.one;

		Button[] buttons = currentViewObject.GetComponentsInChildren<Button> ();
		for (int i = 0; i < buttons.Length; i++)
		//foreach (Button btn in buttons)
		{
			SetButtonAction (buttons[i].tag, ref buttons[i]);
			//btn.onClick.AddListener(JumpView);
			Debug.Log (" set button action ");
		}
	}

	// ボタンにクリックアクションを設定する
	private void SetButtonAction(string tag, ref Button button)
	{
		if (!functionDictionary.ContainsKey (tag)) 
		{
			Debug.Log ("Tag Function : " + tag + " is Unregistered");
			return;
		}

		button.onClick.AddListener (functionDictionary [tag]);
	}

	// なんの名前のボタンになんの処理を走らせる関数
	// ちょっとやり方見苦しいが。。これ以上いい方法思いつかない。。
	private void InitButtonFunction()
	{
		functionDictionary.Add ("Jump", JumpView);
	}

	#endregion
}


