using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Models;
using Runtime.Models.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Behaviours
{
    public sealed class FlagBehaviour : MonoBehaviour
    {
        [SerializeField] private SortingGroup sortGroup;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text textComponent;
        [SerializeField] private SerializableDictionary<FlagColor, Sprite> flagsSprites;

        public void SetColor(FlagColor flagColor)
        {
            if (flagsSprites.TryGetValue(flagColor, out var sprite) && sprite != null)
                spriteRenderer.sprite = sprite;
        }

        public void SetNumber(int number) => textComponent.text = number.ToString();

        public void SetSorting(int order) => sortGroup.sortingOrder = order;

        public void RotateToGround(LayerMask groundLayer)
        {
            var tr = transform;
            var up = tr.up;
            var downPos = -up;
            RaycastHit2D downHit = Physics2D.Raycast(tr.position + up, downPos, Mathf.Infinity, groundLayer);

            if (downHit.collider == null)
                return;

            Vector2 normal = downHit.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90f;
            transform.Rotate(Vector3.forward, angle);
        }

        public async UniTask MoveDownAsync(MoveAnimationParameters parameters,
            CancellationToken cancellationToken)
        {
            var tween = this.transform.DOMove(
                parameters.targetPosition, parameters.duration, parameters.snapping);
            await tween.ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask RotateAsync(RotateAroundAnimationParameters parameters,
            CancellationToken cancellationToken)
        {
            var tween = this.transform.DORotate(
                parameters.EndValue, parameters.duration, parameters.rotateMode);
            await tween.ToUniTask(cancellationToken: cancellationToken);
        }


        public enum FlagColor
        {
            Red = 0,
            Blue = 1,
            Cyan = 2,
            Green = 3,
            Yellow = 4,
            Purple = 5,
        }
    }
}