using System;
using UnityEngine;

namespace Tauntastic.ScriptableEnums
{
    /// <summary>Shows a ScriptableEnum field in the Inspector but disables it so it cannot be edited.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ScriptableEnumsDisableAttribute : PropertyAttribute { }
}