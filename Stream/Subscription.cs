using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Lavender.Stream
{
    [Flags] public enum SubscriptionMode {
        All = 0b00, Selected = 0b10, Self = 0b01,
        Selective = Selected | Self
    }
    
     
    public class Subscription<TStream, TEvent> {
        public SubscriptionMode Mode;
        public List<TStream>? StreamSelectors;
        public List<TEvent>? EventSelectors;
        public int DelegateHash;
        
        
        public Subscription(int delegateHash = 0) {
            Mode = SubscriptionMode.Self;
            StreamSelectors = null;
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = null;
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            TStream streamSelector,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = new List<TStream>((IEnumerable<TStream>?) streamSelector);
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            List<TStream> streamSelectors,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = streamSelectors;
            DelegateHash = delegateHash;
        }
        
        
        
        public Subscription(SubscriptionMode mode,
                            TEvent eventSelector,
                            int delegateHash = 0) {
            Mode = mode;
            EventSelectors = new List<TEvent>((IEnumerable<TEvent>?) eventSelector);
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            List<TEvent> eventSelectors,
                            int delegateHash = 0) {
            Mode = mode;
            EventSelectors = eventSelectors;
            DelegateHash = delegateHash;
        }        
        
        
        public Subscription(SubscriptionMode mode,
                            TStream streamSelector,
                            TEvent eventSelector,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = new List<TStream>((IEnumerable<TStream>?) streamSelector);
            EventSelectors = new List<TEvent>((IEnumerable<TEvent>?) eventSelector);
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            TStream streamSelector,
                            List<TEvent> eventSelectors,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = new List<TStream>((IEnumerable<TStream>?) streamSelector);
            EventSelectors = eventSelectors;
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            List<TStream> streamSelectors,
                            TEvent eventSelector,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = streamSelectors;
            EventSelectors = new List<TEvent>((IEnumerable<TEvent>?) eventSelector);
            DelegateHash = delegateHash;
        }
        
        
        public Subscription(SubscriptionMode mode,
                            List<TStream> streamSelectors,
                            List<TEvent> eventSelectors,
                            int delegateHash = 0) {
            Mode = mode;
            StreamSelectors = streamSelectors;
            EventSelectors = eventSelectors;
            DelegateHash = delegateHash;
        }
    }
}