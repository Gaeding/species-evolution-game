using System;

namespace SpeciesEvolution.Nation
{
    /// <summary>
    /// Stable save/menu identity for a nation. Prefer the serialized id on <see cref="NationDefinition"/>.
    /// </summary>
    [Serializable]
    public readonly struct NationId : IEquatable<NationId>
    {
        public readonly string Value;

        public NationId(string value) => Value = value ?? string.Empty;

        public static NationId From(string value) => new NationId(value);

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public bool Equals(NationId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is NationId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public static bool operator ==(NationId a, NationId b) => a.Equals(b);

        public static bool operator !=(NationId a, NationId b) => !a.Equals(b);

        public override string ToString() => Value;
    }
}
