using UnityEngine;
using System.Collections;
using System;

public class MegaComponent : MonoBehaviour
{
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
        public MegaRegister(bool hideInInsepctor = true)
        {
            HideInInspector = hideInInsepctor;
        }
    }
}


#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;
    using System.Collections.Generic;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(MegaComponent), true)]
    public class MegaComponentEditor : Editor
    {
        protected Behaviour[] ComponentList = null;
        bool Confirmation = false;
        public virtual void OnEnable()
        {
            var obj = (target as MegaComponent).gameObject;
            var type = target.GetType();
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
                    SerializedProperty compProp = serializedObject.FindProperty(i.Name);
                    
                    if (compProp.objectReferenceValue == null)
                    {
                        var comp = obj.AddComponent(i.FieldType);
                        compProp.objectReferenceValue = comp;
                    }
                    serializedObject.ApplyModifiedProperties();
                    componentList.Add(compProp.objectReferenceValue as Behaviour);
                }
            }
            ComponentList = componentList.ToArray();
            foreach (var comp in ComponentList)
            {
                comp.hideFlags = HideFlags.HideInInspector;
            }
        }
        public override void OnInspectorGUI()
        {
            foreach(var comp in ComponentList)
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
                    ReleaseComponents();
                }
                if (GUILayout.Button("No"))
                {
                    Confirmation = false;
                }
            }
        }

        public void ReleaseComponents()
        {
            foreach (var i in ComponentList)
            {
                i.hideFlags = HideFlags.None;
            }
            DestroyImmediate(target);
        }
    }
}
#endif