namespace LazySloth.Observable
{
    using System;

    public interface IReadOnlyObservableProperty<T>
    {
        T Value { get; }
        T PreviousValue { get; }
        void Subscribe(Action subscriber, bool withCall = false);
        void Subscribe(Action<T> subscriber, bool withCall = false);
        /// <summary>
        /// Subscribe to be notified with current and previous value of property.
        /// first value = current value
        /// second value = previous value
        /// </summary>
        void Subscribe(Action<T, T> subscriber, bool withCall = false);
        void Unsubscribe(Action subscriber);
        void Unsubscribe(Action<T> subscriber, bool optional = false);
        void Unsubscribe(Action<T, T> subscriber);
    }
}