using Europa.Boids;
using UnityEngine;

namespace Europa.Utils
{
    public class ToDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            if (TryGetComponent(out Boid boid))
                BoidManager.Singleton.boids.RemoveAll(b => b.gameObject == boid.gameObject);
        }
    }
}