using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
[ExecuteInEditMode]
public class MegaComponent : MonoBehaviour
{
    public Behaviour[] ComponentList { get; protected set; }

    //[ExposeProperty]
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

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    protected internal class MegaRegister : Attribute
    {
        public bool HideInInspector { get; private set; }
        public MegaRegister(bool hideInInspector = true)
        {
            HideInInspector = hideInInspector;
        }
    }

    public virtual void Awake()
    {
        ComponentList = GenerateComponents().ToArray();
        
    }

    public void OnDestroy()
    {
#if UNITY_EDITOR
        if ((Game.GameWasPlaying && !Application.isPlaying))
        {
            foreach (var i in ComponentList)
            {
                Destroy(i);
            }
        }
        else if(!Game.GameWasPlaying && Application.isPlaying)
        {
            foreach (var i in ComponentList)
            {
                DestroyImmediate(i);
            }
        }
#else
        foreach (var i in ComponentList)
        {
            Destroy(i);
        }
#endif
    }

    public void ReleaseComponents()
    {
        foreach (var i in ComponentList)
        {
            i.hideFlags = HideFlags.None;
        }
        ComponentList = new List<Behaviour>().ToArray();
        DestroyImmediate(this);
    }

    public List<Behaviour> GenerateComponents()
    {
        var type = GetType();
        List<Behaviour> componentList = new List<Behaviour>();
        foreach (var i in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (i.FieldType.IsDerivedFrom(typeof(Behaviour)))
            {
                var attributes = i.GetCustomAttributes(typeof(MegaComponent.MegaRegister), true);
                if (attributes.Length == 0)
                {
                    continue;
                }
                Behaviour compProp = (Behaviour)i.GetValue(this);
                if (compProp == null)
                {
                    compProp = gameObject.AddComponent(i.FieldType) as Behaviour;
                    i.SetValue(this, compProp);
                }
                componentList.Add(compProp);
            }
        }
        foreach (var comp in componentList)
        {
            comp.hideFlags = HideFlags.HideInInspector;
        }
        return componentList;
    }

    public virtual void Awake()
    {
        ComponentList = GenerateComponents().ToArray();
        
    }

    public void OnDestroy()
    {
        if ((Game.GameWasPlaying && !Application.isPlaying))
        {
            foreach (var i in ComponentList)
            {
                Destroy(i);
            }
        }
        else if(!Game.GameWasPlaying && Application.isPlaying)
        {
            foreach (var i in ComponentList)
            {
                DestroyImmediate(i);
            }
        }
    }

    public void ReleaseComponents()
    {
        foreach (var i in ComponentList)
        {
            i.hideFlags = HideFlags.None;
        }
        ComponentList = new List<Behaviour>().ToArray();
        DestroyImmediate(this);
    }

    public List<Behaviour> GenerateComponents()
    {
        var type = GetType();
        List<Behaviour> componentList = new List<Behaviour>();
        foreach (var i in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (i.FieldType.IsDerivedFrom(typeof(Behaviour)))
            {
                var attributes = i.GetCustomAttributes(typeof(MegaComponent.MegaRegister), true);
                if (attributes.Length == 0)
                {
                    continue;
                }
                Behaviour compProp = (Behaviour)i.GetValue(this);
                if (compProp == null)
                {
                    compProp = gameObject.AddComponent(i.FieldType) as Behaviour;
                    i.SetValue(this, compProp);
                }
                componentList.Add(compProp);
            }
        }
        foreach (var comp in componentList)
        {
            comp.hideFlags = HideFlags.HideInInspector;
        }
        return componentList;
    }
}


#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(MegaComponent), true)]
    public class MegaComponentEditor : Editor
    {
        protected MegaComponent Comp = null;
        bool Confirmation = false;

        public virtual void OnEnable()
        {
            Comp = (target as MegaComponent);
        }
        public override void OnInspectorGUI()
        {
            foreach(var comp in Comp.ComponentList)
            {
                comp.enabled = (target as MegaComponent).enabled;
            }
            this.DrawDefaultInspector(typeof(MegaComponent), true);
            this.DrawDefaultInspector(target.GetType(), false);
            if (!Confirmation)
            {
                Confirmation = GUILayout.Button("Release Components");
            }

            if (Confirmation)
            {
                GUILayout.Label("Are you sure you want to break up this MegaComponent?");
                if (GUILayout.Button("Yes"))
                {
                    Comp.ReleaseComponents();
                }
                if (GUILayout.Button("No"))
                {
                    Confirmation = false;
                }
            }
        }
    }
}
#endif