using System.Collections;
using System.Collections.Generic;
using XLua;
using NiuMa;

namespace UnityEngine
{
    [LuaCallCSharp]
    public class VoiceListener : MonoBehaviour
    {
        private event System.Action<string> RecordCompletedHandler;
        private event System.Action<int> PlayStartHandler;
        private event System.Action<int> PlayStopHandler;

        // Use this for initialization
        private void Start()
        {
            VoiceManager.Instance.AddListener(this);
        }

        // Update is called once per frame
        private void Update()
        {}

        private void OnDestroy()
        {
            VoiceManager.Instance.RemoveListener(this);
        }

        [BlackList]
        public void OnRecordCompleted(string file)
        {
            if (RecordCompletedHandler != null)
                RecordCompletedHandler(file);
        }

        [BlackList]
        public void OnPlayStart(string fileName, int seat)
        {
            if (PlayStartHandler != null)
                PlayStartHandler(seat);
        }

        [BlackList]
        public void OnPlayStop(string fileName, int seat)
        {
            if (PlayStopHandler != null)
                PlayStopHandler(seat);
        }

        public void AddRecordCompletedHandler(System.Action<string> del)
        {
            RecordCompletedHandler += del;
        }

        public void AddPlayStartHandler(System.Action<int> del)
        {
            PlayStartHandler += del;
        }

        public void AddPlayStopHandler(System.Action<int> del)
        {
            PlayStopHandler += del;
        }
    }
}