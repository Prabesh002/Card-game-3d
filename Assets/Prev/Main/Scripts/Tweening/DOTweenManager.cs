using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DOTweenManager : MonoBehaviour
{
    public enum AnimationType
    {
        Position,
        Rotation,
        Scale,
        Fade,
        Color,
        ShakePosition,
        ShakeRotation,
        ShakeScale,
        PunchPosition,
        PunchRotation,
        PunchScale,
        Path
    }

    [System.Serializable]
    public class AnimationBehavior
    {
        public bool foldout = true;
        public AnimationType animationType;

        public bool reverse;

       

        public bool startAtBeginning = true;
        public bool loop;
        public int loopCount = -1;
        public LoopType loopType = LoopType.Restart;

        public Vector3 targetPosition;
        public float positionDuration = 1f;
        public Ease positionEase = Ease.Linear;

        public Vector3 targetRotation;
        public float rotationDuration = 1f;
        public Ease rotationEase = Ease.Linear;

        public Vector3 targetScale = Vector3.one;
        public float scaleDuration = 1f;
        public Ease scaleEase = Ease.Linear;

        public float targetAlpha = 1f;
        public float fadeDuration = 1f;
        public bool reverseFade = false;
        public Ease fadeEase = Ease.Linear;

        public Color targetColor = Color.white;
        public float colorDuration = 1f;
        public Ease colorEase = Ease.Linear;

        public Vector3 shakeStrength = Vector3.one;
        public float shakeDuration = 1f;
        public int vibrato = 10;
        public float randomness = 90f;

        public Vector3 punchStrength = Vector3.one;
        public float punchDuration = 1f;
        public int punchVibrato = 10;
        public float punchElasticity = 1f;

        public Vector3[] pathPoints;
        public float pathDuration = 1f;
        public PathType pathType = PathType.CatmullRom;
        public PathMode pathMode = PathMode.Full3D;

        public UnityEvent OnStartEvent;
        public UnityEvent OnCompleteEvent;
    }

    public List<AnimationBehavior> behaviors = new List<AnimationBehavior>();

    private RectTransform rectTransform;
    private Graphic graphic;
    private CanvasGroup canvasGroup;

     public Vector3 inital;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        
        graphic = GetComponent<Graphic>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (behaviors.Exists(b => b.animationType == AnimationType.Fade) && canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

    }

    private void Start()
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.startAtBeginning)
            {
                PlayAnimation(behavior);
            }
        }
    }

    public void PlayActiveAnimations()
    {

        foreach (var behavior in behaviors)
        {
            PlayAnimation(behavior);
            break;
        }
    }

    public void PlayAnimation(AnimationBehavior behavior)
    {
        Sequence sequence = DOTween.Sequence();

        switch (behavior.animationType)
        {
            case AnimationType.Position:
                sequence.Append(rectTransform.DOAnchorPos(behavior.targetPosition, behavior.positionDuration).SetEase(behavior.positionEase));
                break;

            case AnimationType.Rotation:
                sequence.Append(rectTransform.DORotate(behavior.targetRotation, behavior.rotationDuration).SetEase(behavior.rotationEase));
                break;

            case AnimationType.Scale:
                sequence.Append(rectTransform.DOScale(behavior.targetScale, behavior.scaleDuration).SetEase(behavior.scaleEase));
                break;

            case AnimationType.Fade:
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                    sequence.Append(canvasGroup.DOFade(behavior.targetAlpha, behavior.fadeDuration).SetEase(behavior.fadeEase));
                    if (behavior.reverseFade)
                    {
                        sequence.AppendInterval(behavior.fadeDuration); 
                        sequence.Append(canvasGroup.DOFade(0f, behavior.fadeDuration).SetEase(behavior.fadeEase)); 
                    }
                }
                break;

            case AnimationType.Color:
                if (graphic != null)
                    sequence.Append(graphic.DOColor(behavior.targetColor, behavior.colorDuration).SetEase(behavior.colorEase));
                break;

            case AnimationType.ShakePosition:
                sequence.Append(rectTransform.DOShakeAnchorPos(behavior.shakeDuration, behavior.shakeStrength, behavior.vibrato, behavior.randomness));
                break;

            case AnimationType.ShakeRotation:
                sequence.Append(rectTransform.DOShakeRotation(behavior.shakeDuration, behavior.shakeStrength, behavior.vibrato, behavior.randomness));
                break;

            case AnimationType.ShakeScale:
                sequence.Append(rectTransform.DOShakeScale(behavior.shakeDuration, behavior.shakeStrength, behavior.vibrato, behavior.randomness));
                break;

            case AnimationType.PunchPosition:
                sequence.Append(rectTransform.DOPunchAnchorPos(behavior.punchStrength, behavior.punchDuration, behavior.punchVibrato, behavior.punchElasticity));
                break;

            case AnimationType.PunchRotation:
                sequence.Append(rectTransform.DOPunchRotation(behavior.punchStrength, behavior.punchDuration, behavior.punchVibrato, behavior.punchElasticity));
                break;

            case AnimationType.PunchScale:
                sequence.Append(rectTransform.DOPunchScale(behavior.punchStrength, behavior.punchDuration, behavior.punchVibrato, behavior.punchElasticity));
                break;

            case AnimationType.Path:
                if (behavior.pathPoints != null && behavior.pathPoints.Length > 0)
                    sequence.Append(rectTransform.DOPath(behavior.pathPoints, behavior.pathDuration, behavior.pathType, behavior.pathMode));
                break;
        }

        if (behavior.loop)
        {
            sequence.SetLoops(behavior.loopCount, behavior.loopType);
        }

        behavior.OnStartEvent?.Invoke();
        sequence.OnComplete(() => behavior.OnCompleteEvent?.Invoke());
    }

    public void PlayAllAnimations()
    {
        foreach (var behavior in behaviors)
        {
            PlayAnimation(behavior);
        }
    }


    public void ReverseAnim()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(inital, 0.5f));

    }


}
