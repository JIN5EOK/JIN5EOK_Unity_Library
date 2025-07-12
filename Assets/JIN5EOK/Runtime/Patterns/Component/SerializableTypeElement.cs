using System;
using System.Collections.Generic;

namespace Jin5eok.Patterns
{
    [Serializable]
    public class SerializableTypeElementWrapper
    {
        public List<SerializableTypeElement> TypeElements = new ();   
    }
    [Serializable]
    public abstract class SerializableTypeElement
    {
        public SerializableTypeElement() {}
    }
}