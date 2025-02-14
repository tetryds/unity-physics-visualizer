using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class SphereCastDemo : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            var ray = new Ray(transform.position, transform.forward);
            Physics.SphereCast(ray, radius, out RaycastHit hitInfo, maxDistance, layers);
            PhysicsVisualizer.SphereCast(ray, radius, hitInfo, maxDistance);
        }
    }
}
