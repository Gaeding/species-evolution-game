using System;
using SpeciesEvolution.Resources;
using UnityEngine;

namespace SpeciesEvolution.Nation
{
    /// <summary>
    /// Serialized starting stock for a nation (applied over scene defaults in <see cref="GameSession"/>).
    /// </summary>
    [Serializable]
    public struct NationResourceStock
    {
        public ResourceDefinition Definition;
        [Min(0f)] public float Amount;
    }
}
