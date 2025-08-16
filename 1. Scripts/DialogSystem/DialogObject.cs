using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class DialogObject 
    {
        public string id;
        public string speaker;
        public string dialog;
        public string nextId;
        public string choices;
        public string choicesNextId;
        public string trigger;
    }
}
