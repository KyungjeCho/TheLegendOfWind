using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : SingletonMonoBehaviour<DialogManager>
{
    [SerializeField]
    private DialogDB db;

    public DialogObject GetDialogObject(string dialogId)
    {
        if (db == null || db.Container == null)
        {
            return null;
        }
        return db.Container[dialogId];
    }
}
