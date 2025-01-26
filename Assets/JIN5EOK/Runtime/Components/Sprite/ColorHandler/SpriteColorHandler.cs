using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok.Sprite
{
    public class ColorLayer : IDisposable
    {
        private SpriteColorHandler _parent;
        private Color _color;
        
        internal ColorLayer(Color color, SpriteColorHandler parent)
        {
            _parent = parent;
            Color = color;
        }
        
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _parent?.UpdateColor();
            }
        }
        
        public void Dispose()
        {
            _parent?.RemoveMultiplyColor(this);
        }

        ~ColorLayer()
        {
            Dispose();   
        }
    }
    
    public class SpriteColorHandler : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private List<ColorLayer> _colorLayers = new List<ColorLayer>();
        
        public void Init(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }
        
        public ColorLayer GetColorLayer()
        {
            var layer = new ColorLayer(Color.white, this);
            _colorLayers.Add(layer);
            return layer;
        }
        
        internal void RemoveMultiplyColor(ColorLayer target)
        {
            if (_colorLayers.Contains(target) == true)
            {
                _colorLayers.Remove(target);
                UpdateColor();
            }
        }
        
        public void UpdateColor()
        {
            float r = 1;
            float g = 1;
            float b = 1;
            float a = 1;
            
            foreach (var colorLayer in _colorLayers)
            {
                float multiplyR = colorLayer.Color.r;
                float multiplyG = colorLayer.Color.g;
                float multiplyB = colorLayer.Color.b;
                float multiplyA = colorLayer.Color.a;
                
                r *= multiplyR;
                g *= multiplyG;
                b *= multiplyB;
                a *= multiplyA;
            }
            _spriteRenderer.color = new Color(r, g, b, a);
        }
    }
}