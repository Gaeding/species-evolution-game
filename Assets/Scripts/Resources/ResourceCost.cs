namespace SpeciesEvolution.Resources
{
    /// <summary>
    /// Pure-data cost entry for ledger rules (no Unity objects).
    /// </summary>
    public readonly struct ResourceCost
    {
        public readonly ResourceId ResourceId;
        public readonly float Amount;

        public ResourceCost(ResourceId resourceId, float amount)
        {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}
