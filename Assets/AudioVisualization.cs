using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        //    Debug.Log(audioSource.time);
        //    Debug.Log(audioSource.clip.length);
        var percentCompleted = audioSource.time / audioSource.clip.length;
        Debug.Log(percentCompleted * 100 + "%");
    }
}
