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
            StartCoroutine(VisitedFade());
            visited = true;
        }

        // subsequent frames while listening to this beacon
        if (isPlayerClose && visited) {
            int currentTime = (int)Math.Ceiling(audioSource.time);
            int length = (int)Math.Ceiling(audioSource.clip.length);

            minuteListened.Invoke(trackNumber, currentTime);
        }
    }

    public int GetLength() {
        return audioSource ? (int)Math.Ceiling(audioSource.clip.length) : 0;
    }

// then tie it to the minutes/total listened instead of fadeSeconds
    IEnumerator VisitedFade()
    {
        float t = 0f;

        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            
            foreach (Renderer childRenderer in childRenderers)
            {
                // _BaseMap_ST is a 4d vector containing tiling and offset (for URP/Lit)
                childRenderer.material.SetVector("_BaseMap_ST", Vector4.Lerp(startOffset, endOffset, t / fadeSeconds));

                // also fade emission color
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
