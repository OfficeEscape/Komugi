﻿using Komugi.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi
{
    public class TitleMain : MonoBehaviour
    {
        private const string START_DIALOG = "最初からやり直しになります\nよろしいですか";

        [SerializeField]
        private GameObject GameManagerObj = null;

        [SerializeField]
        Button startButton = null;

        [SerializeField]
        Button continueButton = null;

        [SerializeField]
        Button creditButton = null;

        [SerializeField]
        Button exitButton = null;

        private GameObject _canvas;

        private DialogMenu dialog;

        // Use this for initialization
        void Start()
        {
            _canvas = transform.Find("Canvas").gameObject;

            startButton.onClick.AddListener(() => StartButtonHandler());
            continueButton.onClick.AddListener(() => ContinueButtonHandle());
            continueButton.gameObject.SetActive(DataManager.Instance.CheckSaveData());
            creditButton.onClick.AddListener(() => CreditButtonHandle());
            exitButton.onClick.AddListener(() => Application.Quit());

            dialog = _canvas.AddComponent<DialogMenu>();

            Invoke("PlayBGM", 3f);

            if (!Persistence.Created)
            {
                Instantiate(GameManagerObj);
                Persistence.Created = true;
            }
        }
        
        public void StartButtonHandler()
        {
            if (continueButton.gameObject.activeSelf)
            {
                dialog.OpenCheckDialog(START_DIALOG, StartSequence);
            }
            else
            {
                StartSequence((int)CheckDialog.ResultType.OK);
            }
        }

        public void ContinueButtonHandle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
        }

        public void CreditButtonHandle()
        {
            GameObject obj = Resources.Load("Prefabs/ui/credit", typeof(GameObject)) as GameObject;
            GameObject credit = Instantiate(obj) as GameObject;

            Button btn = credit.GetComponent<Button>();
            if (btn != null)
            {
                credit.transform.SetParent(_canvas.transform, false);
                btn.onClick.AddListener(() => Destroy(credit));
            }
            
        }


        private void createOpening()
        {
            GameObject obj = Resources.Load("Prefabs/ui/opening", typeof(GameObject)) as GameObject;
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

        private void StartSequence(int res)
        {
            if (res == (int)CheckDialog.ResultType.CANCEL)
            {
                return;
            }

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
    }
}
