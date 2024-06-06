using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFX : CanPause
{
    [SerializeField] private AudioSource source;

    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    public void Play()
    {
        source.Play();
        isPlaying = true;
    }

    public void DestroyAfterPlay()
    {
        StartCoroutine(Playing());
    }

    private IEnumerator Playing()
    {
        source.Play();

        yield return new WaitForSeconds(source.clip.length);
        
        Destroy(gameObject);
    }

    public void Stop()
    {
        source.Stop();
        isPlaying = false;
    }
}
