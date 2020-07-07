using System;
using SkriptKit.Core.Interfaces;

namespace SkriptKit.Core.Objects
{
    public abstract class Builder<T>
    {
        public abstract T Build();
    }
}