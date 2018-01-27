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
                source.Play();

                Instance.StartCoroutine(Instance.ReturnToPool(obj, clip.length));
            }
        }
    }

    IEnumerator ReturnToPool(GameObject audioObj, float time)
    {
        yield return new WaitForSeconds(time);

        audioObj.SetActive(false);
    }
}
