using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioPlayerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlaySound(AudioClip clip, Vector2 position)
    {
        if(Instance.audioPlayerPrefab)
        {
            GameObject obj = ObjectPooler.GetPooledObject(Instance.audioPlayerPrefab.gameObject);
            obj.transform.position = position;

            AudioSource source = obj.GetComponent<AudioSource>();
            if(source)
            {
                source.clip = clip;

                source.volume = 1;
                source.pitch = 1;

                source.Play();

                Instance.StartCoroutine(Instance.ReturnToPool(obj, clip.length));
            }
        }
    }

    public static void PlaySound(AudioEvent audio, Vector2 position)
    {
        if (Instance.audioPlayerPrefab)
        {
            GameObject obj = ObjectPooler.GetPooledObject(Instance.audioPlayerPrefab.gameObject);
            obj.transform.position = position;

            AudioSource source = obj.GetComponent<AudioSource>();
            if (source)
            {
                source.clip = audio.clip;

                source.volume = audio.volume;
                source.pitch = Random.Range(audio.pitchMin, audio.pitchMax);

                source.Play();

                Instance.StartCoroutine(Instance.ReturnToPool(obj, audio.clip.length));
            }
        }
    }

    IEnumerator ReturnToPool(GameObject audioObj, float time)
    {
        yield return new WaitForSeconds(time);

        audioObj.SetActive(false);
    }
}

[System.Serializable]
public class AudioEvent
{
    public AudioClip clip;
    public float pitchMin = 1;
    public float pitchMax = 1;
    public float volume = 1;

    public void Play(Vector2 position)
    {
        SoundManager.PlaySound(this, position);
    }
}