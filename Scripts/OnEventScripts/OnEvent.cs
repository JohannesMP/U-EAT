using UnityEngine;
using System.Collections;
using ActionSystem;

using System.Collections.Generic;

using System;

public class OnEvent : MonoBehaviour
{
    public EventData Data = new EventData();
    Action<EventData> OnEventFunctionProp;
    public Action<EventData> OnEventFunction
    {
        get
        {
            return OnEventFunctionProp;
        }
        set
        {
            OnEventFunctionProp = value;
            Reconnect();
        }
    }
    [ExposeProperty]
    public bool Active
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
        }
    }
    [SerializeField]
    Events ListenEventNameProp = Events.DefaultEvent;
    [ExposeProperty]
    public Events ListenEventName
    {
        get
        {
            return ListenEventNameProp;
        }
        set
        {
            ListenEventNameProp = value;
            Reconnect();
        }
    }
    [SerializeField]
    GameObject ListenTargetProp;
    [ExposeProperty]
    public GameObject ListenTarget
    {
        get
        {
            if(ListenTargetProp == null)
            {
                ListenTargetProp = gameObject;
            }
            return ListenTargetProp;
        }
        set
        {
            ListenTargetProp = value;
            Reconnect();
        }
    }
    public bool IsConnected { get; private set; }
    public bool DispatchEvents = false;
    [SerializeField]
    Events DispatchEventNameProp = Events.DefaultEvent;
    [ExposeProperty]
    public Events DispatchEventName
    {
        get
        {
            return DispatchEventNameProp;
        }
        set
        {
            DispatchEventNameProp = value;
            Reconnect();
        }
    }
    [SerializeField]
    GameObject DispatchTargetProp;
    [ExposeProperty]
    public GameObject DispatchTarget
    {
        get
        {
            return DispatchTargetProp;
        }
        set
        {
            DispatchTargetProp = value;
            Reconnect();
        }
    }
    protected bool DelayedDispatch = false;
    //ActionSequence Seq = new ActionSequence();

    public void SetActive(bool setter)
    {
        Active = setter;
    }

    public virtual void Awake()
    {
        if(!IsConnected)
        {
            if (ListenTargetProp == null)
            {
                ListenTargetProp = gameObject;
            }
            if (OnEventFunctionProp == null)
            {
                OnEventFunctionProp = OnEventFunc;
            }
            Connect();
        }
    }

    public virtual void Start()
    {
    }

    public void Connect()
    {
        IsConnected = true;
        ListenTarget.Connect(ListenEventName, OnEventInternal);
    }

    public void Disconnect()
    {
        IsConnected = false;
        ListenTarget.Disconnect(ListenEventName, OnEventInternal);
    }

    public void Reconnect(GameObject listenTarget = null, string eventName = null, System.Action<EventData> onEventFunc = null)
    {
        if(IsConnected)
        {
            ListenTarget.Disconnect(ListenEventName, OnEventInternal);
        }
        if(listenTarget != null)
        {
            ListenTargetProp = listenTarget;
        }
        
        if(eventName != null)
        {
            ListenEventNameProp = eventName;
        }
        if(onEventFunc != null)
        {
            OnEventFunctionProp = onEventFunc;
        }
        
        ListenTarget.Connect(ListenEventName, OnEventInternal);
    }

    void OnEventInternal(EventData data)
    {
        if (!Active)
        {
            return;
        }

        OnEventFunction(data);

        if(DispatchEvents && !DelayedDispatch)
        {
            DispatchEvent();
        }
    }

    virtual public void OnEventFunc(EventData data)
    {

    }

    public void DispatchEvent()
    {
        if(DispatchTarget == null)
        {
            return;
        }
        DispatchTarget.DispatchEvent(DispatchEventName, Data);
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(OnEvent), true)]
    public class OnEventEditor : Editor
    {
        protected Dictionary<Type, System.Action> EditorDrawFunctions = new Dictionary<Type, System.Action>();
        PropertyInfo ListenEventNameProp;
        PropertyInfo ListenTargetProp;
        PropertyInfo DispatchEventNameProp;
        PropertyInfo DispatchTargetProp;
        //For the OnEventFunc dropdown.
        //string[] FunctionNames;
        //List<Action<EventData>> FunctionList;

        public virtual void OnEnable()
        {
            EditorDrawFunctions.Add(typeof(OnEvent), Draw);
            OnEvent comp = target as OnEvent;
            if (comp.ListenTarget == null)
            {
                comp.ListenTarget = comp.gameObject;
            }
            if (comp.DispatchTarget == null)
            {
                comp.DispatchTarget = comp.gameObject;
            }

            if (comp.OnEventFunction == null)
            {
                comp.OnEventFunction = comp.OnEventFunc;
            }
            ListenEventNameProp = target.GetType().GetProperty("ListenEventName");
            ListenTargetProp = target.GetType().GetProperty("ListenTarget");
            DispatchEventNameProp = target.GetType().GetProperty("DispatchEventName");
            DispatchTargetProp = target.GetType().GetProperty("DispatchTarget");
            //For the OnEventFunc dropdown.
            //var functions = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            //List<string> functionNames = new List<string>();
            //FunctionList = new List<System.Action<EventData>>();
            //foreach(var i in functions)
            //{
            //    if(i.ReturnType == typeof(void) && !i.IsConstructor)
            //    {
            //        var param = i.GetParameters();
            //        if(param.Length == 1 && param[0].ParameterType == typeof(EventData))
            //        {
            //            functionNames.Add(i.Name);
            //            Action<EventData> func;
            //            if (i.IsStatic)
            //            {
            //                func = (Action<EventData>)Delegate.CreateDelegate(typeof(Action<EventData>), i);
            //            }
            //            else
            //            {
            //                func = (Action<EventData>)Delegate.CreateDelegate(typeof(Action<EventData>),comp, i);
            //            }
            //            FunctionList.Add(func);
            //        }
            //    }
            //}

            //FunctionNames = functionNames.ToArray();

        }

        public override void OnInspectorGUI()
        {
            Draw();
            var type = target.GetType();
            if(type != typeof(OnEvent))
            {
                this.DrawDefaultInspector(target.GetType());
            }
        }

        public void Draw()
        {
            
            var iter = serializedObject.GetIterator();
            //EditorGUILayout.PropertyField(iter);
            iter.NextVisible(true);
            if(iter.objectReferenceValue != null)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.PropertyField(iter);
            GUI.enabled = true;

            OnEvent comp = target as OnEvent;


            var optionsRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Active");
            optionsRect.width = optionsRect.width / 2;
            comp.Active = EditorGUI.Toggle(optionsRect, " ", comp.Active);
            
            optionsRect.x += optionsRect.width;
            comp.DispatchEvents = EditorGUI.Toggle(optionsRect, "Dispatch Events", comp.DispatchEvents);
            EditorGUILayout.EndHorizontal();

            this.ExposeProperty(ListenEventNameProp);
            this.ExposeProperty(ListenTargetProp);
            //Editor for the OnEventFunc dropdown
            //var functionRect = EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.PrefixLabel("");
            //var index = FunctionList.IndexOf(comp.OnEventFunction);

            //if (index == -1)
            //{
            //    index = 0;
            //}

            //comp.OnEventFunction = FunctionList[EditorGUI.Popup(functionRect, "Target Function", index, FunctionNames)];
            //EditorGUILayout.EndHorizontal();

            if (comp.DispatchEvents)
            {
                this.ExposeProperty(DispatchEventNameProp);
                this.ExposeProperty(DispatchTargetProp);
            }
            
        }
    }
}
#endif