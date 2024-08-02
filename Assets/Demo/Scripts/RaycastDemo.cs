using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    public class RaycastDemo : MonoBehaviour
    {
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            var ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layers);
            PhysicsVisualizer.Raycast(ray, hitInfo, maxDistance);
        }

    }
}
