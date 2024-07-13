using UnityEngine;

namespace Wixot.Engine
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                SetInstance();
                _instance.Initialize();

                return _instance;
            }
        }

        private static void SetInstance()
        {
            var instance = FindObjectOfType<T>();
            if (instance != null)
            {
                _instance = instance;
                _instance.Initialize();
            }
            else
                Instantiate(Resources.Load(Singletons.TypeNameMap[typeof(T)]) as GameObject);
        }

        protected virtual void Initialize()
        {
            if (Application.IsPlaying(this))
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void Awake()
        {
            CheckInstance();
        }

        protected virtual void OnEnable()
        {
            CheckInstance();
        }

        private void CheckInstance()
        {
            if (_instance != null && _instance != this)
                DestroyImmediate(gameObject);
            else if (_instance == null)
            {
                _instance = (T) this;
                Initialize();
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
