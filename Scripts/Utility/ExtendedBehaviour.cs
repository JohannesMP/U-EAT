using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExtendedBehaviour : MonoBehaviour
{

}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true, isFallback = false)]
public class ExtendedBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomInspector.CustomInspectorScripts.DrawDefaultInspector(this, target.GetType(), true);
    }
}
#endif

