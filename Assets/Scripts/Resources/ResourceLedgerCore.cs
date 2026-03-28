using System;
using System.Collections.Generic;

namespace SpeciesEvolution.Resources
{
    /// <summary>
    /// Pure resource math: clamping, spend checks, multi-resource spend. No Unity dependencies.
    /// </summary>
    public static class ResourceLedgerCore
    {
        public const float Epsilon = 1e-6f;

        public static float ClampNonNegative(float value) => value < 0f ? 0f : value;

        public static bool NearlyEqual(float a, float b) => Math.Abs(a - b) < Epsilon;

        public static float GetAmount(IReadOnlyDictionary<ResourceId, float> amounts, ResourceId id)
        {
            if (id.IsEmpty)
                return 0f;
            return amounts != null && amounts.TryGetValue(id, out var v) ? v : 0f;
        }

        /// <summary>Result is non-negative.</summary>
        public static float ApplyDelta(float current, float delta) =>
            ClampNonNegative(current + delta);

        public static bool TrySpendSingle(float current, float cost, out float nextAmount)
        {
            nextAmount = current;
            if (cost <= 0f)
                return true;

            if (current + Epsilon < cost)
                return false;

            nextAmount = ClampNonNegative(current - cost);
            return true;
        }

        /// <summary>
        /// Verifies affordability for every positive cost, then returns a new dictionary with amounts deducted.
        /// A null <paramref name="costs"/> list is treated like empty (success, copy of <paramref name="current"/>).
        /// </summary>
        public static bool TrySpendAll(
            IReadOnlyDictionary<ResourceId, float> current,
            IReadOnlyList<ResourceCost> costs,
            out Dictionary<ResourceId, float> next)
        {
            next = null;
            if (current == null)
                return false;

            if (costs == null)
            {
                next = new Dictionary<ResourceId, float>(current);
                return true;
            }

            for (var i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c.ResourceId.IsEmpty || c.Amount <= 0f)
                    continue;

                var have = GetAmount(current, c.ResourceId);
                if (have + Epsilon < c.Amount)
                    return false;
            }

            next = new Dictionary<ResourceId, float>(current);
            for (var i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c.ResourceId.IsEmpty || c.Amount <= 0f)
                    continue;

                var have = GetAmount(next, c.ResourceId);
                next[c.ResourceId] = ClampNonNegative(have - c.Amount);
            }

            return true;
        }
    }
}
