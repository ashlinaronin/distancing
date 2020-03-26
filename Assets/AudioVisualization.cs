using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;
    Renderer renderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        float completedRatio = audioSource.time / audioSource.clip.length;
        Vector4 newEmissionColor = new Vector4(completedRatio, 0.8524f, 0f, 1.0f);
        Debug.Log(newEmissionColor);
        renderer.material.SetColor("_EmissionColor", newEmissionColor);
    }
}
