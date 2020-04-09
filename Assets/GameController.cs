using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public int totalTracks = 9;
    public List<int> playedTracks = new List<int>();

    public Dictionary<int,HashSet<int>> playedMinutes = new Dictionary<int,HashSet<int>>();

    [Serializable]
    public class StringEvent : UnityEvent<string>{};

    public StringEvent updateDisplay;

    public void SetTrackPlayed(int trackNumber)
    {
        playedTracks.Add(trackNumber);
        SetDisplayMessage();
    }

    public void SetMinutePlayed(int trackNumber, int minutePlayed)
    {
        if (!playedMinutes.ContainsKey(trackNumber)) {
            playedMinutes[trackNumber] = new HashSet<int>();
        }

        playedMinutes[trackNumber].Add(minutePlayed);
        SetDisplayMessage();
        // Debug.Log(playedMinutes[trackNumber].ToArray());
        //Debug.Log(string.Join("", playedMinutes[trackNumber]));
    }

    private void SetDisplayMessage() {
        int totalMinutes = 0;

        foreach (KeyValuePair<int,HashSet<int>> track in playedMinutes)
        {
            foreach (int minute in track.Value)
            {
                Debug.Log("minute" + " " + minute);
                totalMinutes++;
            }
        }

        Debug.Log(totalMinutes);

        string message = totalMinutes.ToString();
        updateDisplay.Invoke(message);

        // int tracksRemaining = totalTracks - playedTracks.Count;
        // string message = tracksRemaining > 0 ? tracksRemaining.ToString() : "Thank you for listening.";
        // updateDisplay.Invoke(message);
    }
}
