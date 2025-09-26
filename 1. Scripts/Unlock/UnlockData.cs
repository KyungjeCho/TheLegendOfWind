using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class UnlockData : BaseData<Unlock>
    {
        public event Action<UnlockList, bool> OnUnlock;

        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                data = new Unlock[] { new Unlock() };
            }
            else
            {
                names = ArrayHelper.Add(name, names);
                data = ArrayHelper.Add(new Unlock(), data);
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
        public Unlock GetCopy(int index)
        {
            if (index < 0 || index >= names.Length)
            {
                return null;
            }

            Unlock copy = new Unlock();
            Unlock original = data[index];
            copy.isUnlocked = original.isUnlocked;
            return copy;
        }

        public bool GetIsUnlocked(UnlockList unlockList)
        {
            return data[(int)unlockList].isUnlocked;
        }
        public void SetIsUnlocked(UnlockList unlockList, bool isUnlocked)
        {
            data[(int)unlockList].isUnlocked = isUnlocked;
            OnUnlock?.Invoke(unlockList, isUnlocked);
            SaveData();
        }
    }
}