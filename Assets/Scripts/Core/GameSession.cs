using System;
using SpeciesEvolution.Nation;
using SpeciesEvolution.Research;
using SpeciesEvolution.Resources;
using UnityEngine;

namespace SpeciesEvolution.Core
{
    /// <summary>
    /// Applies the selected nation after <see cref="ResourceSystem"/> Awake (overwrites matching resources).
    /// Persists nation choice via PlayerPrefs for menu → game flow.
    /// </summary>
    public sealed class GameSession : MonoBehaviour
    {
        public const string SelectedNationIdPrefsKey = "SpeciesEvolution.SelectedNationId";

        [SerializeField] ResourceSystem _resourceSystem;
        [SerializeField] ResearchSystem _researchSystem;
        [SerializeField] NationDefinition _defaultNation;
        [SerializeField] NationDefinition[] _availableNations = Array.Empty<NationDefinition>();

        NationDefinition _currentNation;

        /// <summary>Resolved in <see cref="Start"/>.</summary>
        public NationDefinition CurrentNation => _currentNation;

        void Start()
        {
            _currentNation = ResolveNation();
            ApplyNation(_currentNation);
        }

        /// <summary>Call from main menu before loading the game scene.</summary>
        public static void SetSelectedNationForNextSession(NationDefinition nation)
        {
            if (nation == null)
                return;

            var id = nation.Id.Value;
            if (string.IsNullOrEmpty(id))
                return;

            PlayerPrefs.SetString(SelectedNationIdPrefsKey, id);
            PlayerPrefs.Save();
        }

        public static void ClearSelectedNationPreference()
        {
            PlayerPrefs.DeleteKey(SelectedNationIdPrefsKey);
            PlayerPrefs.Save();
        }

        NationDefinition ResolveNation()
        {
            var saved = PlayerPrefs.GetString(SelectedNationIdPrefsKey, "");
            if (!string.IsNullOrEmpty(saved))
            {
                foreach (var n in _availableNations)
                {
                    if (n != null && n.Id.Value == saved)
                        return n;
                }
            }

            return _defaultNation;
        }

        void ApplyNation(NationDefinition nation)
        {
            if (nation == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: No nation resolved; skipping nation apply.", this);
                return;
            }

            if (_resourceSystem != null)
            {
                foreach (var stock in nation.StartingResources)
                {
                    if (stock.Definition == null)
                        continue;

                    _resourceSystem.SetAmount(stock.Definition, stock.Amount);
                }
            }

            if (_researchSystem != null)
                _researchSystem.RegisterStartingUnlocks(nation.StartingTechnologies);
        }
    }
}
