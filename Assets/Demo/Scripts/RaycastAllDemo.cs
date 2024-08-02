using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class RaycastAllDemo : MonoBehaviour
    {
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.RaycastAll(ray, maxDistance, layers);
            PhysicsVisualizer.RaycastAll(ray, hits, hits.Length, maxDistance);
        }

    }
}
