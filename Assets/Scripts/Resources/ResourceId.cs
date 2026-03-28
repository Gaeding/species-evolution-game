using System;

namespace SpeciesEvolution.Resources
{
    /// <summary>
    /// Stable runtime/save identity for a resource. Prefer the serialized id on <see cref="ResourceDefinition"/>.
    /// </summary>
    [Serializable]
    public readonly struct ResourceId : IEquatable<ResourceId>
    {
        public readonly string Value;

        public ResourceId(string value) => Value = value ?? string.Empty;

        public static ResourceId From(string value) => new ResourceId(value);

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public bool Equals(ResourceId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is ResourceId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public static bool operator ==(ResourceId a, ResourceId b) => a.Equals(b);

        public static bool operator !=(ResourceId a, ResourceId b) => !a.Equals(b);

        public override string ToString() => Value;
    }
}
