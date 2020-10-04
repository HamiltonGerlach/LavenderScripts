using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lavender.Stream;

public class NpcObject : MonoBehaviour
{    
    public void OnEnable()
        => SegmentStreamer.Subscribe(this.gameObject, OnWorldSegmentEvent, SubscriptionMode.Self);
    public void OnDisable()
        => SegmentStreamer.Unsubscribe(this.gameObject, OnWorldSegmentEvent, true);
    
    public void OnWorldSegmentEvent(SegmentEvent Event, GameObject Reporter, WorldSegment Segment)
    {
        if (Event == SegmentEvent.TriggerEnter)
            OnWorldSegmentEnter(Segment, Reporter);
        else
            OnWorldSegmentExit(Segment, Reporter);
    }
    public void OnWorldSegmentEnter(WorldSegment Segment, GameObject Reporter)
    {
        // Log("Entered " + seg.name);
    }
    public void OnWorldSegmentExit(WorldSegment Segment, GameObject Reporter)
    {
        
        // Log("Exited " + seg.name);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
