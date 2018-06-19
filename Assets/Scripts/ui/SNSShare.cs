using System.Collections;
using UnityEngine;

namespace Komugi.Community
{ 
    public class SNSShare : MonoBehaviour
    {
        public void Share(string msg)
        {
            StartCoroutine(ShareScreenShot(msg));
        }

        IEnumerator ShareScreenShot(string msg)
        {
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
        }
    }
}