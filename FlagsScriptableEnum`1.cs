using System;
using System.Collections.Generic;
using System.Linq;

namespace Tauntastic
{
    [Serializable]
    public class FlagsScriptableEnum<T> : FlagsScriptableEnum where T : ScriptableEnum
    {
        public List<T> TypedValuesSE = new();
        public override Type SEType => typeof(T);
        public override List<ScriptableEnum> ValuesSE
        {
            get => TypedValuesSE.Cast<ScriptableEnum>().ToList();
            set => TypedValuesSE = value.Cast<T>().ToList();
        }
    }
}