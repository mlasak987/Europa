using UnityEngine;

namespace Europa.Utils
{
    public class LandingPod : MonoBehaviour
    {
        private static LandingPod _singleton;
        public static LandingPod Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null) _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(LandingPod)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }
    }

}