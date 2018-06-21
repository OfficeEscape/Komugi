using System.Collections;
using UnityEngine;

namespace Komugi.Community
{ 
    public class SNSShare : MonoBehaviour
    {
        public void Share(string msg, string url, string imageFile)
        {
            StartCoroutine(ShareScreenShot(msg, url, imageFile));
        }

        IEnumerator ShareScreenShot(string msg, string url, string imageFile)
        {
            /*
            //ファイル名が重複しないように実行時間を付与。
            string fileName = System.DateTime.Now.ToString("ScreenShot yyyy-MM-dd HH.mm.ss") + ".png";

            //スクリーンショット画像の保存先を設定。
            string imagePath = Application.persistentDataPath + "/" + fileName;

            //スクリーンショットを撮影
            Application.CaptureScreenshot(fileName);

            yield return new WaitForEndOfFrame();

            // アプリやシーンごとにShareするメッセージを設定
            string text = msg;
            string URL = "url";
            yield return new WaitForSeconds(1);
            
            //Shareする
            SocialConnector.SocialConnector.Share(text, URL, imagePath);
            */
            
            string streamAssetsImagePath = System.IO.Path.Combine(Application.streamingAssetsPath, imageFile);

            yield return new WaitForSeconds(0.5f);

#if UNITY_ANDROID
            WWW www = new WWW(streamAssetsImagePath); // ローカルファイルを読む
            yield return www;

            string imagePath = Application.temporaryCachePath + "/" + imageFile;
            System.IO.File.WriteAllBytes(imagePath, www.bytes);
            SocialConnector.SocialConnector.Share(msg, url, imagePath);
#else
        // iOSなら直接StreamAssetsが読める
        SocialConnector.SocialConnector.Share(text, url, streamAssetsImagePath);
        yield break;
#endif
        }
    }
}