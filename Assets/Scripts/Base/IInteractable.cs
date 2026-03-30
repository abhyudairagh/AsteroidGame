using System;

namespace Base
{
    public interface IInteractable<out T>
    {
        event Action<T> OnInteractedEvent; 
    }
}