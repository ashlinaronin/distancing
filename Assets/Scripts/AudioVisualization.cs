using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;
    Renderer[] childRenderers;

    public int trackNumber;

    public float playedSeconds = 0f;

    bool visited = false;

    public Vector4 startOffset = new Vector4(1f, 0.5f, 0f, 0f);
    public Vector4 endOffset = new Vector4(1f, 0.5f, 0f, 0.5f);

    [Serializable]
    public class TrackListenedEvent : UnityEvent<int,int>{};

    public TrackListenedEvent secondListened;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        var isPlayerClose = CheckCloseToTag("MainCamera", 20);

        // initial visit to this beacon
        if (isPlayerClose && !visited) {
            visited = true;
        }

        // subsequent frames while listening to this beacon
        if (isPlayerClose && visited && playedSeconds <= audioSource.clip.length) {
            // add the number of seconds since the last frame
            playedSeconds += Time.deltaTime;

            secondListened.Invoke(trackNumber, (int)Math.Ceiling(playedSeconds));
            VisualizeListenedProgress();
        }
    }

    private void VisualizeListenedProgress() {
        float t = playedSeconds / audioSource.clip.length;

        foreach (Renderer childRenderer in childRenderers)
        {
            // _BaseMap_ST is a 4d vector containing tiling and offset (for URP/Lit)
            childRenderer.material.SetVector("_BaseMap_ST", Vector4.Lerp(startOffset, endOffset, t));
        }
    }

    public int GetLength() {
        return audioSource ? (int)Math.Ceiling(audioSource.clip.length) : 0;
    }

    bool CheckCloseToTag(string tag, float minimumDistance)
    {
        GameObject[] goWithTag = GameObject.FindGameObjectsWithTag(tag);
    
        for (int i = 0; i < goWithTag.Length; ++i)
        {
            if (Vector3.Distance(transform.position, goWithTag[i].transform.position) <= minimumDistance)
                return true;
        }
    
        return false;
    }
}
