using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public delegate void InputCallback<T>(T input) where T : notnull;

    public interface IInputBase
    {
        public void UpdateState();
    }
    
    public interface IInput<T> : IInputBase where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; }
    }
    
    public class ButtonInputKey : IInput<bool>
    {
        public KeyCode KeyCode { get; set; }
        public event InputCallback<bool> InputValueChanged;
        public bool Value { get; private set; }

        public ButtonInputKey(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
        
        public void UpdateState()
        {
            var currentInput = Input.GetKeyDown(KeyCode) == true || Input.GetKey(KeyCode) == true;
            
            if (Value != currentInput)
            {
                Value = currentInput;
                InputValueChanged?.Invoke(currentInput);    
            }
        }
    }
    
    public abstract class VectorInputBase : IInput<Vector2>
    {
        public event InputCallback<Vector2> InputValueChanged;
        public Vector2 Value { get; private set; }

        public abstract void UpdateState();

        protected void UpdateState(Vector2 currentValue)
        {
            if (currentValue != Value)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public class VectorInputKey : VectorInputBase
    {
        public KeyCode Up { get; set; }
        public KeyCode Down { get; set; }
        public KeyCode Left { get; set; }
        public KeyCode Right { get; set; }

        public VectorInputKey(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
        
        public override void UpdateState()
        {
            var isHorizontalPositive = Input.GetKey(Right) == true && Input.GetKey(Left) == false;
            var isHorizontalNegative = Input.GetKey(Left) == true && Input.GetKey(Right) == false;
            var isVerticalPositive = Input.GetKey(Up) == true && Input.GetKey(Down) == false;
            var isVerticalNegative = Input.GetKey(Down) == true && Input.GetKey(Up) == false;
            
            float horizontal = isHorizontalPositive == true ? 1.0f : isHorizontalNegative ? -1.0f : 0.0f;
            float vertical = isVerticalPositive == true ? 1.0f : isVerticalNegative ? -1.0f : 0.0f;
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
  
    public class VectorInputAxis : VectorInputBase
    {
        public string HorizontalAxisName { get; set; }
        public string VerticalAxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        
        public VectorInputAxis(string horizontalAxisName, string verticalAxisName, bool isUsingAxisRaw = false)
        {
            HorizontalAxisName = horizontalAxisName;
            VerticalAxisName = verticalAxisName;
            IsUsingAxisRaw = isUsingAxisRaw;
        }
        
        public override void UpdateState()
        {
            var horizontal = IsUsingAxisRaw == true ? Input.GetAxisRaw(HorizontalAxisName) : Input.GetAxis(HorizontalAxisName);
            var vertical = IsUsingAxisRaw == true ? Input.GetAxisRaw(VerticalAxisName) : Input.GetAxis(VerticalAxisName);
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }

    public abstract class AxisInputBase : IInput<float>
    {
        public event InputCallback<float> InputValueChanged;
        public float Value { get; private set; }
        
        public abstract void UpdateState();
        
        protected void UpdateState(float currentValue)
        {
            if (Mathf.Approximately(Value, currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);   
            }
        }
    }
    
    public class AxisInputAxis : AxisInputBase
    {
        public string AxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        public AxisInputAxis(string axisName, bool isUsingAxisRaw = false)
        {
            AxisName = axisName;
            IsUsingAxisRaw = isUsingAxisRaw;
        }

        public override void UpdateState()
        {
            var currentValue = IsUsingAxisRaw == true ? Input.GetAxisRaw(AxisName) : Input.GetAxis(AxisName);
            UpdateState(currentValue);
        }
    }

    public class AxisInputKey : AxisInputBase
    {
        public KeyCode PositiveKey { get; set; }
        public KeyCode NegativeKey { get; set; }
        
        public AxisInputKey(KeyCode positiveKey, KeyCode negativeKey)
        {
            PositiveKey = positiveKey;
            NegativeKey = negativeKey;
        }

        public override void UpdateState()
        {
            var isNegative = Input.GetKey(NegativeKey) == true && Input.GetKey(PositiveKey) == false;
            var isPositive = Input.GetKey(PositiveKey) == true && Input.GetKey(NegativeKey) == false;
            var currentValue = isNegative ? -1 : isPositive ? 1 : 0;
            UpdateState(currentValue);
        }
    }

    public class CompositeInput<T> : IInput<T> where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; private set; }
        private List<IInput<T>> _inputGroup = new();
        
        public CompositeInput(params IInput<T>[] inputGroup)
        {
            foreach (var input in inputGroup)
            {
                AddInput(input);    
            }
        }

        public void AddInput(IInput<T> input)
        {
            if (_inputGroup.Contains(input) == false)
            {
                _inputGroup.Add(input);    
            }
        }
        
        public void RemoveInput(IInput<T> input)
        {
            if (_inputGroup.Contains(input) == true)
            {
                _inputGroup.Remove(input);    
            }
        }

        public void RemoveAllInputs()
        {
            _inputGroup.Clear();
        }
        
        public void UpdateState()
        {
            if (_inputGroup == null || _inputGroup.Count == 0)
            {
                return;
            }
            
            // Cache a copy to prevent changes during iteration
            var inputGroup = _inputGroup.ToList();
            
            foreach (var input in inputGroup)
            {
                input.UpdateState();
            }

            var oneOfInputs = inputGroup.First();
            if (oneOfInputs.Value.Equals(Value))
            {
                Value = oneOfInputs.Value;
                InputValueChanged?.Invoke(Value);
            }
        }
    }
}