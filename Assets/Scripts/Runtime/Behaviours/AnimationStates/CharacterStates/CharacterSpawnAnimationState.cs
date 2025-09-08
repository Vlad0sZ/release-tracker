using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.CharacterStates.Payloads;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates
{
    public sealed class CharacterSpawnAnimationState : AnimationStateWithResult<CharacterSpawnStatePayload, Character>
    {
        private readonly Transform _parent;
        private readonly PrefabOf<Character> _characterPrefab;
        private readonly AnimationSettings _animationSettings;

        public CharacterSpawnAnimationState(Transform parent,
            PrefabOf<Character> characterPrefab, AnimationSettings animationSettings)
        {
            _parent = parent;
            _characterPrefab = characterPrefab;
            _animationSettings = animationSettings;
        }


        protected override async UniTask<Character> AnimateWithResult(CharacterSpawnStatePayload payload,
            CancellationToken cancellationToken)
        {
            var character = await Instantiate(_parent, _characterPrefab, cancellationToken);
            character.transform.position = payload.Position + Vector3.up * _animationSettings.characterYOffset;
            await character.Animator.JumpAsync(cancellationToken);
            await UniTask.NextFrame();

            return character;
        }
    }
}