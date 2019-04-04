using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region ATTRIBUTES

    static T _instance;

    static object _lock = new object();

    static bool applicationIsQuitting = false;

    #endregion

    #region METHODS

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(Singleton) " + typeof(T).ToString();
                    }
                }
                return _instance;
            }
        }
    }

    /// <summary>
    /// Prevents Unity from creating a new instance when quitting the application
    /// if any script tries to access it after it has already been destroyed
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    #endregion
}
