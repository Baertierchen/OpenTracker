﻿using OpenTracker.Interfaces;
using OpenTracker.Models.Items;
using OpenTracker.Models.UndoRedo;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Globalization;

namespace OpenTracker.ViewModels.UIPanels.ItemsPanel.LargeItems
{
    /// <summary>
    /// This is the ViewModel for the large Items panel control representing a pair of items.
    /// </summary>
    public class PairLargeItemVM : LargeItemVMBase, IClickHandler
    {
        private readonly IItem[] _items;
        private readonly string _imageSourceBase;

        public string ImageSource =>
            _imageSourceBase +
            ((_items[1].Current * (_items[0].Maximum + 1)) + _items[0].Current)
            .ToString(CultureInfo.InvariantCulture) + ".png";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">
        /// An array of items that are to be represented by this control.
        /// </param>
        public PairLargeItemVM(IItem[] items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));

            if (_items.Length != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(items));
            }

            foreach (var item in _items)
            {
                if (item == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(items));
                }

                item.PropertyChanged += OnItemChanged;
            }

            _imageSourceBase = "avares://OpenTracker/Assets/Images/Items/" +
                _items[0].Type.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the IItem interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnItemChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(ImageSource));
        }

        /// <summary>
        /// Handles left clicks and cycles the first item.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnLeftClick(bool force)
        {
            UndoRedoManager.Instance.Execute(new CycleItem(_items[0]));
        }

        /// <summary>
        /// Handles right clicks and cycles the second item.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnRightClick(bool force)
        {
            UndoRedoManager.Instance.Execute(new CycleItem(_items[1]));
        }
    }
}
