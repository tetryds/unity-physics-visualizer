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

        [SerializeField] Transform pivot;

        private void OnDrawGizmos()
        {
            if (pivot == null) return;
            Gizmos.DrawIcon(transform.position, "laser", false);
            Vector3 pos1 = pivot.position - pivot.up * pivot.localScale.y;
            Vector3 pos2 = pivot.position + pivot.up * pivot.localScale.y;
            var hits = Physics.CapsuleCastAll(pos1, pos2, radius, transform.forward, maxDistance, layers);
            PhysicsVisualizer.CapsuleCastAll(pos1, pos2, transform.forward, radius, hits, hits.Length, maxDistance);
        }
    }
}
