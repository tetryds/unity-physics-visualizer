using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    [ExecuteAlways]
    public class ExtraGizmosDemo : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] int steps;
        [SerializeField] float distance;
        [SerializeField] float radius;
        [SerializeField] Color color;
        [SerializeField] LayerMask layers;

        private void OnDrawGizmos()
        {
            float angle = Mathf.Atan2(radius, distance) * Mathf.Rad2Deg;
            Gizmos.color = color;
            ExtraGizmos.STEPS = Mathf.Clamp(steps, 1, int.MaxValue);
            //ExtraGizmos.DrawCapsule(transform.position, transform.forward, radius, distance);
            ExtraGizmos.DrawCylinder(transform.position, transform.forward, radius, distance);
            ExtraGizmos.DrawCone(transform.position, transform.forward, distance, angle);
            ExtraGizmos.DrawCircleCap(transform.position, transform.forward, radius);
            ExtraGizmos.DrawSphereCap(transform.position, transform.forward, radius, 90f);
        }
    }
}
