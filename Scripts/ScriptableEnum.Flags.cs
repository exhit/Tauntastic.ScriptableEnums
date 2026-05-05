using System;
using System.Collections.Generic;
using System.Linq;

namespace Tauntastic
{
    public partial class ScriptableEnum
    {
        [Serializable]
        abstract public class Flags
        {
            abstract public Type Type { get; }
            abstract public List<ScriptableEnum> Values { get; set; }
        }

        [Serializable]
        public class Flags<T> : Flags where T : ScriptableEnum
        {
            public List<T> TypedValuesSE = new();
            public override Type Type => typeof(T);

            public override List<ScriptableEnum> Values
            {
                get => TypedValuesSE.Cast<ScriptableEnum>().ToList();
                set => TypedValuesSE = value.Cast<T>().ToList();
            }

            #region IMPLICIT

            public static implicit operator List<ScriptableEnum>(Flags<T> tFlags) => tFlags.Values;
            public static implicit operator List<T>(Flags<T> tFlags) => tFlags.Values.Cast<T>().ToList();
            public static implicit operator Type(Flags<T> tFlags) => tFlags.Type;

            #endregion
        }
    }
}