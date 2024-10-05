﻿using UnityEngine;

namespace Europa.Boids
{
    public class Spawner : MonoBehaviour
    {

        public enum GizmoType { Never, SelectedOnly, Always }

        public Boid prefab;
        public float spawnRadius = 10;
        public int spawnCount = 10;
        public Color colour;
        public GizmoType showSpawnRegion;

        private void Awake()
        {
            Spawn();
        }

        public void Spawn()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
                Boid boid = Instantiate(prefab, transform);
                boid.transform.position = pos;
                boid.transform.forward = Random.insideUnitSphere;

                boid.SetColor(colour);
            }
        }

        public void Despawn()
        {
            foreach (Transform boid in transform) Destroy(boid.gameObject);
        }

        private void OnDrawGizmos()
        {
            if (showSpawnRegion == GizmoType.Always) DrawGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (showSpawnRegion == GizmoType.SelectedOnly) DrawGizmos();
        }

        void DrawGizmos()
        {
            Gizmos.color = new Color(colour.r, colour.g, colour.b, 0.3f);
            Gizmos.DrawSphere(transform.position, spawnRadius);
        }
    }
}
