using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip firstTrack;    
    public AudioClip secondTrack;   

    void Start()
    {
        StartCoroutine(PlayMusicSequentially());
    }

    IEnumerator PlayMusicSequentially()
    {
       
        audioSource.clip = firstTrack;
        audioSource.Play();
        
        yield return new WaitForSeconds(firstTrack.length);

      
        audioSource.clip = secondTrack;
        audioSource.Play();
    }
}
