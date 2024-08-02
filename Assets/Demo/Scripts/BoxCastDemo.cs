using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class BoxCastDemo : MonoBehaviour
    {
        [SerializeField] Transform pivot;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            if (pivot == null) return;

            var ray = new Ray(transform.position, transform.forward);
            Physics.BoxCast(transform.position, pivot.localScale, transform.forward, out RaycastHit hitInfo, pivot.rotation, maxDistance, layers);
            PhysicsVisualizer.BoxCast(ray, pivot.localScale, pivot.rotation, hitInfo, maxDistance);
        }
    }
}
