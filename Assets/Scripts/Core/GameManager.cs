using SpeciesEvolution.Phases;
using SpeciesEvolution.Research;
using SpeciesEvolution.Resources;
using UnityEngine;

namespace SpeciesEvolution.Core
{
    /// <summary>
    /// Scene-level coordinator: wires core systems. Other features should subscribe to
    /// <see cref="PhaseManager"/>, <see cref="ResourceSystem"/>, and <see cref="ResearchSystem"/> rather than referencing this class.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] PhaseManager _phaseManager;
        [SerializeField] ResourceSystem _resourceSystem;
        [SerializeField] ResearchSystem _researchSystem;

        public PhaseManager Phases => _phaseManager;
        public ResourceSystem Resources => _resourceSystem;
        public ResearchSystem Research => _researchSystem;

        void Awake()
        {
            if (_phaseManager == null)
                _phaseManager = FindFirstObjectByType<PhaseManager>();
            if (_resourceSystem == null)
                _resourceSystem = FindFirstObjectByType<ResourceSystem>();
            if (_researchSystem == null)
                _researchSystem = FindFirstObjectByType<ResearchSystem>();

            if (_phaseManager == null)
                Debug.LogError($"{nameof(GameManager)}: Assign or add a {nameof(PhaseManager)}.", this);
            if (_resourceSystem == null)
                Debug.LogError($"{nameof(GameManager)}: Assign or add a {nameof(ResourceSystem)}.", this);
        }
    }
}
