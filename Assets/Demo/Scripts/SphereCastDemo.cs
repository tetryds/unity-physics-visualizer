using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    [ExecuteAlways]
    public class SphereCastDemo : MonoBehaviour
    {
        [SerializeField] float radius;
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
            Physics.SphereCast(ray, radius, out RaycastHit hitInfo, maxDistance, layers);
            PhysicsVisualizer.SphereCast(ray, radius, hitInfo, 3f);
        }
    }
}
