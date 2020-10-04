using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Lavender.Stream
{
    public class Subscriber<TSubject, TStream, TEvent> {
        public TSubject Subject;
        public List<Subscription<TStream, TEvent>>? Subscriptions;
        
        
        public Subscriber(TSubject subject) => Subject = subject;
        
        
        public Subscriber(TSubject subject,
                            List<Subscription<TStream, TEvent>> subscriptions)
        {
            Subject = subject;
            Subscriptions = subscriptions;
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode,
                            List<TStream> StreamSelectors)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode, StreamSelectors);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode,
                            List<TStream> StreamSelectors,
                            List<TEvent> EventSelectors)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode, StreamSelectors, EventSelectors);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode,
                            TStream StreamSelector,
                            List<TEvent> EventSelectors)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode, StreamSelector, EventSelectors);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode,
                            List<TStream> StreamSelectors,
                            TEvent EventSelector)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode, StreamSelectors, EventSelector);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public Subscriber(TSubject subject,
                            SubscriptionMode Mode,
                            TStream StreamSelector,
                            TEvent EventSelector)
        {
            Subject = subject;
            
            var Sub = new Subscription<TStream, TEvent>(Mode, StreamSelector, EventSelector);
            
            Subscriptions = new List<Subscription<TStream, TEvent>> { Sub };
        }
        
        
        public void AddSubscription(SubscriptionMode Mode,
                                    Delegate? Handler,
                                    TEvent Event)
        {
            var Sub = new Subscription<TStream, TEvent>(Mode, Event, Handler?.GetHashCode() ?? 0);
            Subscriptions?.Add(Sub);
        }
        
        
        public void AddSubscription(SubscriptionMode Mode,
                                    Delegate? Handler,
                                    TEvent Event,
                                    TStream Selector)
        {
            var Sub = new Subscription<TStream, TEvent>(Mode, Selector, Event,
                                                        Handler?.GetHashCode() ?? 0);
            Subscriptions?.Add(Sub);
        }
        
        
        public void AddSubscription(SubscriptionMode Mode,
                                    Delegate? Handler,
                                    TEvent Event,
                                    List<TStream> Selectors)
        {
            var Sub = new Subscription<TStream, TEvent>(Mode, Selectors, Event,
                                                        Handler?.GetHashCode() ?? 0);
            Subscriptions?.Add(Sub);
        }
        
        
        public void AddSubscription(SubscriptionMode Mode,
                                    Delegate? Handler,
                                    List<TEvent> Events,
                                    TStream Selector)
        {
            var Sub = new Subscription<TStream, TEvent>(Mode, Selector, Events,
                                                        Handler?.GetHashCode() ?? 0);
            Subscriptions?.Add(Sub);
        }
        
        
        public void AddSubscription(SubscriptionMode Mode,
                                    Delegate? Handler,
                                    List<TEvent> Events,
                                    List<TStream> Selectors)
        {
            var Sub = new Subscription<TStream, TEvent>(Mode, Selectors, Events,
                                                        Handler?.GetHashCode() ?? 0);
            Subscriptions?.Add(Sub);
        }
    }
}