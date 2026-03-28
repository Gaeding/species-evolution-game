using System;
using System.Collections.Generic;
using SpeciesEvolution.Core;
using SpeciesEvolution.Phases;
using SpeciesEvolution.Resources;
using UnityEngine;

namespace SpeciesEvolution.Research
{
    /// <summary>
    /// Shell: unlocked tech set, spends resources via <see cref="ResourceSystem"/>, enforces phase and prerequisites via <see cref="ResearchLedgerCore"/>.
    /// </summary>
    public sealed class ResearchSystem : MonoBehaviour
    {
        [SerializeField] ResourceSystem _resourceSystem;
        [SerializeField] PhaseManager _phaseManager;
        [SerializeField] List<TechnologyDefinition> _startingUnlocked = new();

        readonly HashSet<TechnologyId> _unlocked = new();

        /// <summary>Fired after a technology is successfully researched and unlocked.</summary>
        public event Action<TechnologyDefinition> TechnologyResearched;

        void Awake()
        {
            foreach (var tech in _startingUnlocked)
                RegisterUnlocked(tech);
        }

        public IEnumerable<TechnologyId> UnlockedTechnologyIds => _unlocked;

        public bool IsUnlocked(TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            return ResearchLedgerCore.IsUnlocked(_unlocked, technology.Id);
        }

        /// <summary>
        /// Phase, prerequisites, and not already unlocked. Does not check resources.
        /// </summary>
        public bool AreRequirementsMet(TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            var phaseOk = IsPhaseRequirementMet(technology);
            var prereqIds = CollectPrerequisiteIds(technology);
            return ResearchLedgerCore.CanCompleteResearch(_unlocked, technology.Id, prereqIds, phaseOk);
        }

        /// <summary>
        /// Requirements plus sufficient resources (no spend). Free tech (no costs) returns true without a resource system.
        /// </summary>
        public bool CanAffordResearch(TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            var costs = technology.ResearchCosts;
            if (costs == null || costs.Length == 0)
                return true;

            if (_resourceSystem == null)
                return false;

            foreach (var entry in costs)
            {
                if (entry.Definition == null || entry.Amount <= 0f)
                    continue;

                if (_resourceSystem.GetAmount(entry.Definition) + ResourceLedgerCore.Epsilon < entry.Amount)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validates requirements, spends costs, unlocks tech, raises <see cref="TechnologyResearched"/>.
        /// </summary>
        public bool TryCompleteResearch(TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            var phaseOk = IsPhaseRequirementMet(technology);
            var prereqIds = CollectPrerequisiteIds(technology);
            if (!ResearchLedgerCore.CanCompleteResearch(_unlocked, technology.Id, prereqIds, phaseOk))
                return false;

            var costs = technology.ResearchCosts;
            var hasCosts = costs != null && costs.Length > 0;
            if (hasCosts)
            {
                if (_resourceSystem == null || !_resourceSystem.TrySpendAll(costs))
                    return false;
            }

            if (!_unlocked.Add(technology.Id))
                return true;

            TechnologyResearched?.Invoke(technology);
            return true;
        }

        bool IsPhaseRequirementMet(TechnologyDefinition technology)
        {
            var minimum = (int)technology.MinimumPhase;
            if (_phaseManager == null)
                return ResearchLedgerCore.IsPhaseRequirementMet(0, minimum);

            var current = (int)_phaseManager.CurrentPhase;
            return ResearchLedgerCore.IsPhaseRequirementMet(current, minimum);
        }

        static List<TechnologyId> CollectPrerequisiteIds(TechnologyDefinition technology)
        {
            var prereqs = technology.Prerequisites;
            var list = new List<TechnologyId>(prereqs.Length);
            foreach (var p in prereqs)
            {
                if (p == null)
                    continue;

                var id = p.Id;
                if (!id.IsEmpty)
                    list.Add(id);
            }

            return list;
        }

        void RegisterUnlocked(TechnologyDefinition technology)
        {
            if (technology == null)
                return;

            var id = technology.Id;
            if (id.IsEmpty)
                return;

            _unlocked.Add(id);
        }
    }
}
