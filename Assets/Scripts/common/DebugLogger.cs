using System.Diagnostics;

public class DebugLogger
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object o)
    {
        UnityEngine.Debug.LogError(o);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object o)
    {
        UnityEngine.Debug.LogWarning(o);
    }
}
