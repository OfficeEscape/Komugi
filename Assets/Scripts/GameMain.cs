using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {

	#region =============================== フィールド ===============================
    /** 各画面のステージデータ */
	private Dictionary<int, StageData> stageDictionary = new Dictionary<int, StageData> ();

    /** ゲームマネージャー */
    private GameManager gameManager;

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
        //リソースフォルダのデータを非同期に読み込む
        StartCoroutine(LoadAsyncStageCoroutine(""));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	#endregion

	#region =============================== C#メソッド ===============================

	// リソース非同期読み込み
    public IEnumerator LoadAsyncStageCoroutine(string filePath)
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
        Debug.Log("終了  " + Time.time.ToString());

        //最初の画面を出す
		ChangeView(gameManager.currentView);
    }

	private void ChangeView(int sceneId)
	{
		if (currentViewObject != null) { Destroy (currentViewObject);}
		currentViewObject = Instantiate(Resources.Load("Prefabs/" + gameManager.stageDictionary[sceneId].prefab, typeof(GameObject))) as GameObject;
		currentViewObject.transform.SetParent(mainCanvas.transform);
		currentViewObject.transform.localPosition = Vector3.zero;
		currentViewObject.transform.localScale = Vector3.one;
		currentViewObject.transform.SetAsFirstSibling();
	}

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

    public void scaleView()
    {

    }

    public void openDoor()
    {

    }

	#endregion
}


