using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public int totalTracks = 9;
    public List<int> playedTracks = new List<int>();

    [Serializable]
    public class StringEvent : UnityEvent<string>{};

    public StringEvent updateDisplay;

    public void SetTrackPlayed(int trackNumber)
    {
        playedTracks.Add(trackNumber);
        SetDisplayMessage();
    }

    private void SetDisplayMessage() {
        int tracksRemaining = totalTracks - playedTracks.Count;
        string message = tracksRemaining > 0 ? tracksRemaining.ToString() : "Thank you for listening.";
        updateDisplay.Invoke(message);
    }
}
