using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindFunction : MonoBehaviour
{
    // gameManager variables
    public GameObject GameManager;
    // The struct State stores a snapshot of the object's state at a certain time.
    private struct State
    {
        public float timestamp;
        public Vector3 position;
        public Quaternion rotation;

        // The constructor for the State struct.
        public State(float timestamp, Vector3 position, Quaternion rotation)
        {
            this.timestamp = timestamp;
            this.position = position;
            this.rotation = rotation;
        }
    }

    // Public flags to control rewinding and recording states
    public bool isRewinding = false;
    public bool shouldRecord = true;

    // A list of past states, stored in order to provide the rewind functionality.
    private List<State> states = new List<State>();

    // Update is called once per frame in Unity
    void Update()
    {
        // If we're rewinding, we run the Rewind method, otherwise we run Record method.
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    // The Record method stores the object's current state if it's not currently rewinding.
    private void Record()
    {
        // If the list of states is too long, remove the oldest one and add a new state at the beginning.
        if (!isRewinding && states.Count > Mathf.Round(GameManager.GetComponent<GameManager>().inputTime*5 / Time.fixedDeltaTime))
        {
            //Debug.Log("Recording too long, deleting oldest state");
            states.RemoveAt(states.Count - 1);
        }

        // Add a new state to the beginning of the list, storing the current time, position, and rotation.
        if (!isRewinding && shouldRecord)
        {
            //Debug.Log("Recording");
            states.Insert(0, new State(Time.time, transform.position, transform.rotation));
        }
    }

    // The Rewind method sets the object's state back to the most recent recorded state.
    private void Rewind()
    {
        //Debug.Log("Rewinding");
        // If there's a state to rewind to, use it to set the current position and rotation, and then remove it from the list.
        if (states.Count > 0)
        {
            float rewindSpeed = GameManager.GetComponent<GameManager>().inputTime - GameManager.GetComponent<GameManager>().countdownTime;
            int rewindSpeedInt = Mathf.RoundToInt(rewindSpeed);

            State state = states[0];
            transform.position = state.position;
            transform.rotation = state.rotation;
            states.RemoveRange(0, Mathf.Min(rewindSpeedInt, states.Count)); // Skip 4 additional states if they exist
        }
        // If there are no states left to rewind to, stop the rewind process.
        else
        {
            StopRewind();
        }
    }

    // The StartRewind method allows the rewind process to start.
    public void StartRewind()
    {
        isRewinding = true;
    }

    // The StopRewind method allows the rewind process to stop.
    public void StopRewind()
    {
        isRewinding = false;
    }
}
