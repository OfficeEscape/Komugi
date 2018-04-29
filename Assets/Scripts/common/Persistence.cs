using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistence : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
		if (SceneManager.GetActiveScene().name != "EndingScene")
        {
            DontDestroyOnLoad(this);
        }
	}
}
