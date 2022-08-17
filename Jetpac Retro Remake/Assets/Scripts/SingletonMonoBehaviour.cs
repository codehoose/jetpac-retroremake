using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }

            return _instance;
        }
    }
}
