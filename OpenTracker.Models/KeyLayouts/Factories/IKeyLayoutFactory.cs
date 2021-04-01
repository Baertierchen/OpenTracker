﻿using System.Collections.Generic;
using OpenTracker.Models.Dungeons;

namespace OpenTracker.Models.KeyLayouts.Factories
{
    /// <summary>
    /// This interface contains creation logic for key layout data.
    /// </summary>
    public interface IKeyLayoutFactory
    {
        IList<IKeyLayout> GetDungeonKeyLayouts(IDungeon dungeon);
    }
}