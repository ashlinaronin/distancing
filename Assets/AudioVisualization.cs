using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;
    Renderer[] childRenderers;

    bool visited = false;

    public int fadeSeconds = 4;

    public Color startColor;
    public Vector4 endColor = new Vector4(1f, 0.8524641f, 0f, 0f);

    public UnityEvent trackPlayed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        childRenderers = GetComponentsInChildren<Renderer>();
        startColor = renderer.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        var isPlayerClose = CheckCloseToTag("MainCamera", 20);
        if (isPlayerClose && !visited) {
            StartCoroutine(VisitedFade());
            visited = true;
            trackPlayed.Invoke();
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
