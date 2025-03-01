using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jin5eok.Patterns.Component
{
    public class ComponentContainer<T> : ComponentContainerBase<T>
    {
        protected override Dictionary<Type, T> ComponentsMap { get; set; }
    }
}