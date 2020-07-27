﻿using OpenTracker.Models.BossPlacements;
using OpenTracker.Models.UndoRedo;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace OpenTracker.ViewModels.BossSelect
{
    /// <summary>
    /// This is the ViewModel class for the boss select popup control.
    /// </summary>
    public class BossSelectPopupVM : ViewModelBase
    {
        private readonly IBossPlacement _bossPlacement;

        public ObservableCollection<BossSelectButtonVM> Buttons { get; }

        private bool _popupOpen;
        public bool PopupOpen
        {
            get => _popupOpen;
            set => this.RaiseAndSetIfChanged(ref _popupOpen, value);
        }

        public ReactiveCommand<BossType?, Unit> ChangeBossCommand { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bossPlacement">
        /// The marking to be represented.
        /// </param>
        /// <param name="buttons">
        /// The observable collection of boss select button control ViewModel instances.
        /// </param>
        public BossSelectPopupVM(
            IBossPlacement bossPlacement, ObservableCollection<BossSelectButtonVM> buttons)
        {
            _bossPlacement = bossPlacement ??
                throw new ArgumentNullException(nameof(bossPlacement));
            Buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));

            ChangeBossCommand = ReactiveCommand.Create<BossType?>(ChangeBoss);
        }

        /// <summary>
        /// Changes the boss of the section to the specified boss.
        /// </summary>
        /// <param name="boss">
        /// The boss to be set.
        /// </param>
        private void ChangeBoss(BossType? boss)
        {
            UndoRedoManager.Instance.Execute(new ChangeBoss(_bossPlacement, boss));
            PopupOpen = false;
        }
    }
}
