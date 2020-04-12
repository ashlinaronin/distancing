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

    public HashSet<int> playedSeconds = new HashSet<int>();

    bool visited = false;

    public int fadeSeconds = 4;

    public Color startColor;
    public Color endColor = new Color(0f, 0f, 0f, 0f);
    public Vector4 startOffset = new Vector4(1f, 0.5f, 0f, 0f);
    public Vector4 endOffset = new Vector4(1f, 0.5f, 0f, 0.5f);

    [Serializable]
    public class MinuteListenedEvent : UnityEvent<int,int>{};

    public MinuteListenedEvent minuteListened;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        childRenderers = GetComponentsInChildren<Renderer>();

        // for our purposes, we will assume both children have the same material/color
        startColor = childRenderers[0].material.GetColor("_EmissionColor");
    }

    void Update()
    {
        var isPlayerClose = CheckCloseToTag("MainCamera", 20);

        // initial visit to this beacon
        if (isPlayerClose && !visited) {
            visited = true;
        }

        // subsequent frames while listening to this beacon
        if (isPlayerClose && visited) {
            int currentTimeRounded = (int)Math.Ceiling(audioSource.time);
            int lengthRounded = (int)Math.Ceiling(audioSource.clip.length);

            playedSeconds.Add(currentTimeRounded);

            // todo: eventually refactor to just send all the minutes from each beacon so we don't track it in two places?
            minuteListened.Invoke(trackNumber, currentTimeRounded);
            VisualizeListenedProgress(audioSource.time, audioSource.clip.length);
        }
    }

    // todo: now fade is jerky because we are only tracking minutes  
    // probably could then also refactor the way we send events to the game controller, but that can wait til after it's working
    private void VisualizeListenedProgress(float currentTime, float length) {
        float t = playedSeconds.Count / length;

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
