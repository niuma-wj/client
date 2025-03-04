using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主线程调度器
/// </summary>
public static class UnityMainThreadDispatcher
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface Task
    {
        void execute();
    }

    /// <summary>
    /// 基本动作任务
    /// </summary>
    public class ActionTask : Task
    {
        private Action _action = null;

        public ActionTask(Action action)
        {
            _action = action;
        }

        public void execute()
        {
            if (_action != null)
                _action();
        }
    }

    // 任务队列
    private static Queue<Task> _tasks = new Queue<Task>();

    public static void Update()
    {
        lock (_tasks)
        {
            while (_tasks.Count > 0)
            {
                Task task = _tasks.Dequeue();
                task.execute();
            }
        }
    }

    public static void Enqueue(Task task)
    {
        lock (_tasks)
        {
            _tasks.Enqueue(task);
        }
    }

    public static void Enqueue(Action action)
    {
        lock (_tasks)
        {
            _tasks.Enqueue(new ActionTask(action));
        }
    }
}