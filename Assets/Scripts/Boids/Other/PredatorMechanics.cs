using UnityEngine;

namespace Europa.Boids
{
    public class PredatorMechanics : MonoBehaviour
    {
        public Vector3 currentTarget = Vector3.zero;
        private OtherBoid boid;

        private void Start()
        {
            boid = GetComponent<OtherBoid>();
        }

        private void Update()
        {
            if (Near(currentTarget)) currentTarget = Vector3.zero;

            if (currentTarget != Vector3.zero) boid.SwimTo(currentTarget);
            else currentTarget = Random.insideUnitSphere * 50f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && Random.Range(0f, 1f) <= 0.2f) { if (currentTarget == Vector3.zero) boid.SwimTo(other.transform.position); currentTarget = other.transform.position; }
            else if (other.GetComponent<Boid>() && Random.Range(0f, 1f) <= 0.5f) { if (currentTarget == Vector3.zero) boid.SwimTo(other.transform.position); currentTarget = other.transform.position; }
        }

        private bool Near(Vector3 pos)
        {
            if (Mathf.Abs(transform.position.x - pos.x) + Mathf.Abs(transform.position.y - pos.y) + Mathf.Abs(transform.position.z - pos.z) < 1.5f) return true;
            return false;
        }
    }
}
