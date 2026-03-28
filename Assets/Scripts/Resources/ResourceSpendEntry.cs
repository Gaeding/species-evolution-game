using System;
using UnityEngine;

namespace SpeciesEvolution.Resources
{
    /// <summary>
    /// Inspector-friendly spend line; the resource shell maps this to <see cref="ResourceCost"/>.
    /// </summary>
    [Serializable]
    public struct ResourceSpendEntry
    {
        public ResourceDefinition Definition;
        [Min(0f)] public float Amount;
    }
}
