using UnityEngine;
using System.Collections;

public class GameManager {

	private static GameManager mInstance;

    /** 現在表示している画面 */
    public int currentView = 0;

    /** 現在表示している画面の何番目の画像 */
    public int currentAngle = 0;

    // Private Constructor
    private GameManager()
    { 
        Debug.Log("Create SampleSingleton instance.");
    }

    public static GameManager Instance
    {
        get {

            if (mInstance == null) mInstance = new GameManager();

            return mInstance;
        }
    }
}
