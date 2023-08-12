using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {  get; private set; }

    private List<EventInstance> eventInstances;
    
    private List<StudioEventEmitter> eventEmitters;
    
    private EventInstance ambienceEventInstance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emmiterGameObject)
    {
        StudioEventEmitter emmiter = emmiterGameObject.GetComponent<StudioEventEmitter>();
        emmiter.EventReference = eventReference;
        eventEmitters.Add(emmiter);
        return emmiter;
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.Rambience);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emmiters because if we don't they may hang around in other scenes
        foreach(StudioEventEmitter emiiter in eventEmitters)
        {
            emiiter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
