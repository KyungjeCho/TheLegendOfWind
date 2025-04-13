using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 데이터 기본 클래스
/// 
/// </summary>
public class BaseData : ScriptableObject
{
    public const string dataDirectory = "Assets/9. Resources/Resources/Data";
    public string[] names = null;

    public int GetDataCount() => names != null ? names.Length : 0;

    public string[] GetNameList(bool indexing)
    {
        if (names == null)
            return new string[0];

        if (indexing)
            return names
                .Select((x, i) => i + ". " + x).ToArray();
        else
            return names;
    }

    public virtual int AddData(string name) { return GetDataCount(); }
    public virtual void RemoveData(int index) { }
}
