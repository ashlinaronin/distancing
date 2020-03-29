using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;
    Renderer renderer;

    bool visited = false;

    public int fadeSeconds = 4;

    public Color startColor;
    public Vector4 endColor = new Vector4(1f, 0.8524641f, 0f, 0f);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponentInChildren<Renderer>();
        startColor = renderer.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        var isPlayerClose = CheckCloseToTag("MainCamera", 20);
        if (isPlayerClose && !visited) {
            StartCoroutine(VisitedFade());
            visited = true;
        }
    }

    IEnumerator VisitedFade()
    {
        float t = 0f;

        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            renderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, t / fadeSeconds));
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
