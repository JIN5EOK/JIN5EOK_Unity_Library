using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Jin5eok.Extension;
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

    public abstract class ButtonInputBase : IInput<bool>
    {
        public abstract void UpdateState();

        public event InputCallback<bool> InputValueChanged;
        public bool Value { get; private set; }
        
        protected void UpdateState(bool currentValue)
        {
            if (currentValue != Value)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public class ButtonInputKeyCode : ButtonInputBase
    {
        public KeyCode KeyCode { get; set; }
        
        public ButtonInputKeyCode(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }

        public override void UpdateState()
        {
            var currentInput = Input.GetKey(KeyCode) == true && Input.GetKeyUp(KeyCode) == false;
            UpdateState(currentInput);
        }
    }
    
    public class ButtonInputOldInputSystem : ButtonInputBase
    {
        public string ButtonName { get; set; }
        
        public ButtonInputOldInputSystem(string buttonName)
        {
            ButtonName = buttonName;
        }

        public override void UpdateState()
        {
            var currentInput = Input.GetButton(ButtonName) == true && Input.GetButtonUp(ButtonName) == false;
            UpdateState(currentInput);
        }
    }
    
    public abstract class VectorInputBase : IInput<Vector2>
    {
        public event InputCallback<Vector2> InputValueChanged;
        public Vector2 Value { get; private set; }

        public abstract void UpdateState();

        protected void UpdateState(Vector2 currentValue)
        {
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public class VectorInputKeyCode : VectorInputBase
    {
        public KeyCode Up { get; set; }
        public KeyCode Down { get; set; }
        public KeyCode Left { get; set; }
        public KeyCode Right { get; set; }

        public VectorInputKeyCode(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
        
        public override void UpdateState()
        {
            var isHorizontalPositive = Input.GetKey(Right) == true && Input.GetKeyUp(Right) == false && Input.GetKey(Left) == false;
            var isHorizontalNegative = Input.GetKey(Left) == true  && Input.GetKeyUp(Left) == false && Input.GetKey(Right) == false;
            var isVerticalPositive = Input.GetKey(Up) == true && Input.GetKeyUp(Up) == false  && Input.GetKey(Down) == false;
            var isVerticalNegative = Input.GetKey(Down) == true && Input.GetKeyUp(Down) == false  && Input.GetKey(Up) == false;
            
            float horizontal = isHorizontalPositive == true ? 1.0f : isHorizontalNegative == true ? -1.0f : 0.0f;
            float vertical = isVerticalPositive == true ? 1.0f : isVerticalNegative == true ? -1.0f : 0.0f;
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
  
    public class VectorInputOldInputSystem : VectorInputBase
    {
        public string HorizontalAxisName { get; set; }
        public string VerticalAxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        
        public VectorInputOldInputSystem(string horizontalAxisName, string verticalAxisName, bool isUsingAxisRaw = false)
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
    
    public class AxisInputKeyCode : AxisInputBase
    {
        public KeyCode PositiveKey { get; set; }
        public KeyCode NegativeKey { get; set; }
        
        public AxisInputKeyCode(KeyCode positiveKey, KeyCode negativeKey)
        {
            PositiveKey = positiveKey;
            NegativeKey = negativeKey;
        }

        public override void UpdateState()
        {
            var isNegative = Input.GetKey(NegativeKey) == true && Input.GetKeyUp(NegativeKey) == false && Input.GetKey(PositiveKey) == false;
            var isPositive = Input.GetKey(PositiveKey) == true && Input.GetKeyUp(PositiveKey) == false && Input.GetKey(NegativeKey) == false;
            var currentValue = isNegative ? -1 : isPositive ? 1 : 0;
            UpdateState(currentValue);
        }
    }
    
    public class AxisInputOldInputSystem : AxisInputBase
    {
        public string AxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        public AxisInputOldInputSystem(string axisName, bool isUsingAxisRaw = false)
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
    
    public class MousePositionInput : IInput<Vector3>
    {
        public event InputCallback<Vector3> InputValueChanged;
        public Vector3 Value { get; private set; }
        
        public void UpdateState()
        {
            var currentValue = Input.mousePosition;
            
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public abstract class CompositeInput<T> : IInput<T> where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; protected set; }
        private List<IInput<T>> _inputGroup = new();
        private IInput<T> _currentActiveInput;
        
        public CompositeInput(params IInput<T>[] inputGroup)
        {
            foreach (var input in inputGroup)
            {
                AddInput(input);    
            }
        }

        public IInput<T>[] GetInputs()
        {
            return _inputGroup.ToArray();
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

            T changedValue = default;
            foreach (var input in inputGroup)
            {
                input.UpdateState();
                if (IsEquals(input.Value, default) == false)
                {
                    changedValue = input.Value;
                }
            }

            var isNoInputCurrent = changedValue.Equals(default);
            var isNoInputBefore = Value.Equals(default);
            var isInputChanged = changedValue.Equals(Value) == false;
            
            if (isNoInputCurrent && isNoInputBefore)
            {
                return;
            }
            
            if (isNoInputCurrent)
            {
                Value = default;
                InputValueChanged?.Invoke(default);
                return;
            }
            
            if (isInputChanged)
            {
                Value = changedValue;
                InputValueChanged?.Invoke(changedValue);
                return;
            }
        }
        
        protected abstract bool IsEquals(T a, T b);
    }
    
    public class CompositeInputBool : CompositeInput<bool>
    {
        public CompositeInputBool(params IInput<bool>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(bool a, bool b)
        {
            return a == b;
        }
    }
    
    public class CompositeInputVector2 : CompositeInput<Vector2>
    {
        public CompositeInputVector2(params IInput<Vector2>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector2 a, Vector2 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
    
    public class CompositeInputVector3 : CompositeInput<Vector3>
    {
        public CompositeInputVector3(params IInput<Vector3>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector3 a, Vector3 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
    
    public class CompositeInputFloat : CompositeInput<float>
    {
        public CompositeInputFloat(params IInput<float>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
}