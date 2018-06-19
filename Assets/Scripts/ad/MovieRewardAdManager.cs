using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.Ad
{
    public class MovieRewardAdManager : SingletonMonoBehaviour<MovieRewardAdManager>
    {
        private AdfurikunMovieRewardUtility adutil;
        private bool initialized = false;
        private enum SCENE_STATE { MAIN, QUIT_WAIT, QUIT, END };
        private SCENE_STATE sceneState = SCENE_STATE.MAIN;

        public Action closeCallBack = null;

        public Action finishCallBack = null;

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();
            
            if (adutil == null) { adutil = GameObject.Find("AdfurikunMovieRewardUtility").GetComponent<AdfurikunMovieRewardUtility>(); }
        }


        // Use this for initialization
        void Start()
        {
            adutil.initializeMovieReward();
        }

        /// <summary>
        /// リワード動画を再生する
        /// </summary>
        public void playRewardMovie()
        {
            StartCoroutine(StartMovie());
        }

        /// <summary>
        /// リワード動画再生時のアドフリ君処理
        /// </summary>
        /// <returns></returns>
        IEnumerator StartMovie()
        {
            SoundManger.Instance.BgmOff = 1;
#if UNITY_EDITOR
            Debug.Log("yield return null");
            yield return null;
#elif UNITY_IOS
        while (!adutil.isPreparedMovieReward()) {
            yield return new WaitForSeconds(0.2f);
        }
            
        adutil.playMovieReward();
#else
        //リワード動画の準備ができるまでWaitして再生開始
        
        Debug.Log("adutil.isPreparedMovieReward() : " + adutil.isPreparedMovieReward());
        while (!adutil.isPreparedMovieReward()) {
            yield return new WaitForSeconds(0.2f);
        }
        
        Debug.Log("adutil.playMovieReward(); ");
        adutil.playMovieReward();
#endif
        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
                adutil.setMovieRewardSrcObject(gameObject);
            }
            switch (this.sceneState)
            {
                case SCENE_STATE.MAIN:
                    break;
                case SCENE_STATE.QUIT_WAIT:
                    sceneState = SCENE_STATE.QUIT;
                    break;
                case SCENE_STATE.QUIT:
                    sceneState = SCENE_STATE.END;
                    break;
                case SCENE_STATE.END:
                    SoundManger.Instance.BgmOff = 0;
                    break;
            }
        }

        void MovieRewardCallback(ArrayList vars)
        {
            int stateName = (int)vars[0];
            string appID = (string)vars[1];
            string adnetworkKey = (string)vars[2];

            AdfurikunMovieRewardUtility.ADF_MovieStatus state = (AdfurikunMovieRewardUtility.ADF_MovieStatus)stateName;
            switch (state)
            {
                case AdfurikunMovieRewardUtility.ADF_MovieStatus.PrepareSuccess:
                    //"準備完了"
                    Debug.Log("リワード動画：準備完了");
                    break;
                case AdfurikunMovieRewardUtility.ADF_MovieStatus.StartPlaying:
                    //"再生開始"
                    Debug.Log("リワード動画：再生開始");
                    break;
                case AdfurikunMovieRewardUtility.ADF_MovieStatus.FinishedPlaying:
                    //"再生完了"
                    Screen.orientation = ScreenOrientation.Portrait;
                    Debug.Log("リワード動画：再生完了");
                    //ここで報酬を付与します
                    if (finishCallBack != null)
                    {
                        finishCallBack.Invoke();
                    }
                    break;
                case AdfurikunMovieRewardUtility.ADF_MovieStatus.FailedPlaying:
                    //"再生失敗"
                    Screen.orientation = ScreenOrientation.Portrait;
                    Debug.Log("リワード動画：再生失敗");
                    break;
                case AdfurikunMovieRewardUtility.ADF_MovieStatus.AdClose:
                    //"動画を閉じた"
                    Screen.orientation = ScreenOrientation.Portrait;
                    Debug.Log("リワード動画：動画を閉じた");
                    if (closeCallBack != null)
                    {
                        closeCallBack.Invoke();
                    }
                    break;
                default:
                    Debug.Log("リワード動画：その他");
                    return;
            }
        }
    }
}