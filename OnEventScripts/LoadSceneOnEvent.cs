using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnEvent : OnEvent
{
    public string SceneName;
    
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        
	}

    public override void OnEventFunc(EventData data)
    {
        if(SceneName == "")
        {
            return;
        }
        //LoadSceneMode.)
        //var scene = SceneManager.GetActiveScene();
        //option to destroy main camera.
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        //make each scene its own render layer
        //Debug.Log(SceneManager.GetSceneByName("TestLoadScene").buildIndex);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
