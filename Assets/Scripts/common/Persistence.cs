using UnityEngine;

namespace Komugi
{ 
    public class Persistence : MonoBehaviour
    {
        public static bool Created = false;

	    // Use this for initialization
	    void Awake ()
        {
            DontDestroyOnLoad(this);
	    }
    }
}