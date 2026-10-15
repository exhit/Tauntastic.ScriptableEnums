using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace Tauntastic
{
    /// <summary>Nested flags types of <see cref="ScriptableEnum"/>: serializable multi-selections of enum assets.</summary>
    public partial class ScriptableEnum
    {
        /// <summary>Non-generic base of the serialized multi-selection of ScriptableEnum values, exposing the enum type and the selected values.</summary>
        [Serializable]
        abstract public class Flags
        {
            abstract public Type Type { get; }
            abstract public List<ScriptableEnum> Values { get; set; }
        }

        /// <summary>Serialized multi-selection of ScriptableEnum assets of type T, convertible to a list of T, a list of ScriptableEnum or the enum Type.</summary>
        [Serializable]
        public class Flags<T> : Flags where T : ScriptableEnum
        {
            public List<T> TypedValues = new();
            public override Type Type => typeof(T);

            public override List<ScriptableEnum> Values
            {
                get => TypedValues.Cast<ScriptableEnum>().ToList();
                set => TypedValues = value.Cast<T>().ToList();
            }

            #region IMPLICIT

            public static implicit operator List<ScriptableEnum>(Flags<T> tFlags) => tFlags.Values;
            public static implicit operator List<T>(Flags<T> tFlags) => tFlags.Values.Cast<T>().ToList();
            public static implicit operator Type(Flags<T> tFlags) => tFlags.Type;

            #endregion
        }
    }
}