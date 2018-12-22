// 참고한 링크: http://www.kaappine.fi/tutorials/using-microphone-input-in-unity3d/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
    private string microphone;          // microphone name

    private AudioSource audio;
    private List<string> allMicrophones = new List<string>();

    public float sensitivity = 100;         // for tweaking the output value
    public float loudness = 0;                 // for storing the average volume for further use

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>(); 

        for(int i = 0;  i < Microphone.devices.Length; i++)
        {
            if (microphone == null)
            {
                microphone = Microphone.devices[i];
            }

            allMicrophones.Add(Microphone.devices[i]);
        }

        // Start Microphone
        audio.clip = Microphone.Start(microphone, true, 10, 44100);
        audio.loop = true;      // Set the AudioClip to loop
        audio.mute = false;     // (true): Mute the sound, we don't want the player to hear it

        if (Microphone.IsRecording(microphone))
        {
            while (!(Microphone.GetPosition(microphone) > 0)) { }     // Wait until the recording has started
            Debug.Log("Playing..");
            audio.Play();   // Play the audio source
        }
        else
        {
            Debug.Log("Fail..");
        }
	}
	
	// Update is called once per frame
	void Update () {
        loudness = GetAveragedVolume() * sensitivity;
        Debug.Log(loudness.ToString());
	}

    float GetAveragedVolume()
    {
        // sample size = 256
        int sampleSize = 256;
        float[] data = new float[sampleSize];
        float a = 0;
        audio.GetOutputData(data, 0);

        foreach(float s in data)
        {
            a += Mathf.Abs(s);
        }

        // return average volume (sum / samplesize)
        return a / sampleSize;
    }
}
