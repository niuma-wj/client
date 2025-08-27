using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace NiuMa
{
    // ��ʱ����ӿ�
    [CSharpCallLua]
    public interface ITimer
    {
        /**
         * ��ʱִ�к���������true��ʾ������ʱ����
         */
        bool OnTimer();
    }

    public class TimerWrapper
    {
        public TimerWrapper(ITimer timer, float delay, float interval = -1.0f)
        {
            _timer = timer;
            _delay = delay;
            _interval = interval;
        }

        // ������ʱ����
        public bool Trigger()
        {
            if (_timer == null)
                return true;
            return _timer.OnTimer();
        }

        // ��ʱ����
        private ITimer _timer;

        // �ӳ�
        private float _delay;

        // �������
        private float _interval;

        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public float Interval
        {
            get { return _interval; }
        }
    }
}