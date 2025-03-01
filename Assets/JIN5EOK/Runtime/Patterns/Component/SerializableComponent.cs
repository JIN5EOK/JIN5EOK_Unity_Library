using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jin5eok.Patterns.Component
{
    [Serializable]
    public class SerializableComponentWrapper
    {
        [SerializeReference]
        public List<SerializableComponent> Components = new ();   
    }
    [Serializable]
    public abstract class SerializableComponent
    {
        public SerializableComponent() {}
    }
}