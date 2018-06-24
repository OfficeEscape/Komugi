using System.Collections;
using UnityEngine;

namespace Komugi.Community
{ 
    public class SNSShare : MonoBehaviour
    {
        private bool isPrepared = true;

        public void Share(string msg, string url, string imageFile)
        {
            if (!isPrepared) { return; }
            StartCoroutine(ShareScreenShot(msg, url, imageFile));
        }

        IEnumerator ShareScreenShot(string msg, string url, string imageFile)
        {
            isPrepared = false;

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
            SocialConnector.SocialConnector.Share(msg, url, streamAssetsImagePath);
#endif
            yield return new WaitForSeconds(0.5f);

            isPrepared = true;

        }
    }
}