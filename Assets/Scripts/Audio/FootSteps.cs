using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }
    
    private void Step()
    {
        AudioClip clip = _clips[UnityEngine.Random.Range(0, _clips.Length)];
        _audioSource.PlayOneShot(clip);
    }
}
