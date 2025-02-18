using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

// Used: https://www.youtube.com/watch?v=BgpqoRFCNOs
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    IObjectPool<SoundEmitter> soundEmitterPool;
    readonly List<SoundEmitter> activeSoundEmitters = new();
    public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();

    [SerializeField] SoundEmitter soundEmitterPrefab;
    [SerializeField] bool collectionCheck = true;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxPoolSize = 100;
    [SerializeField] int maxSoundInstances = 100;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        InitializePool();
    }

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.frequentSound) return true;

        if (FrequentSoundEmitters.Count >= maxSoundInstances && FrequentSoundEmitters.TryDequeue(out var soundEmitter))
        {
            try
            {
                soundEmitter.Stop();
                return true;
            }
            catch
            {
                Debug.Log("SoundEmitter is already released");
            }
            return false;
        }
        return true;
    }

    public SoundEmitter Get()
    {
        return soundEmitterPool.Get();
    }
    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        soundEmitterPool.Release(soundEmitter);
    }

    void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter.gameObject);
    }
    void OnReturnedFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(false);
        activeSoundEmitters.Remove(soundEmitter);
    }

    void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        activeSoundEmitters.Add(soundEmitter);
    }

    SoundEmitter CreateSoundEmitter()
    {
        var soundEmitter = Instantiate(soundEmitterPrefab);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }

    void InitializePool()
    {
        soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedFromPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
            );
    }

    public static void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {

    }
}

[Serializable]
public class SoundData
{
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    public bool loop;
    public bool playOnAwake;
    public bool frequentSound;
}

