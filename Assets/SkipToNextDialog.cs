using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkipToNextDialog : MonoBehaviour
{
    public UnityEvent onSceneSkip;
    private TimelineAsset _timeline;
    private PlayableDirector _director;
    private List<TimelineClip> _dialogClips;
    private TimelineClip _nextClip = null;
    

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _timeline = (TimelineAsset) _director.playableAsset;
        IEnumerable tracks = _timeline.GetOutputTracks();

        foreach (TrackAsset track in tracks)
        {
            if (track.name == "OldManDialog")
            {
                _dialogClips = track.GetClips().ToList();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_nextClip == null) return;
            _director.time = _nextClip.start;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // skip to next scene.
            onSceneSkip.Invoke();
        }
    }

    public void PlayNextDialog()
    {
        TimelineClip currentClip = null;
        // Get current time and check to see what clip we just started
        double time = _director.time;
        foreach (TimelineClip clip in _dialogClips)
        {
            if (clip.start <= time && time <= clip.end)
            {
                currentClip = clip;
            }
        }
        

        if (currentClip == null) return;
        
        // Get the next clip
        for (int i = 0; i < _dialogClips.Count; i++)
        {
            if (_dialogClips[i] == currentClip)
            {
                _nextClip = i + 1 == _dialogClips.Count ? null : _dialogClips[i + 1];
            }
        }
        
        //Debug.Log(_nextClip.start);
        
    }
    
    
}
