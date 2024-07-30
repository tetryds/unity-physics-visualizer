using UnityEngine;

namespace PhysicsVisualizer
{
    public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        public static T Instance { get; private set; }

        [field: SerializeField]
        public bool IsDontDestroyOnLoad { get; private set; } = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"There is already a {GetType()} present. Destroying duplicate");
                Destroy(this);
                return;
            }

            Instance = this as T;

            if (Instance == null)
            {
                Debug.LogError($"Attempted to assign an instance of type {GetType()} to {typeof(T)}. This is not allowed.");
                Destroy(this);
                return;
            }

            if (IsDontDestroyOnLoad)
                DontDestroyOnLoad(Instance);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
