using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Space : MonoBehaviour
{
    static public LevelInfo CurrentLevel;
    public LevelInfo LevelInformation;
	// Use this for initialization
	void Awake ()
    {
        
        var obj = GameObject.FindGameObjectWithTag("Space");
        if(obj == null)
        {
            obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Core/Space"));
        }
        
        

        CurrentLevel = LevelInformation;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

[Serializable]
public struct LevelInfo
{
    public string Name;
}