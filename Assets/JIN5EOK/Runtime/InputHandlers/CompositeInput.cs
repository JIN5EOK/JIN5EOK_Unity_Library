using System.Collections.Generic;
using System.Linq;
using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class CompositeInputHandlerBase<T> : IInputHandler<T> where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; protected set; }
        private List<IInputHandler<T>> _inputGroup = new();
        private IInputHandler<T> _currentActiveInputHandler;
        
        public CompositeInputHandlerBase(params IInputHandler<T>[] inputGroup)
        {
            foreach (var input in inputGroup)
            {
                AddInput(input);    
            }
        }

        public IInputHandler<T>[] GetInputs()
        {
            return _inputGroup.ToArray();
        }
        
        public void AddInput(IInputHandler<T> inputHandler)
        {
            if (_inputGroup.Contains(inputHandler) == false)
            {
                _inputGroup.Add(inputHandler);    
            }
        }
        
        public void RemoveInput(IInputHandler<T> inputHandler)
        {
            if (_inputGroup.Contains(inputHandler) == true)
            {
                _inputGroup.Remove(inputHandler);    
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

            var isInputBefore = Value.Equals(null) == false;
            var isInputCurrent = changedValue != null && changedValue.Equals(null) == false;
            var isInputChanged = changedValue != null && changedValue.Equals(Value) == false;
            
            if (isInputBefore == false && isInputCurrent == false)
            {
                return;
            }
            
            if (isInputBefore == true && isInputCurrent == false)
            {
                Value = default;
                InputValueChanged?.Invoke(Value);
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
    
    public class CompositeInputHandlerInt : CompositeInputHandlerBase<int>
    {
        public CompositeInputHandlerInt(params IInputHandler<int>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(int a, int b)
        {
            return a == b;
        }
    }
    
    public class CompositeInputHandlerFloat : CompositeInputHandlerBase<float>
    {
        public CompositeInputHandlerFloat(params IInputHandler<float>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
    
    public class CompositeInputHandlerBool : CompositeInputHandlerBase<bool>
    {
        public CompositeInputHandlerBool(params IInputHandler<bool>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(bool a, bool b)
        {
            return a == b;
        }
    }
    
    public class CompositeInputHandlerVector2 : CompositeInputHandlerBase<Vector2>
    {
        public CompositeInputHandlerVector2(params IInputHandler<Vector2>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector2 a, Vector2 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
    
    public class CompositeInputHandlerVector3 : CompositeInputHandlerBase<Vector3>
    {
        public CompositeInputHandlerVector3(params IInputHandler<Vector3>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector3 a, Vector3 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
}