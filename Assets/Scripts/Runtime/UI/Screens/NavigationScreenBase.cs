using Runtime.Interfaces.Navigations;
using UnityEngine;
using DG.Tweening;
using Runtime.Core;

namespace Runtime.UI.Screens
{
    /// <summary>
    /// Base class for navigation screens in the Release Tracker application.
    /// Provides common functionality for showing and hiding screens using CanvasGroup and DOTween animations.
    /// </summary>
    public abstract class NavigationScreenBase : DestroyableObject, INavigationScreen
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.3f; // Duration of fade animation in seconds

        /// <summary>
        /// Initializes the screen, ensuring CanvasGroup is set up.
        /// </summary>
        protected virtual void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        /// <summary>
        /// Shows the screen with a fade-in animation using DOTween.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration)
                .SetEase(Ease.InOutQuad)
                .OnStart(() =>
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                    BeforeShow();
                })
                .OnComplete(AfterShow)
                .SetUpdate(true);
        }

        /// <summary>
        /// Hides the screen with a fade-out animation using DOTween.
        /// </summary>
        public virtual void Hide()
        {
            canvasGroup.DOKill();

            canvasGroup.DOFade(0f, fadeDuration)
                .SetEase(Ease.InOutQuad)
                .OnStart(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.interactable = false;
                    BeforeHide();
                })
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    AfterHide();
                })
                .SetUpdate(true);
        }


        protected virtual void BeforeShow()
        {
        }

        protected virtual void AfterShow()
        {
        }

        protected virtual void BeforeHide()
        {
        }

        protected virtual void AfterHide()
        {
        }
    }
}