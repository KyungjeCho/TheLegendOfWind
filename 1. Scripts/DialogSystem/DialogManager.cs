using KJ;
using KJ.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : SingletonMonoBehaviour<DialogManager>
{
    [SerializeField]
    private DialogDB db;
    [SerializeField]
    private UIDialogPanelController dialogPanelController;
    private Dictionary<string, DialogObject> dict = new Dictionary<string, DialogObject>();
    private DialogObject currentDialog;

    protected StateMachine<DialogManager> stateMachine;
    
    public DialogDB DB => db;
    public UIDialogPanelController DialogPanelController => dialogPanelController;
    public DialogObject CurrentDialog => currentDialog;

    protected override void Awake()
    {
        base.Awake();

        if (db.Container == null)
        {
            Debug.Log("Load Dialog CSV");
            db.LoadCSV();
        }

        foreach (var d in db.Container)
        {
            dict.Add(d.id, d);
        }
    }

    private void Start()
    {
        stateMachine = new StateMachine<DialogManager>(this, new EndState());

        stateMachine.AddState(new DialogState());
        stateMachine.AddState(new StartState());
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
    public DialogObject GetDialogObject(string dialogId)
    {
        return dict.TryGetValue(dialogId, out var value) ? value : null;
    }
    public void SetCurrentDialog(string dialogId)
    {
        Debug.Log(dict[dialogId]);
        currentDialog = dict.TryGetValue(dialogId, out var value) ? value : null;
    }
    public void SetCurrentDialog(DialogObject dialogObject)
    {
        currentDialog = dialogObject;
    }
    public void StartDialog(string dialogId)
    {
        SetCurrentDialog(dialogId);
        stateMachine.ChangeState<StartState>();
    }
}
