using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class CapsuleCastDemo : MonoBehaviour
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
            Physics.CapsuleCast(pos1, pos2, radius, transform.forward, out RaycastHit hitInfo, maxDistance, layers);
            PhysicsVisualizer.CapsuleCast(pos1, pos2, transform.forward, radius, hitInfo, maxDistance);
        }
    }
}
