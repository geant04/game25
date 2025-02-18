using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public SoundData Data { get; private set; }
    AudioSource audioSource;
    Coroutine playingCoroutine;

    public void Play()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
        }

        audioSource.Play();
        playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        SoundManager.Instance.ReturnToPool(this);
    }

    public void Stop()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }

        audioSource.Stop();
        SoundManager.Instance.ReturnToPool(this);
    }

    public void Initialize(SoundData data)
    {
        Data = data;
        audioSource.clip = data.clip;
        audioSource.outputAudioMixerGroup = data.mixerGroup;
        audioSource.loop = data.loop;
        audioSource.playOnAwake = data.playOnAwake;
    }

    public void WithRandomPitch(float min = -0.01f, float max = 0.01f)
    {
        audioSource.pitch += Random.Range(min, max);
    }
}

