﻿using OpenTracker.Models.Items;

namespace OpenTracker.Models.PrizePlacements
{
    /// <summary>
    /// This is the class for creating prize placement classes.
    /// </summary>
    internal static class PrizePlacementFactory
    {
        /// <summary>
        /// Returns a new prize placement instance for the specified prize placement ID.
        /// </summary>
        /// <param name="id">
        /// The prize placement ID.
        /// </param>
        /// <returns>
        /// A new prize placement instance.
        /// </returns>
        internal static IPrizePlacement GetPrizePlacement(PrizePlacementID id)
        {
            switch (id)
            {
                case PrizePlacementID.ATPrize:
                    {
                        return new PrizePlacement(ItemDictionary.Instance[ItemType.Aga1]);
                    }
                case PrizePlacementID.GTPrize:
                    {
                        return new PrizePlacement(ItemDictionary.Instance[ItemType.Aga2]);
                    }
            }

            return new PrizePlacement();
        }
    }
}
