using UnityEngine;
using System.Collections;
using System;
using ActionSystem;

[Serializable]
public class MegaButtonSounds : MegaComponent
{
    [ExposeProperty]
    public AudioClip MouseEnterSound
    {
        get
        {
            return MouseEnterSoundComp.SoundClip;
        }
        set
        {
            MouseEnterSoundComp.SoundClip = value;
        }
    }

    [ExposeProperty]
    public AudioClip MouseExitSound
    {
        get
        {
            return MouseExitSoundComp.SoundClip;
        }
        set
        {
            MouseExitSoundComp.SoundClip = value;
        }
    }

    [ExposeProperty]
    public AudioClip MouseUpSound
    {
        get
        {
            return MouseUpSoundComp.SoundClip;
        }
        set
        {
            MouseUpSoundComp.SoundClip = value;
        }
    }

    [ExposeProperty]
    public AudioClip MouseDownSound
    {
        get
        {
            return MouseDownSoundComp.SoundClip;
        }
        set
        {
            MouseDownSoundComp.SoundClip = value;
        }
    }

    [MegaRegister]
    public PlaySoundOnEvent MouseEnterSoundComp;
    [MegaRegister]
    public PlaySoundOnEvent MouseExitSoundComp;
    [MegaRegister]
    public PlaySoundOnEvent MouseUpSoundComp;
    [MegaRegister]
    public PlaySoundOnEvent MouseDownSoundComp;
    // Use this for initialization
    public override void Awake ()
    {
        base.Awake();
        var reactive = this.GetOrAddComponent<Reactive>();
        reactive.MouseEvents = true;
        MouseEnterSoundComp.ListenEventName = Events.MouseEnter;
        MouseExitSoundComp.ListenEventName = Events.MouseExit;
        MouseUpSoundComp.ListenEventName = Events.MouseUpAsButton;
        MouseDownSoundComp.ListenEventName = Events.MouseDown;
    }
}