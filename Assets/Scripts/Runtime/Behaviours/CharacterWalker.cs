using UnityEngine;

namespace Runtime.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class CharacterWalker : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private bool isWalking;
        [SerializeField] private bool backward;

        private Rigidbody2D _rigidbody;

        public bool IsWalking
        {
            get => isWalking;
            set => isWalking = value;
        }

        public float Speed
        {
            get => backward ? -moveSpeed : moveSpeed;
            set => moveSpeed = Mathf.Abs(value);
        }

        public bool Backward
        {
            get => backward;
            set => backward = value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }


        private void FixedUpdate()
        {
            if (!IsWalking)
                return;

            Vector2 forward = transform.right;
            Vector2 newPos = _rigidbody.position + forward * (Speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPos);
        }
    }
}