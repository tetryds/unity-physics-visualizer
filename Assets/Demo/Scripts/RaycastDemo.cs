using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    [ExecuteAlways]
    public class RaycastDemo : MonoBehaviour
    {
        [SerializeField] LayerMask layers;

        void Update()
        {
            if (!Application.isPlaying) return;

            CastRayWithDebug();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            CastRayWithDebug();
        }

        private void CastRayWithDebug()
        {
            var ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, 3f, layers);
            PhysicsVisualizer.Raycast(ray, hitInfo, 3f);
        }
    }
}
