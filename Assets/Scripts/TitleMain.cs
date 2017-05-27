using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMain : MonoBehaviour {

    private GameObject _canvas;

	// Use this for initialization
	void Start () {
        _canvas = transform.Find("Canvas").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startButtonHandler ()
    {
        //SceneManager.LoadScene("GameScene");
        Button[] btns = _canvas.GetComponentsInChildren<Button>();
        foreach(Button btn in btns)
        {
            btn.enabled = false;
        }

        Animator animator = _canvas.GetComponent<Animator>();
        animator.Play("startAnimation");
        Invoke("createOpening", 1f);
    }

    public void continueButtonHandler ()
    {

    }

    private void createOpening()
    {
        GameObject obj = Resources.Load("Prefabs/opening", typeof(GameObject)) as GameObject;
        GameObject opening = Instantiate(obj) as GameObject;
        opening.transform.SetParent(_canvas.transform);

        RectTransform rectTransform = opening.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}
