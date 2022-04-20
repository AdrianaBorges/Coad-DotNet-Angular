using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Interfaces
{
    /// <summary>
    /// Define uma interface de cópia
    /// </summary>
    public interface IPrototype<T> : ICloneable<T>
    {
        new T Clone();
    }

    public interface IPrototype : IPrototype<object>
    {
    }

    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
