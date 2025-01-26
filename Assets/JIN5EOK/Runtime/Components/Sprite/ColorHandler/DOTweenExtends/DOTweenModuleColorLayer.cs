
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/13

#if DOTWEEN // MODULE_MARKER
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591

namespace Jin5eok.Sprite.DOTween
{
	public static class DOTweenModuleSprite
    {
        #region Shortcuts

        #region SpriteRenderer

        /// <summary>Tweens a SpriteRenderer's color to the given value.
        /// Dotween의 SpriteRenderer, Image등에 확장 메서드로 구현되어 있는 것 가져와 구현</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this ColorLayer target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DG.Tweening.DOTween.To(() => target.Color, x => target.Color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        /// <summary>Tweens a Material's alpha color to the given value.
        /// Dotween의 SpriteRenderer, Image등에 확장 메서드로 구현되어 있는 것 가져와 구현 </summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this ColorLayer target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DG.Tweening.DOTween.ToAlpha(() => target.Color, x => target.Color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        /// <summary>Tweens a SpriteRenderer's color using the given gradient
        /// Dotween의 SpriteRenderer, Image등에 확장 메서드로 구현되어 있는 것 가져와 구현
        /// Dotween의 SpriteRenderer, Image등에 확장 메서드로 구현되어 있는 것 가져와 구현</summary>
        /// <param name="gradient">The gradient to use</param><param name="duration">The duration of the tween</param>
        public static Sequence DOGradientColor(this ColorLayer target, Gradient gradient, float duration)
        {
            Sequence s = DG.Tweening.DOTween.Sequence();
            GradientColorKey[] colors = gradient.colorKeys;
            int len = colors.Length;
            for (int i = 0; i < len; ++i) {
                GradientColorKey c = colors[i];
                if (i == 0 && c.time <= 0) {
                    target.Color = c.color;
                    continue;
                }
                float colorDuration = i == len - 1
                    ? duration - s.Duration(false) // Verifies that total duration is correct
                    : duration * (i == 0 ? c.time : c.time - colors[i - 1].time);
                s.Append(target.DOColor(c.color, colorDuration).SetEase(Ease.Linear));
            }
            s.SetTarget(target);
            return s;
        }

        #endregion

        #region Blendables

        #region SpriteRenderer

        /// <summary>Tweens a SpriteRenderer's color to the given value,
        /// in a way that allows other DOBlendableColor tweens to work together on the same target,
        /// instead than fight each other as multiple DOColor would do.
        /// Also stores the SpriteRenderer as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The value to tween to</param><param name="duration">The duration of the tween</param>
        public static Tweener DOBlendableColor(this ColorLayer target, Color endValue, float duration)
        {
            endValue = endValue - target.Color;
            Color to = new Color(0, 0, 0, 0);
            return DG.Tweening.DOTween.To(() => to, x => {
                    Color diff = x - to;
                    to = x;
                    target.Color += diff;
                }, endValue, duration)
                .Blendable().SetTarget(target);
        }

        #endregion

        #endregion

        #endregion
	}
}
#endif
