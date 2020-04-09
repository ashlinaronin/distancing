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

    bool visited = false;

    public int fadeSeconds = 4;

    public Color startColor;
    public Color endColor = new Color(0f, 0f, 0f, 0f);


    // if we want to store these values for other processing, we should use a different event type
    // but for now, let's just pass the display value directly to the ui text
    [Serializable]
    public class TrackListenedEvent : UnityEvent<string>{};

    [Serializable]
    public class MinuteListenedEvent : UnityEvent<int,int>{};

    public UnityEvent trackPlayed;

    public TrackListenedEvent trackListened;

    public MinuteListenedEvent minuteListened;

    void Start()
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
            StartCoroutine(VisitedFade());
            visited = true;
            trackPlayed.Invoke();
        }

        // subsequent frames while listening to this beacon
        if (isPlayerClose && visited) {
            int currentTime = (int)Math.Ceiling(audioSource.time);
            int length = (int)Math.Ceiling(audioSource.clip.length);

            trackListened.Invoke($"{currentTime} / {length}s");
            minuteListened.Invoke(trackNumber, currentTime);
        }
    }

    IEnumerator VisitedFade()
    {
        float t = 0f;

        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            
            foreach (Renderer childRenderer in childRenderers)
            {
                childRenderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, t / fadeSeconds));
            }

            yield return null;
        }
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
