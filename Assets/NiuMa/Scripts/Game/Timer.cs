using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace NiuMa
{
    // 定时任务接口
    [CSharpCallLua]
    public interface ITimer
    {
        /**
         * 定时执行函数，返回true表示结束定时任务
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

        // 触发定时任务
        public bool Trigger()
        {
            if (_timer == null)
                return true;
            return _timer.OnTimer();
        }

        // 定时任务
        private ITimer _timer;

        // 延迟
        private float _delay;

        // 触发间隔
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