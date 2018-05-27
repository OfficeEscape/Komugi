﻿using UnityEngine;
using UnityEngine.UI;

namespace Komugi
{
    public class TitleMain : MonoBehaviour
    {
        private GameObject _canvas;

        [SerializeField]
        private GameObject GameManagerObj = null;

        [SerializeField]
        Button startButton = null;

        [SerializeField]
        Button continueButton = null;

        // Use this for initialization
        void Start()
        {
            _canvas = transform.Find("Canvas").gameObject;

            startButton.onClick.AddListener(() => StartButtonHandler());
            continueButton.onClick.AddListener(() => ContinueButtonHandle());
            continueButton.gameObject.SetActive(DataManager.Instance.CheckSaveData());

            Invoke("PlayBGM", 3f);

            if (!Persistence.Created)
            {
                Instantiate(GameManagerObj);
                Persistence.Created = true;
            }
        }
        
        public void StartButtonHandler()
        {
            Button[] btns = _canvas.GetComponentsInChildren<Button>();
            foreach (Button btn in btns)
            {
                btn.enabled = false;
            }

            Animator animator = _canvas.GetComponent<Animator>();
            animator.Play("startAnimation");
            Invoke("createOpening", 1f);

            if (SoundManger.Instance != null)
            {
                SoundManger.Instance.PlaySe(AudioConst.SE_BUTTON);
            }

            DataManager.Instance.SaveDataReset();
        }

        public void ContinueButtonHandle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
        }


        private void createOpening()
        {
            GameObject obj = Resources.Load("Prefabs/office/opening", typeof(GameObject)) as GameObject;
            GameObject opening = Instantiate(obj) as GameObject;
            opening.transform.SetParent(_canvas.transform, false);
        }

        private void PlayBGM()
        {
            if (SoundManger.Instance != null)
            {
                SoundManger.Instance.PlayBgm(AudioConst.BGM_OPENING);
            }
        }
    }
}
