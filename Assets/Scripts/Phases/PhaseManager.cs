using System;
using SpeciesEvolution.Core;
using UnityEngine;

namespace SpeciesEvolution.Phases
{
    /// <summary>
    /// Owns the current game phase and notifies listeners when it changes.
    /// </summary>
    public sealed class PhaseManager : MonoBehaviour
    {
        [SerializeField] GamePhase _initialPhase = GamePhase.Planetary;

        GamePhase _current;

        public GamePhase CurrentPhase => _current;

        /// <summary>Fired with (previous, next) after the phase has changed.</summary>
        public event Action<GamePhase, GamePhase> PhaseChanged;

        void Awake()
        {
            _current = _initialPhase;
        }

        /// <summary>Moves to the next phase in order if not already at the last.</summary>
        /// <returns>True if the phase advanced.</returns>
        public bool TryAdvancePhase()
        {
            if (_current >= GamePhase.Interstellar)
                return false;

            SetPhase(_current + 1);
            return true;
        }

        public void SetPhase(GamePhase next)
        {
            if (_current == next)
                return;

            var previous = _current;
            _current = next;
            PhaseChanged?.Invoke(previous, _current);
        }
    }
}
