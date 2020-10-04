using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Lavender.Stream
{
    public enum SegmentEvent {
        CollisionEnter, CollisionExit,
        TriggerEnter, TriggerExit 
    }
    
    public class SegmentStreamer :
                        Streamer<SegmentEvent, GameObject, WorldSegment, string>
    {
        // new public delegate void StreamEvent (SegmentEvent Event,
        //                                         GameObject Subject,
        //                                         WorldSegment Object);      
        //new protected static event StreamEvent Publish;  
        
        
        new int Target(StreamEvent Publisher)
        {
            GameObject Target = (GameObject) Publisher.Target;
            
            return Target.GetHashCode();
        }
        
        
        new bool Validate(Subscription<string, SegmentEvent> Sub,
                                                int ReceiverHash, int TargetHash,
                                                GameObject Receiver)
        {
            bool selfMatch, selectedMatch, compMatch;
            
            if (Sub.Mode == SubscriptionMode.All)
                compMatch = true;
            else
            {
                List<string> Tags = Sub.StreamSelectors;
                
                selectedMatch = (Tags?.Contains(Receiver.tag)) ?? false;
                selfMatch = TargetHash == ReceiverHash;
                compMatch = false;
                
                if (Sub.Mode.HasFlag(SubscriptionMode.Self))
                    compMatch |= selfMatch;
                if (Sub.Mode.HasFlag(SubscriptionMode.Selected))
                    compMatch |= selectedMatch;
            }
               
            return compMatch;
        }
        
    }
    
    
    public class StreamerLibrary<TEvent, TSubject, TObject, TStream>
        where TEvent : new()
        where TSubject : new()
        where TObject : new()
        where TStream : new()
    {
        public static readonly object Instance = new Streamer<TEvent, TSubject, TObject, TStream>();
    }
}