using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    [ExecuteAlways]
    public class RaycastDemo : MonoBehaviour
    {
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layers;

        void Update()
        {
            if (!Application.isPlaying) return;

            CastRayWithDebug();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "laser", false);
            if (Application.isPlaying) return;

            CastRayWithDebug();
        }

        private void CastRayWithDebug()
        {
            var ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layers);
            PhysicsVisualizer.Raycast(ray, hitInfo, maxDistance);
        }
    }
}
