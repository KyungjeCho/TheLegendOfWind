using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class NPCData : BaseData<NPCClip>
    {
        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                data = new NPCClip[] { new NPCClip() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                data = ArrayHelper.Add(new NPCClip(), data);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index)
        {
            if (names.Length == 0)
            {
                names = null;
                data = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            data = ArrayHelper.Remove(index, data);
        }

        public NPCClip GetCopy(int index)
        {
            if (index < 0 || index >= data.Length)
                return null;

            NPCClip copy = new NPCClip();
            NPCClip original = data[index];
            copy.clipPath = original.clipPath;
            copy.clipName = original.clipName;
            copy.PreLoad();
            return copy;
        }
    }
}