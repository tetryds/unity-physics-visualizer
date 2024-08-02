using UnityEngine;

namespace PhysicsVisualizer
{
    public class AutoSingletonBehaviour<T> : MonoBehaviour where T : AutoSingletonBehaviour<T>
    {
        private static T instance;

        public static T Instance => GetInstance();

        private void Awake()
        {
            if (instance != this)
            {
                if (instance != null)
                {
                    Debug.LogWarning($"There is already a {GetType()} present. Destroying duplicate");
                    Destroy(gameObject);
                    return;
                }

                instance = this as T;

                if (instance == null)
                {
                    Debug.LogError($"Attempted to assign an instance of type {GetType()} to {typeof(T)}. This is not allowed.");
                    Destroy(gameObject);
                    return;
                }
            }

            if (Application.isPlaying)
                DontDestroyOnLoad(instance);
        }

        private static T GetInstance()
        {
            if (instance != null) return instance;
            
            instance = FindObjectOfType<T>();
            if (instance == null)
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return instance;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                instance = null;
        }
    }
}
