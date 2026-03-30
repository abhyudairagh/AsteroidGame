using System;

namespace Base
{
   /// <summary>
   /// Used to get trigger callback when destroyed
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IDestroyedCallback <out T>
   {
      event Action<T> OnDestroyed;
   }
}
