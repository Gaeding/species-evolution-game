using SpeciesEvolution.Core;
using SpeciesEvolution.Resources;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpeciesEvolution.Research
{
    /// <summary>
    /// Tunable technology: prerequisites, research cost, minimum phase. Content is data; unlock flow is <see cref="ResearchSystem"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "Technology", menuName = "Species Evolution/Technology Definition")]
    public sealed class TechnologyDefinition : ScriptableObject
    {
        [SerializeField] string _displayName;
        [SerializeField, TextArea] string _description;
        [SerializeField, HideInInspector] string _stableId;
        [SerializeField] GamePhase _minimumPhase = GamePhase.Planetary;
        [SerializeField] TechnologyDefinition[] _prerequisites;
        [SerializeField] ResourceSpendEntry[] _researchCosts;

        public string DisplayName => string.IsNullOrEmpty(_displayName) ? name : _displayName;

        public string Description => _description ?? string.Empty;

        public TechnologyId Id => TechnologyId.From(string.IsNullOrWhiteSpace(_stableId) ? name : _stableId);

        public GamePhase MinimumPhase => _minimumPhase;

        public TechnologyDefinition[] Prerequisites => _prerequisites ?? System.Array.Empty<TechnologyDefinition>();

        public ResourceSpendEntry[] ResearchCosts => _researchCosts ?? System.Array.Empty<ResourceSpendEntry>();

#if UNITY_EDITOR
        void Reset() => EnsureStableIdAssignedAndSaved();

        void OnValidate() => EnsureStableIdAssignedAndSaved();

        void EnsureStableIdAssignedAndSaved()
        {
            if (!string.IsNullOrWhiteSpace(_stableId))
                return;

            _stableId = System.Guid.NewGuid().ToString("N");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
