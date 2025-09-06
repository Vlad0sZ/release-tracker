using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class GroundRotateComponent : MonoBehaviour
    {
        [SerializeField] private float rayLength = 2f;
        [SerializeField] private float forwardRayAngle = 45f;
        [SerializeField] private LayerMask groundLayer;
        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
        private void FixedUpdate() => FindBestRotate();

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            var downPos = transform.up * -1f;
            Vector3 forwardDownDir = Quaternion.Euler(0, 0, -forwardRayAngle) * downPos;
            Vector3 backwardDownDir = Quaternion.Euler(0, 0, forwardRayAngle) * downPos;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + downPos * rayLength);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pos, pos + forwardDownDir * rayLength);

            Gizmos.color = Color.purple;
            Gizmos.DrawLine(pos, pos + backwardDownDir * rayLength);
        }

        private void FindBestRotate()
        {
            var downPos = transform.up * -1f;
            var hits = HitGround(downPos,
                Quaternion.identity,
                Quaternion.Euler(0, 0, -forwardRayAngle),
                Quaternion.Euler(0, 0, forwardRayAngle));

            if (TryCalculateAngle(hits, out var angle))
                ApplySurfaceAlignment(angle);
        }

        private IEnumerable<RaycastHit2D> HitGround(Vector3 dir, params Quaternion[] rotated)
        {
            foreach (var quaternion in rotated)
            {
                Vector2 direction = quaternion * dir;
                RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, direction, rayLength, groundLayer);

                if (hit.collider != null)
                    yield return hit;
            }
        }

        private static bool TryCalculateAngle(IEnumerable<RaycastHit2D> hits, out float angle)
        {
            angle = 0f;
            var hitsList = hits.ToArray();

            if (hitsList.Length == 0)
                return false;

            if (hitsList.Length == 1)
            {
                angle = Vector2.Angle(Vector2.up, hitsList[0].normal);
                return true;
            }

            Vector2 averageNormal = hitsList.Aggregate(Vector2.zero, (current, hit) => current + hit.normal);
            averageNormal /= hitsList.Length;

            angle = Vector2.SignedAngle(Vector2.up, averageNormal);
            return true;
        }

        private void ApplySurfaceAlignment(float surfaceAngle)
        {
            var different = Mathf.Abs(_rigidbody.rotation - surfaceAngle);
            _rigidbody.MoveRotation(surfaceAngle);
        }
    }
}