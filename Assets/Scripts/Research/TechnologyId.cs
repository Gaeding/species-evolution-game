using System;

namespace SpeciesEvolution.Research
{
    /// <summary>
    /// Stable runtime/save identity for a technology. Prefer the serialized id on <see cref="TechnologyDefinition"/>.
    /// </summary>
    [Serializable]
    public readonly struct TechnologyId : IEquatable<TechnologyId>
    {
        public readonly string Value;

        public TechnologyId(string value) => Value = value ?? string.Empty;

        public static TechnologyId From(string value) => new TechnologyId(value);

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public bool Equals(TechnologyId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is TechnologyId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public static bool operator ==(TechnologyId a, TechnologyId b) => a.Equals(b);

        public static bool operator !=(TechnologyId a, TechnologyId b) => !a.Equals(b);

        public override string ToString() => Value;
    }
}
