using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XLua;

namespace NiuMa
{
    /**
     * ÕðÆÁÐ§¹û
     */
    [LuaCallCSharp]
    public class ShakeScreen : MonoBehaviour
    {
        [SerializeField]
        private Transform content;

        public void Shake()
        {
            if (content == null)
                return;
            content.DOPunchPosition(new Vector3(0, 5, 0), 0.5f);
        }
    }
}