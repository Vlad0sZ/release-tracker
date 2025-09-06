using DG.Tweening;
using UnityEngine;

namespace Runtime.Models.Parameters
{
    /// <summary>
    /// Structure for rotation around axis animation <see cref="DG.Tweening.ShortcutExtensions.DORotate"/>.
    /// </summary>
    [System.Serializable]
    public struct RotateAroundAnimationParameters
    {
        /// <summary>
        /// Axis of the animation.
        /// <remarks>
        /// <see cref="Vector3.up"/> for rotate around Y, or <see cref="Vector3.forward"/> to rotate around Z coordinate.
        /// </remarks>
        /// </summary>
        public Vector3 axis;

        /// <summary>
        /// Angle to rotate a object.
        /// </summary>
        public float angle;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public float duration;

        /// <summary>
        /// <see cref="DG.Tweening.RotateMode"/>.
        /// </summary>
        public RotateMode rotateMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis">Axis around rotate.</param>
        /// <param name="angle">The end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        /// <param name="rotateMode">Rotation mode.</param>
        public RotateAroundAnimationParameters(Vector3 axis, float angle, float duration,
            RotateMode rotateMode = RotateMode.Fast)
        {
            this.axis = axis;
            this.angle = angle;
            this.duration = duration;
            this.rotateMode = rotateMode;
        }

        public Vector3 EndValue => axis * angle;
    }
}