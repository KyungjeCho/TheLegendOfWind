using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KJ
{
    public class ArrayHelper : MonoBehaviour
    {
        public static T[] Add<T>(T n, T[] list)
        {
            ArrayList tmp = new ArrayList();
            foreach (T item in list) tmp.Add(item);
            tmp.Add(n);
            return tmp.ToArray(typeof(T)) as T[];
        }

        public static T[] Remove<T>(int index, T[] list)
        {
            ArrayList tmp = new ArrayList();
            foreach (T item in list) tmp.Add(item);
            tmp.RemoveAt(index);
            return tmp.ToArray(typeof(T)) as T[];
        }
    }
}

