using UnityEngine;

namespace Runtime.Models.Parameters
{
    /// <summary>
    /// Structure for movement animation <see cref="DG.Tweening.ShortcutExtensions.DOMove"/>.
    /// </summary>
    [System.Serializable]
    public struct MoveAnimationParameters
    {
        /// <summary>
        /// Target position. End of animation.
        /// </summary>
        public Vector3 targetPosition;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public float duration;

        /// <summary>
        /// Snapping to int values.
        /// </summary>
        public bool snapping;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="targetPosition">The end value to reach.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers.</param>
        public MoveAnimationParameters(Vector3 targetPosition, float duration, bool snapping = false)
        {
            this.targetPosition = targetPosition;
            this.duration = duration;
            this.snapping = snapping;
        }
    }
}