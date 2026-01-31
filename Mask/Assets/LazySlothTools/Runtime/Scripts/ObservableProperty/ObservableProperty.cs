namespace LazySloth.Observable
{
    using System;
    using System.Collections.Generic;
    using Utilities.Generics;

    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private static readonly IEqualityComparer<T> DefaultEqualityComparer = UnityEqualityComparer.GetDefault<T>();
        
        private List<Action> _planeSubscribers = new List<Action>();
        private List<Action<T>> _subscribers = new List<Action<T>>();
        private List<Action<T,T>> _subscribersWithPrevious = new List<Action<T,T>>();

        private T _value;

        public T PreviousValue { get; private set; }
        public T Value
        {
            get => _value;
            set
            {
                if (!DefaultEqualityComparer.Equals(_value, value))
                {
                    PreviousValue = _value;
                    _value = value;
                    NotifySubscribers();
                }
            }
        }

        public ObservableProperty()
        {
            _value = default;
        }

        public ObservableProperty(T value)
        {
            _value = value;
        }

        public void Subscribe(Action subscriber, bool withCall = false)
        {
            _planeSubscribers.Add(subscriber);
            if (withCall)
            {
                subscriber?.Invoke();
            }
        }

        public void Subscribe(Action<T> subscriber, bool withCall = false)
        {
            _subscribers.Add(subscriber);
            if (withCall)
            {
                subscriber?.Invoke(Value);
            }
        }

        public void Subscribe(Action<T, T> subscriber, bool withCall = false)
        {
            _subscribersWithPrevious.Add(subscriber);
            if (withCall)
            {
                subscriber?.Invoke(Value, PreviousValue);
            }
        }

        public void Unsubscribe(Action subscriber)
        {
            if (!_planeSubscribers.Contains(subscriber))
            {
                throw new ArgumentException("Given method to unsubscribe was not found on subscriber list! (Did you use lambda expression to subscribe?)");
            }

            _planeSubscribers.Remove(subscriber);
        }

        public void Unsubscribe(Action<T> subscriber, bool optional = false)
        {
            if (!optional && !_subscribers.Contains(subscriber))
            {
                throw new ArgumentException("Given method to unsubscribe was not found on subscriber list! (Did you use lambda expression to subscribe?)");
            }

            if (!optional && _subscribers.Contains(subscriber))
            {
                _subscribers.Remove(subscriber);
            }
        }

        public void Unsubscribe(Action<T,T> subscriber)
        {
            if (!_subscribersWithPrevious.Contains(subscriber))
            {
                throw new ArgumentException("Given method to unsubscribe was not found on subscriber list! (Did you use lambda expression to subscribe?)");
            }

            _subscribersWithPrevious.Remove(subscriber);
        }

        public void ForceNotifySubscribers()
        {
            NotifySubscribers();
        }

        private void NotifySubscribers()
        {
            for (int i = 0; i < _subscribers.Count; i++)
            {
                _subscribers[i]?.Invoke(Value);
            }
            
            for (int i = 0; i < _planeSubscribers.Count; i++)
            {
                _planeSubscribers[i]?.Invoke();
            }
            
            for (int i = 0; i < _subscribersWithPrevious.Count; i++)
            {
                _subscribersWithPrevious[i]?.Invoke(Value, PreviousValue);
            }
        }

        public override string ToString() => $"Value = {Value}";
    }
}