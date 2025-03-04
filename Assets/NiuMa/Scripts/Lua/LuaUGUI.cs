using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XLua;

namespace NiuMa
{
    [LuaCallCSharp]
    public static class LuaUGUI
    {
        public static void AddBtnClick(GameObject obj, UnityAction luaFunc)
        {
            if (obj == null || luaFunc == null)
                return;
            Button btn = obj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(luaFunc);
        }

        public static void AddBtnAction(GameObject obj, System.Action<int> del, int param)
        {
            if (obj == null || del == null)
                return;
            Button btn = obj.GetComponent<Button>();
            if (btn == null)
                return;
            btn.onClick.AddListener(new UnityEngine.Events.UnityAction(() => { del(param); }));
        }

        public static void AddToggleClick(GameObject obj, UnityAction<bool> luaFunc)
        {
            if (obj == null || luaFunc == null)
                return;
            Toggle tog = obj.GetComponent<Toggle>();
            if (tog != null)
                tog.onValueChanged.AddListener(luaFunc);
        }

        public static void SetText(GameObject obj, string str)
        {
            if (obj == null)
                return;
            Text t = obj.GetComponent<Text>();
            if (t != null)
                t.text = str;
        }

        public static string GetText(GameObject obj)
        {
            if (obj == null)
                return "";
            string ret = "";
            Text t = obj.GetComponent<Text>();
            if (t != null)
                ret = t.text;
            return ret;
        }

        public static void SetTextColor(GameObject obj, Color clr)
        {
            if (obj == null)
                return;
            Text t = obj.GetComponent<Text>();
            if (t != null)
                t.color = clr;
        }

        public static void GetTextPreferredSize(GameObject obj, out float width, out float height)
        {
            width = 0.0f;
            height = 0.0f;
            if (obj == null)
                return;
            Text t = obj.GetComponent<Text>();
            if (t != null)
            {
                width = t.preferredWidth;
                height = t.preferredHeight;
            }
        }

        public static void SetTexture(GameObject obj, Texture2D tex)
        {
            if (obj == null)
                return;
            RawImage ri = obj.GetComponent<RawImage>();
            if (ri != null)
                ri.texture = tex;
        }

        public static void SetTextureColor(GameObject obj, Color clr)
        {
            if (obj == null)
                return;
            RawImage ri = obj.GetComponent<RawImage>();
            if (ri != null)
                ri.color = clr;
        }

        public static void SetImage(GameObject obj, Sprite sp, bool nativeSize = true)
        {
            if (obj == null)
                return;
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sp;
                if (nativeSize)
                    img.SetNativeSize();
            }
        }

        public static void SetImageTexture(GameObject obj, Texture2D tex)
        {
            if (obj == null)
                return;
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                Sprite sp = null;
                if (tex != null)
                    sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                img.sprite = sp;
            }
        }

        public static void SetImageColor(GameObject obj, Color clr)
        {
            if (obj == null)
                return;
            Image img = obj.GetComponent<Image>();
            if (img != null)
                img.color = clr;
        }

        public static bool GetToggleOn(GameObject obj)
        {
            if (obj == null)
                return false;
            Toggle tog = obj.GetComponent<Toggle>();
            if (tog == null)
                return false;
            return tog.isOn;
        }

        public static void SetToggleOn(GameObject obj, bool s)
        {
            if (obj == null)
                return;
            Toggle tog = obj.GetComponent<Toggle>();
            if (tog == null)
                return;
            tog.isOn = s;
        }

        public static string GetInputText(GameObject obj)
        {
            if (obj == null)
                return "";
            string text = "";
            InputField input = obj.GetComponent<InputField>();
            if (input != null)
                text = input.text;
            return text;
        }

        public static void SetInputText(GameObject obj, string text)
        {
            if (obj == null)
                return;
            InputField input = obj.GetComponent<InputField>();
            if (input != null)
                input.text = text;
        }

        public static bool GetInputFocused(GameObject obj)
        {
            if (obj == null)
                return false;
            InputField input = obj.GetComponent<InputField>();
            if (input != null)
                return input.isFocused;
            return false;
        }

        public static void SetInputChangeHandler(GameObject obj, UnityAction<string> del)
        {
            if (obj == null)
                return ;
            InputField input = obj.GetComponent<InputField>();
            if (input != null)
                input.onValueChanged.AddListener(del);
        }

        public static void SetSliderChangeHandler(GameObject obj, UnityAction<float> del)
        {
            if (obj == null)
                return;
            Slider slider = obj.GetComponent<Slider>();
            if (slider != null)
                slider.onValueChanged.AddListener(del);
        }

        public static void SetSliderValue(GameObject obj, float val)
        {
            if (obj == null)
                return;
            Slider slider = obj.GetComponent<Slider>();
            if (slider != null)
                slider.value = val;
        }

        public static void AddScroll2EndHandler(GameObject obj, UnityAction del)
        {
            if (obj == null)
                return;
            DragScrollRect sr = obj.GetComponent<DragScrollRect>();
            if (sr != null)
                sr.AddScroll2EndHanlder(del);
        }

        public static Vector2 WorldPointToLocalPointInRectangle(RectTransform rcTrans, Vector3 pos)
        {
            if (rcTrans == null)
                return new Vector2(pos.x, pos.y);
            Vector2 anchoredPosition = new Vector2(0.0f, 0.0f);
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rcTrans, screenPos, Camera.main, out anchoredPosition);
            return anchoredPosition;
        }
    }
}