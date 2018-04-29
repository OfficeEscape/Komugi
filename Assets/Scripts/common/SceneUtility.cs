using UnityEngine;
using UnityEngine.SceneManagement;

namespace Komugi
{
    public class SceneUtility : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
