using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpeciesEvolution.Resources
{
    /// <summary>
    /// Tunable resource identity and display data. Create assets via Create menu.
    /// A stable id is generated once in the editor for saves and ledger keys.
    /// </summary>
    [CreateAssetMenu(fileName = "Resource", menuName = "Species Evolution/Resource Definition")]
    public sealed class ResourceDefinition : ScriptableObject
    {
        [SerializeField] string _displayName;
        [SerializeField, HideInInspector] string _stableId;

        public string DisplayName => string.IsNullOrEmpty(_displayName) ? name : _displayName;

        /// <summary>Stable id for saves and pure ledger operations.</summary>
        public ResourceId Id => ResourceId.From(string.IsNullOrWhiteSpace(_stableId) ? name : _stableId);

#if UNITY_EDITOR
        void Reset()
        {
            EnsureStableIdAssignedAndSaved();
        }

        void OnValidate()
        {
            EnsureStableIdAssignedAndSaved();
        }

        /// <summary>
        /// Assigns a GUID only when missing, then marks the asset dirty so it persists.
        /// Without SetDirty, the id can fail to save and regenerate on the next validation pass.
        /// </summary>
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
