using UnityEngine;
using System.Collections;
using ActionSystem;
using System;

public class PlaySoundOnEvent : EditOnEvent
{
    public AudioClip SoundClip = null;

    public float Delay = 0;

    ActionGroup Grp;
    AudioSource Source;
	// Use this for initialization
    public PlaySoundOnEvent()
    {
        
    }
	public override void Awake()
    {
        base.Awake();
        Grp = this.GetActions();
        Source = this.GetOrAddComponent<AudioSource>();
        Source.ignoreListenerVolume = false;
    }

    public override void OnEventFunc(EventData data)
    {
        if (!Active)
        {
            return;
        }
        
        Source.clip = SoundClip;
        var Seq = ActionSystem.Action.Sequence(Grp);
        if(Delay > 0)
        {
            ActionSystem.Action.Delay(Seq, Delay);
        }
        
        if(SoundClip)
        {
            ActionSystem.Action.Call(Seq, Source.Play);
        }
        else
        {
            ActionSystem.Action.Call(Seq, Source.Stop);
        }
        if(DispatchOnFinish)
        {
            if(SoundClip)
            {
                ActionSystem.Action.Delay(Seq, SoundClip.length);
            }

            ActionSystem.Action.Call(Seq, DispatchEvent);
        }
        EditChecks(Seq);
    }
}

[Serializable]
public struct AudioSettings
{

}
