using Jin5eok.Patterns.Component;
using UnityEngine;

namespace Jin5eok.Samples
{
    public class ComponentContainerSample : MonoBehaviour
    {
        public class BaseFoo
        {
            public BaseFoo(string say)
            {
                this.say = say;
            }

            public string say;
        }
        public class SubFoo1 : BaseFoo
        {
            public SubFoo1(string say, bool subField) : base(say)
            {
                subField1 = subField;
            }

            public bool subField1;
        }
        public class SubFoo2 : BaseFoo
        {
            public SubFoo2(string say, int subField) : base(say)
            {
                subField2 = subField;
            }
            
            public int subField2;
        }
        
        public class SubFoo3 : BaseFoo
        {
            public SubFoo3() : base("I'm SubFoo3")
            {
                subField3 = Vector2.left;
            }
            
            public Vector2 subField3;
        }
        
        
        private ComponentContainer<BaseFoo> _fooContainer = new ComponentContainer<BaseFoo>();        
        void Start()
        {
            StartTest(_fooContainer);
        }
    
        public void StartTest(ComponentContainer<BaseFoo> fooContainer)
        {
            AddAndGetItem(fooContainer);
            GetInherited(fooContainer);
            Remove(fooContainer);
            Debug.Log("-----------------------------------------------------------------------");
        }
    
        public void AddAndGetItem(ComponentContainer<BaseFoo> fooContainer)
        {
            
            fooContainer.Add(new SubFoo1("I'm SubFoo1",true));
            fooContainer.Add(new SubFoo2("I'm SubFoo2", 500));
            fooContainer.Add<SubFoo3>();
            fooContainer.Add(new BaseFoo("I'm BaseFoo"));
            
            var subFoo1 = fooContainer.Get<SubFoo1>();
            var subFoo2 = fooContainer.Get<SubFoo2>();
            var subFoo3 = fooContainer.Get<SubFoo3>();
            var baseFoo = fooContainer.Get<BaseFoo>();

            Debug.Log($"Add {nameof(SubFoo1)} : {subFoo1.say}, {subFoo1.subField1}");
            Debug.Log($"Add {nameof(SubFoo2)} : {subFoo2.say}, {subFoo2.subField2}");
            Debug.Log($"Add {nameof(SubFoo3)} : {subFoo3.say}, {subFoo3.subField3}");
            Debug.Log($"Add {nameof(BaseFoo)} : {baseFoo.say}");
        }
    
        public void GetInherited(ComponentContainer<BaseFoo> fooContainer)
        {
            var inherited = fooContainer.GetInherited<BaseFoo>();
            Debug.Log($"Get one of inherited from {nameof(BaseFoo)} : {inherited.say}");
            
            var inheritedAll = fooContainer.GetInheritedAll<BaseFoo>();
            Debug.Log($"Get all items inherited from {nameof(BaseFoo)} : Count => {inheritedAll.Count}");
        }
        
        public void Remove(ComponentContainer<BaseFoo> fooContainer)
        {
            fooContainer.Remove<SubFoo1>();
            Debug.Log($"Remove {nameof(SubFoo1)} : current Count => {fooContainer.Count}");
            
            var removeCount = fooContainer.RemoveInheritedAll<BaseFoo>();
            Debug.Log($"Remove All inherited({removeCount}) : current count => {fooContainer.Count}");
        }
    }
}

