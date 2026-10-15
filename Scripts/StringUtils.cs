namespace Tauntastic.ScriptableEnums
{
    /// <summary>String extension that wraps an object's text in a rich text color tag.</summary>
    public static class StringUtils
    {
        public static string Color(this object msg, UnityEngine.Color color)
        {
            return $"<color=#{color.ToHexString()}>{msg}</color>";
        }
    }
}