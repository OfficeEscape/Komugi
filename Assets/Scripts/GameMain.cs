using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class GameMain : MonoBehaviour {

    /** 各画面のステージデータ */
    private StageData[] stageList;

    /** ゲームマネージャー */
    private GameManager gameManager;

    [SerializeField]
    /** メインキャンパス */
    private Canvas mainCanvas;

	// Use this for initialization
	void Start () {

        TextAsset json = Resources.Load("Data/StageData") as TextAsset;
        stageList = JsonMapper.ToObject<StageData[]>(json.text);
        Debug.Log(stageList[0].openType + Time.time.ToString());

        gameManager = GameManager.Instance;
        //リソースフォルダのデータを非同期に読み込む
        StartCoroutine(LoadAsyncStageCoroutine(""));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
        GameObject obj = Instantiate(Resources.Load("Prefabs/" + stageList[gameManager.currentView].prefab, typeof(GameObject))) as GameObject;
        obj.transform.SetParent(mainCanvas.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.SetAsFirstSibling();
    }

    public void nextView()
    {

    }

    public void scaleView()
    {

    }

    public void openDoor()
    {

    }
}


