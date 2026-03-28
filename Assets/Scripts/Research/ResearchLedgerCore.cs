using System.Collections.Generic;

namespace SpeciesEvolution.Research
{
    /// <summary>
    /// Pure research rules: prerequisites and phase gating. No Unity dependencies.
    /// </summary>
    public static class ResearchLedgerCore
    {
        /// <summary>
        /// True when <paramref name="currentPhase"/> is at least <paramref name="minimumPhase"/> (enum order).
        /// </summary>
        public static bool IsPhaseRequirementMet(int currentPhase, int minimumPhase) =>
            currentPhase >= minimumPhase;

        public static bool IsUnlocked(IEnumerable<TechnologyId> unlocked, TechnologyId techId)
        {
            if (techId.IsEmpty || unlocked == null)
                return false;

            foreach (var id in unlocked)
            {
                if (id == techId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Null or empty <paramref name="prerequisiteIds"/> is satisfied.
        /// </summary>
        public static bool ArePrerequisitesSatisfied(
            IEnumerable<TechnologyId> unlocked,
            IReadOnlyList<TechnologyId> prerequisiteIds)
        {
            if (prerequisiteIds == null || prerequisiteIds.Count == 0)
                return true;

            if (unlocked == null)
                return false;

            for (var i = 0; i < prerequisiteIds.Count; i++)
            {
                var p = prerequisiteIds[i];
                if (p.IsEmpty)
                    continue;

                if (!IsUnlocked(unlocked, p))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Ready to complete research: phase ok, not already unlocked, prerequisites met.
        /// Does not validate resources.
        /// </summary>
        public static bool CanCompleteResearch(
            IEnumerable<TechnologyId> unlocked,
            TechnologyId techId,
            IReadOnlyList<TechnologyId> prerequisiteIds,
            bool phaseRequirementMet)
        {
            if (techId.IsEmpty || !phaseRequirementMet)
                return false;

            if (IsUnlocked(unlocked, techId))
                return false;

            return ArePrerequisitesSatisfied(unlocked, prerequisiteIds);
        }
    }
}
