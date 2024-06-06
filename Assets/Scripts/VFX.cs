using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private float playDuration;

    private void Awake()
    {
        vfx.gameObject.SetActive(false);
    }

    public void Play()
    {
        vfx.gameObject.SetActive(true);
        vfx.Play();
    }

    public void PlayOnce()
    {
        StartCoroutine(Playing());
    }

    private IEnumerator Playing()
    {
        Play();
        yield return new WaitForSeconds(playDuration);
        Stop();
    }

    public void Stop()
    {
        vfx.gameObject.SetActive(false);
    }
}
