using UnityEngine;

namespace Tauntastic.ScriptableEnums
{
    /// <summary>Color extension that converts a Color to a hexadecimal string.</summary>
    public static class ColorUtils
    {
        public static string ToHexString(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}