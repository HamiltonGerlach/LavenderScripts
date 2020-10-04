using System;
using System.Collections.Generic;
using UnityEngine;


namespace Lavender.Stream
{
    public class Streamer<TEvent, TSubject, TObject, TStream> : MonoBehaviour
    {
        public delegate void StreamEvent (TEvent Event, TSubject Subject, TObject Object);
        protected event StreamEvent Publish;
        protected readonly Dictionary<int, Subscriber<TSubject, TStream, TEvent>> ObjectRegistry
            = new Dictionary<int, Subscriber<TSubject, TStream, TEvent>>(); // HashCode, Subscriber
        public static readonly Streamer<TEvent, TSubject, TObject, TStream> Instance
            = new Streamer<TEvent, TSubject, TObject, TStream>();
        protected Streamer() {}
        
        
        
        public void Broadcast(TEvent Event,
                                    TSubject Receiver,
                                    TObject Object)
        {
            int ReceiverHash = Receiver.GetHashCode();
            
            foreach (StreamEvent p in Publish.GetInvocationList())
            {
                int TargetHash = Target(p);
                
                if (!ObjectRegistry.ContainsKey(TargetHash)) continue;
                
                foreach (var Sub in ObjectRegistry[TargetHash].Subscriptions)
                {
                    if (Validate(Sub, ReceiverHash, TargetHash, Receiver))
                        p?.Invoke(Event, Receiver, Object);
                }
                
                
            }
        }
        
        protected int Target(StreamEvent Publisher) => 0;
                
        protected bool Validate(Subscription<TStream, TEvent> Sub,
                                                int ReceiverHash, int TargetHash,
                                                TSubject Receiver) => false;
        
        public void Subscribe(TSubject Receiver,
                            StreamEvent Handler)
        {
            int Hash = Receiver.GetHashCode();
            
            if (!ObjectRegistry.ContainsKey(Hash))
                ObjectRegistry.Add(Hash,
                                   new Subscriber<TSubject, TStream, TEvent>
                                            (Receiver, SubscriptionMode.Self));
            
            Publish += Handler;
        }
        
        
        public void Subscribe(TSubject Receiver,
                              StreamEvent Handler,
                              SubscriptionMode Mode)
        {
            int Hash = Receiver.GetHashCode();
            
            if (!ObjectRegistry.ContainsKey(Hash))
                ObjectRegistry.Add(Hash,
                                   new Subscriber<TSubject, TStream, TEvent>
                                            (Receiver, Mode));
            
            Publish += Handler;
        }
        
        
        public void Subscribe(TSubject Receiver,
                              StreamEvent Handler,
                              SubscriptionMode Mode,
                              List<TStream> StreamSelectors)
        {
            int Hash = Receiver.GetHashCode();
            
            if (!Mode.HasFlag(SubscriptionMode.Selected))
                Mode &= SubscriptionMode.Selected;
            
            if (!ObjectRegistry.ContainsKey(Hash))
                ObjectRegistry.Add(Hash,
                                   new Subscriber<TSubject, TStream, TEvent>
                                            (Receiver, Mode, StreamSelectors));
            
            Publish += Handler;
        }
        
        
        public void Subscribe(TSubject Receiver,
                              StreamEvent Handler,
                              SubscriptionMode Mode,
                              List<TStream> StreamSelectors,
                              List<TEvent> EventSelectors)
        {
            int Hash = Receiver.GetHashCode();
            
            if (!Mode.HasFlag(SubscriptionMode.Selected))
                Mode &= SubscriptionMode.Selected;
            
            if (!ObjectRegistry.ContainsKey(Hash))
                ObjectRegistry.Add(Hash,
                                   new Subscriber<TSubject, TStream, TEvent>
                                            (Receiver, Mode, StreamSelectors, EventSelectors));
            
            Publish += Handler;
        }
        
                
        public void Subscribe(TSubject Receiver,
                              StreamEvent Handler,
                              SubscriptionMode Mode,
                              TStream StreamSelector,
                              List<TEvent> EventSelectors)
        {
            int Hash = Receiver.GetHashCode();
            
            if (!Mode.HasFlag(SubscriptionMode.Selected))
                Mode &= SubscriptionMode.Selected;
            
            if (!ObjectRegistry.ContainsKey(Hash))
                ObjectRegistry.Add(Hash,
                                   new Subscriber<TSubject, TStream, TEvent>
                                            (Receiver, Mode, StreamSelector, EventSelectors));
            
            Publish += Handler;
        }
        
        
        public void Unsubscribe(TSubject Receiver,
                                StreamEvent Handler,
                                bool Deregister)
        {
            int Hash = Receiver.GetHashCode();
            if (Deregister) ObjectRegistry.Remove(Hash);
            
            Publish -= Handler;
        }
    }
}