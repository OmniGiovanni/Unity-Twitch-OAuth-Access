using System.Collections.Concurrent;
using System.Threading;
using System;
using UnityEngine;

namespace OmniGiovanni.Web
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> _executionQueue = new ConcurrentQueue<Action>();
        private static UnityMainThreadDispatcher _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            while (_executionQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }

        public void Enqueue(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            _executionQueue.Enqueue(action);
        }

        public static UnityMainThreadDispatcher Instance()
        {
            if (_instance == null)
            {
                // Ensuring Instance is only created on the main thread.
                if (Thread.CurrentThread.ManagedThreadId != 1)
                {
                    throw new InvalidOperationException("UnityMainThreadDispatcher can only be accessed from the main thread.");
                }

                _instance = FindObjectOfType<UnityMainThreadDispatcher>();

                if (_instance == null)
                {
                    var obj = new GameObject("MainThreadDispatcher");
                    _instance = obj.AddComponent<UnityMainThreadDispatcher>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }
}
