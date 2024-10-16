using UnityEngine;
using Europa.Player;

namespace Europa.Boids
{
    public class Boid : MonoBehaviour
    {

        BoidSettings settings;

        [HideInInspector] public Vector3 position;
        [HideInInspector] public Vector3 forward;
        Vector3 velocity;

        Vector3 acceleration;
        [HideInInspector] public Vector3 avgFlockHeading;
        [HideInInspector] public Vector3 avgAvoidanceHeading;
        [HideInInspector] public Vector3 centreOfFlockmates;
        [HideInInspector] public int numPerceivedFlockmates;

        Material material;
        Transform cachedTransform;
        Transform target;

        void Awake()
        {
            if (transform.GetComponentInChildren<MeshRenderer>())
                material = transform.GetComponentInChildren<MeshRenderer>().material;
            else material = transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
            cachedTransform = transform;
        }

        public void Initialize(BoidSettings settings, Transform target)
        {
            this.target = target;
            this.settings = settings;

            position = cachedTransform.position;
            forward = cachedTransform.forward;

            float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
            velocity = transform.forward * startSpeed;
        }

        public void SetColor(Color col)
        {
            if (material != null) material.color = col;
        }

        public void UpdateBoid()
        {
            if (Player.Player.Singleton.GamePaused) return;

            Vector3 acceleration = Vector3.zero;

            if (target != null)
            {
                Vector3 offsetToTarget = (target.position - position);
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }

            if (numPerceivedFlockmates != 0)
            {
                centreOfFlockmates /= numPerceivedFlockmates;

                Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += seperationForce;
            }

            if (IsHeadingForCollision())
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }

            velocity += acceleration * Time.deltaTime;
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
            velocity = dir * speed;

            cachedTransform.position += velocity * Time.deltaTime;
            cachedTransform.forward = dir;
            position = cachedTransform.position;
            forward = dir;
        }

        public void SwimTo(Vector3 target)
        {
            if (target != null)
            {
                Vector3 offsetToTarget = (target - position);
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }

            if (IsHeadingForCollision())
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }

            velocity += acceleration * Time.deltaTime;
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
            velocity = dir * speed;

            cachedTransform.position += velocity * Time.deltaTime;
            cachedTransform.forward = dir;
            position = cachedTransform.position;
            forward = dir;
        }

        bool IsHeadingForCollision()
        {
            if (Physics.SphereCast(position, settings.boundsRadius, forward, out RaycastHit hit, settings.collisionAvoidDst, settings.obstacleMask)) 
                return true;
            return false;
        }

        Vector3 ObstacleRays()
        {
            Vector3[] rayDirections = BoidHelper.directions;

            for (int i = 0; i < rayDirections.Length; i++)
            {
                Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
                Ray ray = new Ray(position, dir);
                if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
                    return dir;
            }

            return forward;
        }

        Vector3 SteerTowards(Vector3 vector)
        {
            Vector3 v = vector.normalized * settings.maxSpeed - velocity;
            return Vector3.ClampMagnitude(v, settings.maxSteerForce);
        }

    }
}
