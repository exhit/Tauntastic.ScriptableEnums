using System;
using UnityEngine;

namespace Tauntastic
{
    /// <summary>
    /// Turn any ScriptableObject property into a ScriptableEnum style property
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ScriptableEnumAttribute : PropertyAttribute
    {
        
    }
}