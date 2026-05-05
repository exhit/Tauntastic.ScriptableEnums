using System.Collections.Generic;

namespace Tauntastic.ScriptableEnums.Editor
{
    public static class MaskUtils
    {
        public static int GetMaskFromIndices(List<int> indices, int totalOptionCount)
        {
            int mask = 0;

            for (int i = 0; i < totalOptionCount; i++)
                if (indices.Contains(i))
                    mask |= 1 << i;

            return mask;
        }
        
        /// <summary>
        /// Converts a bitmask integer from a MaskField into a list of indices.
        /// </summary>
        /// <param name="mask">The integer bitmask. A value of -1 represents "Everything".</param>
        /// <param name="totalOptionCount">The total number of options available in the mask field.</param>
        /// <returns>A list of zero-based indices for each selected option.</returns>
        public static List<int> GetIndicesFromMask(int mask, int totalOptionCount)
        {
            var indices = new List<int>();

            // Handle the "Everything" case where the mask is -1 (all bits set)
            if (mask == -1)
            {
                for (int i = 0; i < totalOptionCount; i++)
                    indices.Add(i);

                return indices;
            }

            // Iterate through each possible option to see if its corresponding bit is set
            // Check if the bit at position 'i' is set in the mask
            for (int i = 0; i < totalOptionCount; i++)
                if ((mask & (1 << i)) != 0)
                    indices.Add(i);

            return indices;
        }
    }
}