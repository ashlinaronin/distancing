using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;
    Renderer renderer;

    bool visited = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        renderer.material.SetColor("_EmissionColor", new Vector4(visited ? 1.0f : 0.0f, 0.8524f, 0f, 1.0f));

        var isPlayerClose = CheckCloseToTag("MainCamera", 20);
        if (isPlayerClose) {
            visited = true;
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
