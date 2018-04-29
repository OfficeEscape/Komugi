using UnityEngine;
using UnityEngine.UI;

namespace Komugi
{
    public class TitleMain : MonoBehaviour
    {

        private GameObject _canvas;

        // Use this for initialization
        void Start()
        {
            _canvas = transform.Find("Canvas").gameObject;
            Invoke("PlayBGM", 3f);
        }
        
        public void startButtonHandler()
        {
            Button[] btns = _canvas.GetComponentsInChildren<Button>();
            foreach (Button btn in btns)
            {
                btn.enabled = false;
            }

            Animator animator = _canvas.GetComponent<Animator>();
            animator.Play("startAnimation");
            Invoke("createOpening", 1f);
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
