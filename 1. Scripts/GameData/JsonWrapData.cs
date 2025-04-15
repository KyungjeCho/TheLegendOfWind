using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class JsonWrapData<T>
    {
        public T[] data;
        public string[] names;
        public JsonWrapData(T[] data, string[] names) { this.data = data; this.names = names; }
    }
}


