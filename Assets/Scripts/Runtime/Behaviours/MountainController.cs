using System;
using UnityEngine;

namespace Runtime.Behaviours
{
    public sealed class MountainController : MonoBehaviour
    {
        [SerializeField] private Transform min;
        [SerializeField] private Transform max;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float raycastHeight = 10f;

        [Header("Debug section")] [Range(0, 1)] [SerializeField]
        private float percentDebug;

        private Vector3 Min => min == null ? Vector3.zero : min.position;

        private Vector3 Max => max == null ? Vector3.zero : max.position;

        public Vector2 GetPositionAt(float percentage)
        {
            var lerp = Mathf.Clamp01(percentage);
            var position = Vector3.Lerp(Min, Max, lerp);

            Vector2 ray = new Vector2(position.x, Max.y + raycastHeight);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Infinity, groundLayer);
            if (hit.collider != null)
                return hit.point;

            return Vector2.zero;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Min, Max);

            Gizmos.color = Color.red;
            var pos = GetPositionAt(percentDebug);
            Gizmos.DrawSphere(pos, 0.1f);
        }
    }
}