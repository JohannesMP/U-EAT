using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnEvent : OnEvent
{
#if UNITY_EDITOR
    [ExposeProperty]
    public UnityEditor.SceneAsset SceneToLoad
    {
        get
        {
            return SceneToLoadProp;
        }
        set
        {
            SceneToLoadProp = value;
            if (value)
            {
                SceneName = SceneToLoad.name;
            }
            else
            {
                SceneName = "";
            }
        }
    }
    [SerializeField]
    UnityEditor.SceneAsset SceneToLoadProp;
#endif
    [SerializeField]
    string SceneName;
    public LoadSceneMode SceneMode = LoadSceneMode.Single;

    public override void OnEventFunc(EventData data)
    {
        if (SceneName == "")
        {
            return;
        }
        //LoadSceneMode.)
        //var scene = SceneManager.GetActiveScene();
        //option to destroy main camera.
        SceneManager.LoadScene(SceneName, SceneMode);
        //make each scene its own render layer
        //Debug.Log(SceneManager.GetSceneByName("TestLoadScene").buildIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
