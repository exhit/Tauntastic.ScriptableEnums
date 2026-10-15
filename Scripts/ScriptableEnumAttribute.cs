using System;
using UnityEngine;

namespace Tauntastic
{
    /// <summary>Draws a ScriptableObject field or property as a dropdown of all assets of its type, like a ScriptableEnum.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ScriptableEnumAttribute : PropertyAttribute
    {
        
    }
}