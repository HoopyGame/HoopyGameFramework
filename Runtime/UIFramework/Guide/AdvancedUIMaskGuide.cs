/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：降低DrawCall的按钮点击引导功能 
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HoopyGame
{
    public class AdvancedUIMaskGuide : MaskableGraphic, ICanvasRaycastFilter
    {
        private RectTransform _rectTransform;
        private RectTransform _guideTarget;

        private Vector3 _targetMin = Vector3.zero;
        private Vector3 _targetMax = Vector3.zero;

        private int _width;

        private int _spacing = 5;
        private float _duration = .75f;
        private bool _locked = false;
        private Color _maskColor = new Color(0,0,0,.78f);

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _rectTransform))
            {
                _locked = true;
            }
            color = new Color(0, 0, 0, 0);
        }
        #region 配置
        /// <summary>
        /// 设置当前遮罩的间距
        /// </summary>
        /// <param name="spacing">间距</param>
        /// <returns></returns>
        public AdvancedUIMaskGuide SetSpacing(int spacing = 0)
        {
            _spacing = spacing;
            return this;
        }
        /// <summary>
        /// 设置当前遮罩的颜色
        /// </summary>
        /// <param name="maskColor">颜色</param>
        /// <returns></returns>
        public AdvancedUIMaskGuide SetColor(Color maskColor)
        {
            _maskColor = maskColor;
            return this;
        }
        /// <summary>
        /// 设置当前遮罩的Alpha(0~255)
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public AdvancedUIMaskGuide SetAlpha(int alpha)
        {
            _maskColor.a = alpha / 255f;
            return this;
        }
        /// <summary>
        /// 设置当前遮罩的Alpha(0~1)
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public AdvancedUIMaskGuide SetAlpha(float alpha)
        {
            _maskColor.a = alpha;
            return this;
        }
        /// <summary>
        /// 设置遮罩缩放的持续时间
        /// </summary>
        /// <param name="duration">持续时间</param>
        /// <returns></returns>
        public AdvancedUIMaskGuide SetDuration(float duration)
        {
            _duration = duration;
            return this;
        }

        public AdvancedUIMaskGuide SetTotal(Color color, int spacing = 5, float duraton = .75f)
        {
            _maskColor = color;
            _spacing = spacing;
            _duration = duraton;
            return this;
        }
        #endregion

        public void StartGuide(RectTransform target, TweenCallback tweenCallback = null)
        {
            if (_locked) return;
            _guideTarget = target;
            _width = Screen.width;
            color = _maskColor;
            Play();
            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_rectTransform, _guideTarget);
            SetDrawData(bounds.min, bounds.max);
            SetAllDirty();
        }
        private void Play(TweenCallback tweenCallback = null)
        {
            DOTween.To(() => _width, width => _width = width, _spacing, _duration)
               .OnUpdate(() =>
               {
                   SetAllDirty();
               })
               .OnComplete(tweenCallback);
        }

        private void SetDrawData(Vector3 tarMin, Vector3 tarMax)
        {
            if (tarMin == _targetMin && tarMax == _targetMax)
                return;
            _targetMin = tarMin;
            _targetMax = tarMax;
            SetAllDirty();
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (_targetMin == Vector3.zero && _targetMax == Vector3.zero)
            {
                base.OnPopulateMesh(vh);
                return;
            }
            vh.Clear();
            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;
            Vector2 selfPiovt = rectTransform.pivot;
            Rect selfRect = rectTransform.rect;
            float outerLx = -selfPiovt.x * selfRect.width;
            float outerBy = -selfPiovt.y * selfRect.height;
            float outerRx = (1 - selfPiovt.x) * selfRect.width;
            float outerTy = (1 - selfPiovt.y) * selfRect.height;
            vert.position = new Vector3(outerLx, outerTy);
            vh.AddVert(vert);
            vert.position = new Vector3(outerRx, outerTy);
            vh.AddVert(vert);
            vert.position = new Vector3(outerRx, outerBy);
            vh.AddVert(vert);
            vert.position = new Vector3(outerLx, outerBy);
            vh.AddVert(vert);
            vert.position = new Vector3(_targetMin.x - _width, _targetMax.y + _width);
            vh.AddVert(vert);
            vert.position = new Vector3(_targetMax.x + _width, _targetMax.y + _width);
            vh.AddVert(vert);
            vert.position = new Vector3(_targetMax.x + _width, _targetMin.y - _width);
            vh.AddVert(vert);
            vert.position = new Vector3(_targetMin.x - _width, _targetMin.y - _width);
            vh.AddVert(vert);
            vh.AddTriangle(4, 0, 1);
            vh.AddTriangle(4, 1, 5);
            vh.AddTriangle(5, 1, 2);
            vh.AddTriangle(5, 2, 6);
            vh.AddTriangle(6, 2, 3);
            vh.AddTriangle(6, 3, 7);
            vh.AddTriangle(7, 3, 0);
            vh.AddTriangle(7, 0, 4);
        }
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (_guideTarget == null) return true;
            return !RectTransformUtility.RectangleContainsScreenPoint(_guideTarget, sp, eventCamera);
        }
    }
}