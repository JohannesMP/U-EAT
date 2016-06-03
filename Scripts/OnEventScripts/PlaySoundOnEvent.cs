using UnityEngine;
using System.Collections;
using ActionSystem;
public class PlaySoundOnEvent : EditOnEvent
{
    public AudioClip SoundClip = null;

    public float Delay = 0;

    ActionGroup Grp;
    AudioSource Source;
	// Use this for initialization
	public override void Awake()
    {
        base.Awake();
        Grp = this.GetActions();
        Source = this.GetOrAddComponent<AudioSource>();
        Source.ignoreListenerVolume = false;
    }

    public override void OnEventFunc(EventData data)
    {
        if(!Active)
        {
            return;
        }

        Source.clip = SoundClip;
        var Seq = Action.Sequence(Grp);
        Action.Delay(Seq, Delay);
        if(SoundClip)
        {
            Action.Call(Seq, Source.Play);
        }
        else
        {
            Action.Call(Seq, Source.Stop);
        }
        if(DispatchOnFinish)
        {
            if(SoundClip)
            {
                Action.Delay(Seq, SoundClip.length);
            }
            
            Action.Call(Seq, DispatchEvent);
        }
        EditChecks(Seq);
    }
}
