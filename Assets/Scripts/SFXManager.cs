using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] int _nAudiosources = 10;
    [SerializeField] bool _expandSize = true;

    public static SFXManager Instance { get; private set; }

    Queue<AudioSource> _sources;
    Dictionary<int, AudioSource> _activeSources;
    Dictionary<int, Coroutine> _activeCoroutines;
    Dictionary<int, int> _sequentialActiveSources;

    public float GetSFXVolume(int id)
    {
        return _activeSources[id].volume;
    }

    public void SetSFXVolume(int id, float volume) {
        _activeSources[id].volume = volume;
    }

    //Returns ID for SFX to adjust later
    public int PlayLoopingSFX(AudioClip clip, float volume = 1f) {
        return PlaySFXInternal(clip, volume, true);
    }

    //Returns ID for SFX to adjust later
    public int PlaySFX(AudioClip clip, float volume = 1f)
    {
        return PlaySFXInternal(clip, volume, false);
    }

    public void StopSFX(int id)
    {
        AudioSource src;
        if (_activeSources.TryGetValue(id, out src))
        {
            src.Stop();
            ResetSource(src);
        }
    }

    public void StopAllSFX() {
        foreach(int key in _activeSources.Keys)
        {
            StopSFX(key);
        }
    }

    public int PlaySequentialSFX(AudioClip[] clips, float gap = 0f, System.Action callback = null) {
        int id = clips.GetHashCode();
        Coroutine routine = StartCoroutine(PlaySequentialSFXInteral(id, clips, gap, callback));
        _activeCoroutines[id] = routine;
        return id;
    }

    //Will not stop SFX dead, just prevent it from continuing
    public bool StopSequentialSFX(int id) {
        Coroutine routine;
        if (_activeCoroutines.TryGetValue(id, out routine))
        {
            StopSFX(_sequentialActiveSources[id]);
            StopCoroutine(_activeCoroutines[id]);
            _activeCoroutines.Remove(id);
            _sequentialActiveSources.Remove(id);
            return true;
        }
        return false;
    }

    IEnumerator PlaySequentialSFXInteral(int id, AudioClip[] clips, float gap = 0f, System.Action callback = null) {
        for (int n = 0; n < clips.Length; n++) {
            _sequentialActiveSources[id] = PlaySFX(clips[n]);
            PlaySFX(clips[n]);
            yield return new WaitForSeconds(clips[n].length + gap);
        }
        _sequentialActiveSources.Remove(id);
        callback?.Invoke();
        _activeCoroutines.Remove(id);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _sources = new Queue<AudioSource>();
        _activeSources = new Dictionary<int, AudioSource>();
        _activeCoroutines = new Dictionary<int, Coroutine>();
        _sequentialActiveSources = new Dictionary<int, int>();
        for (int n = 0; n < _nAudiosources; n++)
        {
            AddAudioSource();
        }
    }

    int PlaySFXInternal(AudioClip clip, float volume, bool loop) {
        if (!FreeSourceAvailable())
        {
            Debug.LogError($"Cannot play SFX {clip.name}; out of sources and no expansion");
            return -1;
        }
        AudioSource src = _sources.Dequeue();
        src.loop = loop;
        src.clip = clip;
        src.volume = volume;
        int id = src.GetInstanceID();
        _activeSources.Add(id, src);
        src.Play();
        if (!loop)
        {
            _activeCoroutines.Add(id, StartCoroutine(ResetSourceWhenComplete(src)));
        }
        return id;
    }

    IEnumerator ResetSourceWhenComplete(AudioSource src)
    {
        yield return new WaitForSeconds(src.clip.length);
        ResetSource(src);
    }

    void ResetSource(AudioSource src) {
        src.clip = null;
        src.loop = false;
        _sources.Enqueue(src);
        int id = src.GetInstanceID();
        _activeSources.Remove(id);

        Coroutine routine;
        if (_activeCoroutines.TryGetValue(id, out routine))
        {
            StopCoroutine(routine);
            _activeCoroutines.Remove(id);
        }
    }

    bool FreeSourceAvailable()
    {
        if (_sources.Count == 0)
        {
            if (_expandSize)
            {
                AddAudioSource();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    void AddAudioSource() {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        _sources.Enqueue(newSource);
    }
}
