using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class BoxCastAllDemo : MonoBehaviour
    {
        [SerializeField] Transform pivot;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            if (pivot == null) return;

            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.BoxCastAll(transform.position, pivot.localScale, transform.forward, pivot.rotation, maxDistance, layers);
            PhysicsVisualizer.BoxCastAll(ray, pivot.localScale, pivot.rotation, hits, hits.Length, maxDistance);
        }
    }
}
