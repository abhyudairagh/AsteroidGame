using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace Utility.Impl
{
    public class AutoTimeOutQueue: IAutoTimeOutQueue
    {
        private readonly IUnityLifeCycleHelper _unityLifeCycleHelper;

        public AutoTimeOutQueue(IUnityLifeCycleHelper unityLifeCycleHelper)
        {
            _unityLifeCycleHelper = unityLifeCycleHelper;
        }
    
        private Dictionary<CoroutineRunner, Action> _collection = new ();
        public void Add(float timeOutDuration, Action callback)
        {
            var runner =  new CoroutineRunner(OnTriggered, _unityLifeCycleHelper.Behaviour);
            _collection.Add(runner, callback);
            runner.Run(timeOutDuration);
        }

        public void Remove(Action callback)
        {
            var pair = _collection.FirstOrDefault(x => x.Value == callback);
            if(pair.Value != null && pair.Key != null)
            {
                _collection.Remove(pair.Key);
            }
        }

        private void OnTriggered(CoroutineRunner runner)
        {
            var isAvailable = _collection.TryGetValue(runner, out Action value);
            if (isAvailable)
            {
                value?.Invoke();
            }
            _collection.Remove(runner);
        }

        public void Dispose()
        {
            foreach (var pair in _collection)
            {
                if (pair.Key != null)
                {
                    pair.Key.Stop();
                }
            }
            _collection.Clear();
        }
    
        private class CoroutineRunner
        {
            private Coroutine _coroutine;
            private Action<CoroutineRunner> _callBack { get; }
            private MonoBehaviour _behaviour { get; }
            public CoroutineRunner(Action<CoroutineRunner> callback, MonoBehaviour behaviour)
            {
        
                _callBack = callback;
                _behaviour = behaviour;
            }

            public void Run(float timeOut)
            {
                _coroutine = _behaviour?.StartCoroutine(Timer(_callBack, timeOut)); ;
            }

            IEnumerator Timer(Action<CoroutineRunner> callBack, float timeOut)
            {
                yield return new WaitForSeconds(timeOut);
                callBack?.Invoke(this);
            }

            public void Stop()
            {
                if (_coroutine != null)
                {
                    _behaviour?.StopCoroutine(_coroutine);
                }
            }
        }
    }

    public interface IAutoTimeOutQueue : IDisposable
    {
        void Add(float timeOutDuration, Action callback);
        void Remove(Action callback);
    }
}