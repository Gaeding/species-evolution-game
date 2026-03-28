using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesEvolution.Resources
{
    [Serializable]
    struct ResourceStartingAmount
    {
        public ResourceDefinition Definition;
        [Min(0f)] public float Amount;
    }

    /// <summary>
    /// Shell: authoritative resource storage, maps definitions to ids, raises change events.
    /// Rules live in <see cref="ResourceLedgerCore"/>.
    /// </summary>
    public sealed class ResourceSystem : MonoBehaviour
    {
        [SerializeField] List<ResourceStartingAmount> _startingAmounts = new();

        readonly Dictionary<ResourceId, float> _amounts = new();
        readonly Dictionary<ResourceId, ResourceDefinition> _definitionById = new();

        /// <summary>Fired with (definition, oldAmount, newAmount).</summary>
        public event Action<ResourceDefinition, float, float> AmountChanged;

        void Awake()
        {
            foreach (var entry in _startingAmounts)
            {
                if (entry.Definition == null)
                    continue;

                RegisterDefinition(entry.Definition);
                var id = entry.Definition.Id;
                if (id.IsEmpty)
                    continue;

                if (_amounts.ContainsKey(id))
                    _amounts[id] = entry.Amount;
                else
                    _amounts.Add(id, entry.Amount);
            }
        }

        public float GetAmount(ResourceDefinition definition)
        {
            if (definition == null)
                return 0f;

            var id = definition.Id;
            if (id.IsEmpty)
                return 0f;

            return ResourceLedgerCore.GetAmount(_amounts, id);
        }

        public void SetAmount(ResourceDefinition definition, float amount)
        {
            if (definition == null)
                return;

            RegisterDefinition(definition);
            var id = definition.Id;
            if (id.IsEmpty)
                return;

            var clamped = ResourceLedgerCore.ClampNonNegative(amount);
            var old = ResourceLedgerCore.GetAmount(_amounts, id);
            if (ResourceLedgerCore.NearlyEqual(old, clamped))
                return;

            _amounts[id] = clamped;
            AmountChanged?.Invoke(definition, old, clamped);
        }

        public void Add(ResourceDefinition definition, float delta)
        {
            if (definition == null || ResourceLedgerCore.NearlyEqual(delta, 0f))
                return;

            RegisterDefinition(definition);
            var id = definition.Id;
            if (id.IsEmpty)
                return;

            var old = ResourceLedgerCore.GetAmount(_amounts, id);
            var next = ResourceLedgerCore.ApplyDelta(old, delta);
            if (ResourceLedgerCore.NearlyEqual(old, next))
                return;

            _amounts[id] = next;
            AmountChanged?.Invoke(definition, old, next);
        }

        /// <summary>Subtracts cost if available. Returns false if insufficient.</summary>
        public bool TrySpend(ResourceDefinition definition, float cost)
        {
            if (definition == null)
                return cost <= 0f;

            if (cost <= 0f)
                return true;

            RegisterDefinition(definition);
            var id = definition.Id;
            if (id.IsEmpty)
                return false;

            var old = ResourceLedgerCore.GetAmount(_amounts, id);
            if (!ResourceLedgerCore.TrySpendSingle(old, cost, out var next))
                return false;

            if (ResourceLedgerCore.NearlyEqual(old, next))
                return true;

            _amounts[id] = next;
            AmountChanged?.Invoke(definition, old, next);
            return true;
        }

        /// <summary>Spends all listed costs in one check, or rolls back (no partial spend).</summary>
        public bool TrySpendAll(IReadOnlyList<ResourceSpendEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                return true;

            var costs = new List<ResourceCost>(entries.Count);
            for (var i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e.Definition == null || e.Amount <= 0f)
                    continue;

                RegisterDefinition(e.Definition);
                var id = e.Definition.Id;
                if (id.IsEmpty)
                    continue;

                costs.Add(new ResourceCost(id, e.Amount));
            }

            if (costs.Count == 0)
                return true;

            if (!ResourceLedgerCore.TrySpendAll(_amounts, costs, out var next))
                return false;

            ApplyDictionaryAndRaiseEvents(next);
            return true;
        }

        void ApplyDictionaryAndRaiseEvents(Dictionary<ResourceId, float> next)
        {
            foreach (var kvp in next)
            {
                var old = ResourceLedgerCore.GetAmount(_amounts, kvp.Key);
                if (ResourceLedgerCore.NearlyEqual(old, kvp.Value))
                    continue;

                _amounts[kvp.Key] = kvp.Value;
                if (_definitionById.TryGetValue(kvp.Key, out var def) && def != null)
                    AmountChanged?.Invoke(def, old, kvp.Value);
            }
        }

        void RegisterDefinition(ResourceDefinition definition)
        {
            if (definition == null)
                return;

            var id = definition.Id;
            if (id.IsEmpty)
                return;

            _definitionById[id] = definition;
        }
    }
}
