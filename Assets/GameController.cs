using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public int totalTracks = 9;

    private int totalSecondsAvailable = 0;
    public List<int> playedTracks = new List<int>();

    public Dictionary<int,HashSet<int>> playedSeconds = new Dictionary<int,HashSet<int>>();

    [Serializable]
    public class StringEvent : UnityEvent<string>{};

    public StringEvent updateDisplay;

    void Start()
    {
        var allAudioBeacons = FindObjectsOfType<AudioVisualization>();
        Debug.Log(allAudioBeacons + " : " + allAudioBeacons.Length);
        
        foreach (AudioVisualization audioBeacon in allAudioBeacons)
        {
            totalSecondsAvailable += audioBeacon.GetLength();
            Debug.Log("totalSeconds: " + totalSecondsAvailable);
        }
        SetDisplayMessage();
    }

    public void SetTrackPlayed(int trackNumber)
    {
        playedTracks.Add(trackNumber);
        SetDisplayMessage();
    }

    public void SetMinutePlayed(int trackNumber, int minutePlayed)
    {
        if (!playedSeconds.ContainsKey(trackNumber)) {
            playedSeconds[trackNumber] = new HashSet<int>();
        }

        playedSeconds[trackNumber].Add(minutePlayed);
        SetDisplayMessage();
    }

    private void SetDisplayMessage() {
        int totalSecondsPlayed = 0;

        foreach (KeyValuePair<int,HashSet<int>> track in playedSeconds)
        {
            foreach (int minute in track.Value)
            {
                totalSecondsPlayed++;
            }
        }

        Debug.Log(totalSecondsPlayed);

        Debug.Log("available now: " + totalSecondsAvailable.ToString());

        string message = $"{totalSecondsPlayed.ToString()} / {totalSecondsAvailable.ToString()}";
        updateDisplay.Invoke(message);

        // int tracksRemaining = totalTracks - playedTracks.Count;
        // string message = tracksRemaining > 0 ? tracksRemaining.ToString() : "Thank you for listening.";
        // updateDisplay.Invoke(message);
    }
}
