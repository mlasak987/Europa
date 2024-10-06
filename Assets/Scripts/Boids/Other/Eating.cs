using Europa.Utils;
using UnityEngine;

namespace Europa.Boids
{
    public class Eating : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<PredatorMechanics>().currentTarget = Vector3.zero;

            if (other.GetComponent<Boid>()) Destroy(other.GetComponent<ToDestroy>().gameObject);
            else if (other.CompareTag("Player")) Player.Player.Singleton.Die();
        }
    }
}