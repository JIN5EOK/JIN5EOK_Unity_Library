using System.Collections.Generic;
using System.Linq;
using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class CompositeInputHandlerBase<T> : InputHandler<T> where T : notnull
    {
        public override event InputCallback<T> InputValueChanged;
        public override T Value { get; protected set; }
        
        private readonly List<IInputHandler<T>> _childInputs = new();
        
        public CompositeInputHandlerBase(params IInputHandler<T>[] inputGroup)
        {
            foreach (var input in inputGroup)
            {
                AddInput(input);
            }
        }

        public IInputHandler<T>[] GetInputs()
        {
            return _childInputs.ToArray();
        }
        
        public void AddInput(IInputHandler<T> inputHandler)
        {
            if (_childInputs.Contains(inputHandler) == false)
            {
                _childInputs.Add(inputHandler);   
            }
        }
        
        public void RemoveInput(IInputHandler<T> inputHandler)
        {
            if (_childInputs.Contains(inputHandler) == true)
            {
                _childInputs.Remove(inputHandler);
            }
        }

        public void RemoveAllInputs()
        {
            _childInputs.Clear();
        }

        public override void UpdateState()
        {
            if (_childInputs == null || _childInputs.Count == 0)
            {
                return;
            }
            
            // Cache a copy to prevent changes during iteration
            var inputGroup = _childInputs.ToList();

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

        public override void Dispose()
        {
            base.Dispose();
            foreach (var child in _childInputs)
            {
                child.Dispose();
            }
        }
    }
    
    public class IntCompositeInputHandler : CompositeInputHandlerBase<int>
    {
        public IntCompositeInputHandler(params IInputHandler<int>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(int a, int b)
        {
            return a == b;
        }
    }
    
    public class FloatCompositeInputHandler : CompositeInputHandlerBase<float>
    {
        public FloatCompositeInputHandler(params IInputHandler<float>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
    
    public class BoolCompositeInputHandler : CompositeInputHandlerBase<bool>
    {
        public BoolCompositeInputHandler(params IInputHandler<bool>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(bool a, bool b)
        {
            return a == b;
        }
    }
    
    public class Vector2CompositeInputHandler : CompositeInputHandlerBase<Vector2>
    {
        public Vector2CompositeInputHandler(params IInputHandler<Vector2>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector2 a, Vector2 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
    
    public class Vector3CompositeInputHandler : CompositeInputHandlerBase<Vector3>
    {
        public Vector3CompositeInputHandler(params IInputHandler<Vector3>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector3 a, Vector3 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
}