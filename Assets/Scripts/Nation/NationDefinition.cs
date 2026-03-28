using SpeciesEvolution.Research;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpeciesEvolution.Nation
{
    /// <summary>
    /// Playable nation: starting resources, optional starting techs, tunable multipliers for future systems.
    /// </summary>
    [CreateAssetMenu(fileName = "Nation", menuName = "Species Evolution/Nation Definition")]
    public sealed class NationDefinition : ScriptableObject
    {
        [SerializeField] string _displayName;
        [SerializeField, TextArea] string _description;
        [SerializeField, HideInInspector] string _stableId;
        [SerializeField] NationResourceStock[] _startingResources = System.Array.Empty<NationResourceStock>();
        [SerializeField] TechnologyDefinition[] _startingTechnologies = System.Array.Empty<TechnologyDefinition>();
        [SerializeField, Min(0.01f)] float _productionMultiplier = 1f;
        [SerializeField, Min(0.01f)] float _researchSpeedMultiplier = 1f;

        public string DisplayName => string.IsNullOrEmpty(_displayName) ? name : _displayName;

        public string Description => _description ?? string.Empty;

        public NationId Id => NationId.From(string.IsNullOrWhiteSpace(_stableId) ? name : _stableId);

        public NationResourceStock[] StartingResources => _startingResources ?? System.Array.Empty<NationResourceStock>();

        public TechnologyDefinition[] StartingTechnologies =>
            _startingTechnologies ?? System.Array.Empty<TechnologyDefinition>();

        /// <summary>Future: production / tick systems multiply by this.</summary>
        public float ProductionMultiplier => _productionMultiplier;

        /// <summary>Future: research pacing multiplies by this.</summary>
        public float ResearchSpeedMultiplier => _researchSpeedMultiplier;

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
