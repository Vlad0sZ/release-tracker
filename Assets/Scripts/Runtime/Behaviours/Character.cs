using UnityEngine;

namespace Runtime.Behaviours
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private CharacterWalker walker;
        [SerializeField] private CharacterAnimation animator;

        public CharacterWalker Walker => walker;
        public CharacterAnimation Animator => animator;
    }
}