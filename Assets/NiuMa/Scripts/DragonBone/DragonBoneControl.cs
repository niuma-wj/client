using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonBones
{
    public delegate void AnimationEventHandler(string anim, string type);

    public class DragonBoneControl : MonoBehaviour
    {
        [SerializeField]
        private UnityArmatureComponent _armature = null;

        private event AnimationEventHandler _animationEventHandler;

        // Use this for initialization
        void Start()
        {
            if (_armature != null)
            {
                _armature.AddDBEventListener(EventObject.START, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.LOOP_COMPLETE, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.COMPLETE, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.FADE_IN, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.FADE_IN_COMPLETE, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.FADE_OUT, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.FADE_OUT_COMPLETE, this.OnAnimationEvent);
                _armature.AddDBEventListener(EventObject.FRAME_EVENT, this.OnAnimationEvent);
            }
        }

        // Update is called once per frame
        void Update()
        { }

        private void OnAnimationEvent(string type, EventObject eventObject)
        {
            string anim = eventObject.animationState.name;
            /*string text = string.Format("animationName:{0}, eventType:{1}, eventName:{2}", anim, type, eventObject.name);
            Debug.Log(text);*/
            if (_animationEventHandler != null)
                _animationEventHandler(anim, type);
        }

        public void AddEventHandler(AnimationEventHandler del)
        {
            _animationEventHandler += del;
        }

        public void FadeIn(string anim, float time)
        {
            if (_armature != null)
                _armature.animation.FadeIn(anim, time);
        }
    }
}
