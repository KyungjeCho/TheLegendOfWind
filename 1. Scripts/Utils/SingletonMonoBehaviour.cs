using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance => instance;

    public static T GetOrCreateInstance()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                GameObject newGameObject = new GameObject(typeof(T).Name, typeof(T));
                instance = newGameObject.AddComponent<T>();

            }
        }
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
        if (Application.isPlaying == true)
        {
            if (transform.parent != null && transform.root != null)
            {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
