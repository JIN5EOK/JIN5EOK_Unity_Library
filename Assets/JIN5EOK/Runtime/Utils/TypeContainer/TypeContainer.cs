using System;
using System.Collections.Generic;

namespace Jin5eok
{
    public class TypeContainer<T> : TypeContainerBase<T>
    {
        protected override Dictionary<Type, T> TypeMap { get; set; } = new ();
    }
}