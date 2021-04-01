﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTracker.Models.SaveLoad;
using OpenTracker.Utils;

namespace OpenTracker.Models.Dropdowns
{
    /// <summary>
    ///     This class contains the dictionary container of dropdowns.
    /// </summary>
    public class DropdownDictionary : LazyDictionary<DropdownID, IDropdown>,
        IDropdownDictionary
    {
        private readonly Lazy<IDropdownFactory> _factory;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="factory">
        ///     A factory for creating new dropdowns.
        /// </param>
        public DropdownDictionary(IDropdownFactory.Factory factory) : base(new Dictionary<DropdownID, IDropdown>())
        {
            _factory = new Lazy<IDropdownFactory>(() => factory());
        }

        protected override IDropdown Create(DropdownID key)
        {
            return _factory.Value.GetDropdown(key);
        }

        public void Reset()
        {
            foreach (var dropdown in Values)
            {
                dropdown.Reset();
            }
        }

        /// <summary>
        ///     Returns a dictionary of dropdown save data.
        /// </summary>
        /// <returns>
        ///     A dictionary of dropdown save data.
        /// </returns>
        public IDictionary<DropdownID, DropdownSaveData> Save()
        {
            return Keys.ToDictionary(
                type => type, type => this[type].Save());
        }

        /// <summary>
        ///     Loads a dictionary of dropdown save data.
        /// </summary>
        /// <param name="saveData">
        ///     The save data to be loaded.
        /// </param>
        public void Load(IDictionary<DropdownID, DropdownSaveData>? saveData)
        {
            if (saveData is null)
            {
                return;
            }
            
            foreach (var item in saveData.Keys)
            {
                this[item].Load(saveData[item]);
            }
        }
    }
}
