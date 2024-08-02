using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class SphereCastAllDemo : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.SphereCastAll(ray, radius, maxDistance, layers);
            PhysicsVisualizer.SphereCastAll(ray, radius, hits, hits.Length, maxDistance);
        }
    }
}
