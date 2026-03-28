using SpeciesEvolution.Phases;
using SpeciesEvolution.Resources;
using UnityEngine;

namespace SpeciesEvolution.Core
{
    /// <summary>
    /// Scene-level coordinator: wires core systems. Other features should subscribe to
    /// <see cref="PhaseManager"/> and <see cref="ResourceSystem"/> rather than referencing this class.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] PhaseManager _phaseManager;
        [SerializeField] ResourceSystem _resourceSystem;

        public PhaseManager Phases => _phaseManager;
        public ResourceSystem Resources => _resourceSystem;

        void Awake()
        {
            if (_phaseManager == null)
                _phaseManager = FindFirstObjectByType<PhaseManager>();
            if (_resourceSystem == null)
                _resourceSystem = FindFirstObjectByType<ResourceSystem>();

            if (_phaseManager == null)
                Debug.LogError($"{nameof(GameManager)}: Assign or add a {nameof(PhaseManager)}.", this);
            if (_resourceSystem == null)
                Debug.LogError($"{nameof(GameManager)}: Assign or add a {nameof(ResourceSystem)}.", this);
        }
    }
}
