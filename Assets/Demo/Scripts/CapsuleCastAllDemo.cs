using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class CapsuleCastAllDemo : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        [SerializeField] Transform pivot1;
        [SerializeField] Transform pivot2;

        private void OnDrawGizmos()
        {
            if (pivot1 == null || pivot2 == null) return;
            Gizmos.DrawIcon(Vector3.Lerp(pivot1.position, pivot2.position, 0.5f), "laser", false);

            var hits = Physics.CapsuleCastAll(pivot1.position, pivot2.position, radius, transform.forward, maxDistance, layers);
            PhysicsVisualizer.CapsuleCastAll(pivot1.position, pivot2.position, transform.forward, radius, hits, hits.Length, maxDistance);
        }
    }
}
