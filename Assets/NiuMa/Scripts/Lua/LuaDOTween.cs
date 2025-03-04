using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XLua;

namespace NiuMa
{
    [LuaCallCSharp]
    public static class LuaDOTween
    {
        public static Tweener DOLocalMove(GameObject obj, Vector3 endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOLocalMove(endValue, duration, snapping);
        }

        public static Tweener DOLocalMoveX(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOLocalMoveX(endValue, duration, snapping);
        }

        public static Tweener DOLocalMoveY(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOLocalMoveY(endValue, duration, snapping);
        }

        public static Tweener DOLocalMoveZ(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOLocalMoveZ(endValue, duration, snapping);
        }

        public static Tweener DOLocalRotate(GameObject obj, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            return obj.transform.DOLocalRotate(endValue, duration, mode);
        }

        public static Tweener DOLocalRotateQuaternion(GameObject obj, Quaternion endValue, float duration)
        {
            return obj.transform.DOLocalRotateQuaternion(endValue, duration);
        }

        public static Tweener DOMove(GameObject obj, Vector3 endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOMove(endValue, duration, snapping);
        }

        public static Tweener DOMoveX(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOMoveX(endValue, duration, snapping);
        }

        public static Tweener DOMoveY(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOMoveY(endValue, duration, snapping);
        }

        public static Tweener DOMoveZ(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            return obj.transform.DOMoveZ(endValue, duration, snapping);
        }

        public static Tweener DORotate(GameObject obj, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            return obj.transform.DORotate(endValue, duration, mode);
        }

        public static Tweener DORotateQuaternion(GameObject obj, Quaternion endValue, float duration)
        {
            return obj.transform.DORotateQuaternion(endValue, duration);
        }

        public static Tweener DOScale(GameObject obj, Vector3 endValue, float duration)
        {
            return obj.transform.DOScale(endValue, duration);
        }

        public static Tweener DOScale(GameObject obj, float endValue, float duration)
        {
            return obj.transform.DOScale(endValue, duration);
        }

        public static Tweener DOScaleX(GameObject obj, float endValue, float duration)
        {
            return obj.transform.DOScaleX(endValue, duration);
        }

        public static Tweener DOScaleY(GameObject obj, float endValue, float duration)
        {
            return obj.transform.DOScaleY(endValue, duration);
        }

        public static Tweener DOScaleZ(GameObject obj, float endValue, float duration)
        {
            return obj.transform.DOScaleZ(endValue, duration);
        }

        public static Tweener DOAnchorPos(GameObject obj, Vector2 endValue, float duration, bool snapping = false)
        {
            RectTransform target = obj.GetComponent<RectTransform>();
            if (target == null)
                return null;
            return target.DOAnchorPos(endValue, duration, snapping);
        }

        public static Tweener DOAnchorPosX(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            RectTransform target = obj.GetComponent<RectTransform>();
            if (target == null)
                return null;
            return target.DOAnchorPosX(endValue, duration, snapping);
        }

        public static Tweener DOAnchorPosY(GameObject obj, float endValue, float duration, bool snapping = false)
        {
            RectTransform target = obj.GetComponent<RectTransform>();
            if (target == null)
                return null;
            return target.DOAnchorPosY(endValue, duration, snapping);
        }

        public static Tweener DOSizeDelta(GameObject obj, Vector2 endValue, float duration, bool snapping = false)
        {
            RectTransform target = obj.GetComponent<RectTransform>();
            if (target == null)
                return null;
            return target.DOSizeDelta(endValue, duration, snapping);
        }

        public static T SetEase<T>(T t, Ease ease, float amplitude, float period) where T : Tween
        {
            return t.SetEase(ease, amplitude, period);
        }

        public static T SetEase<T>(T t, Ease ease, float overshoot) where T : Tween
        {
            return t.SetEase(ease, overshoot);
        }

        public static T SetEase<T>(T t, Ease ease) where T : Tween
        {
            return t.SetEase(ease);
        }

        public static void Kill<T>(T t, bool complete = false) where T : Tween
        {
            t.Kill(complete);
        }

        public static T OnComplete<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnComplete(action);
        }

        public static T OnKill<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnKill(action);
        }

        public static T OnPause<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnPause(action);
        }

        public static T OnPlay<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnPlay(action);
        }

        public static T OnRewind<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnRewind(action);
        }

        public static T OnStart<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnStart(action);
        }

        public static T OnStepComplete<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnStepComplete(action);
        }

        public static T OnUpdate<T>(T t, TweenCallback action) where T : Tween
        {
            return t.OnUpdate(action);
        }
    }
}