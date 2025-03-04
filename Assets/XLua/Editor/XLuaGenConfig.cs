/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XLua;
using System.Linq;
using System.Reflection;
using DG.Tweening;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class XLuaGenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
                typeof(System.Object),
                typeof(UnityEngine.Object),
                typeof(UnityAction),
                typeof(UnityAction<bool>),
                typeof(UnityAction<string>),
                typeof(UnityAction<float>),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Quaternion),
                typeof(Color),
                typeof(Ray),
                typeof(Bounds),
                typeof(Ray2D),
                typeof(Time),
                typeof(GameObject),
                typeof(Component),
                typeof(Behaviour),
                typeof(Transform),
                typeof(RectTransform),
                typeof(Resources),
                typeof(TextAsset),
                typeof(Sprite),
                typeof(Keyframe),
                typeof(AnimationCurve),
                typeof(AnimationClip),
                typeof(Animator),
                typeof(AudioSource),
                typeof(AudioClip),
                typeof(MonoBehaviour),
                typeof(ParticleSystem),
                typeof(Renderer),
                typeof(Texture2D),
                typeof(Light),
                typeof(Mathf),
                typeof(String),
                typeof(Action<int>),
                typeof(Action<string>),
                typeof(Action<Texture2D>),
                typeof(Tween),
                typeof(Tweener),
                typeof(TweenCallback),
                typeof(Ease),
                typeof(RotateMode),
                typeof(DragonBones.DragonBoneControl),
                typeof(DragonBones.AnimationEventHandler)
            };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(Action),
                typeof(Action<int>),
                typeof(Action<string>),
                typeof(Action<double>),
                typeof(Action<int, string>),
                typeof(Action<Texture2D>),
                typeof(Func<double, double, double>),
                typeof(UnityAction),
                typeof(UnityAction<bool>),
                typeof(UnityAction<string>),
                typeof(UnityAction<float>),
                typeof(System.Collections.IEnumerator)
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.AudioSource", "PlayOnGamepad", "System.Int32"},
                new List<string>(){"UnityEngine.AudioSource", "DisableGamepadOutput"},
                new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerMixLevel", "System.Int32", "System.Int32"},
                new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerMixLevelDefault", "System.Int32"},
                new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerRestrictedAudio", "System.Int32", "System.Boolean"},
                new List<string>(){"UnityEngine.AudioSource", "gamepadSpeakerOutputType"},
                new List<string>(){"UnityEngine.AudioSource", "GamepadSpeakerSupportsOutputType", "UnityEngine.GamepadSpeakerOutputType"},
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
//#if UNITY_ANDROID
                new List<string>(){"UnityEngine.Light", "SetLightDirty"},
                new List<string>(){"UnityEngine.Light", "shadowRadius"},
                new List<string>(){"UnityEngine.Light", "shadowAngle"},
//#endif
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
            };
    
    public static List<Type> BlackGenericTypeList = new List<Type>()
    {
        typeof(Span<>),
        typeof(ReadOnlySpan<>),
    };

    private static bool IsBlacklistedGenericType(Type type)
    {
        if (!type.IsGenericType) return false;
        return BlackGenericTypeList.Contains(type.GetGenericTypeDefinition());
    }

    [BlackList]
    public static Func<MemberInfo, bool> GenericTypeFilter = (memberInfo) =>
    {
        switch (memberInfo)
        {
            case PropertyInfo propertyInfo:
                return IsBlacklistedGenericType(propertyInfo.PropertyType);

            case ConstructorInfo constructorInfo:
                return constructorInfo.GetParameters().Any(p => IsBlacklistedGenericType(p.ParameterType));

            case MethodInfo methodInfo:
                return methodInfo.GetParameters().Any(p => IsBlacklistedGenericType(p.ParameterType));

            default:
                return false;
        }
    };
}
