using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioData
{
    public string AudioClipName;
    public AudioClip Clip;
    public float ClipLength;
    public bool Loop;

    public static bool operator ==(AudioData a, string b)
    {
        if(a.AudioClipName == b)
            return true;

        return false;
    }

    public static bool operator !=(AudioData a, string b)
    {
        if(a == b)
            return false;

        return true;
    }
}

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private List<AudioData> _AudioList = new List<AudioData>();
    private AudioSource _Source;
    private bool _ClipShouldPlayer = false;
    private bool _IsPlaying = false;
    public bool IsPlaying { get => _IsPlaying; }
    private AudioData _CurrentlyPlaying;
    private bool Loop;

    private void Awake()
    {
        _Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_ClipShouldPlayer)
        {
            if(!_IsPlaying)
            {
                _IsPlaying = true;
                if(_CurrentlyPlaying.Loop)
                    _Source.loop = true;
                else 
                    _Source.loop = false;

                _Source.clip = _CurrentlyPlaying.Clip;
                _Source.Play();
                StartCoroutine(ResetClip());
            }
        }
    }

    /// <summary>
    /// Plays specified audio clip
    /// </summary>
    /// <param name="clipName">Name of the clip to play</param>
    public void PlayAudioClip(string clipName)
    {
        foreach(var clip in _AudioList)
        {
            if(clip == clipName)
            {
                _CurrentlyPlaying = clip;
                _ClipShouldPlayer = true;
                _IsPlaying = false;
                break;
            }
        }
    }

    public IEnumerator ResetClip()
    {
        yield return new WaitForSeconds(_CurrentlyPlaying.ClipLength);
        _ClipShouldPlayer = false;
        _IsPlaying = false;
    }
}
