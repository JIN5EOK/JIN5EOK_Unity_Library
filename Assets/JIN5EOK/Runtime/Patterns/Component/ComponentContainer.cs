using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jin5eok.Patterns
{
    public class ComponentContainer<T> : ComponentContainerBase<T>
    {
        protected override Dictionary<Type, T> ComponentsMap { get; set; } = new ();
    }
}