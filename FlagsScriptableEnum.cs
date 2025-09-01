using System;
using System.Collections.Generic;

namespace Tauntastic
{
    [Serializable]
    abstract public class FlagsScriptableEnum
    {
        abstract public Type SEType { get; }
        abstract public List<ScriptableEnum> ValuesSE { get; set; }
    }
}