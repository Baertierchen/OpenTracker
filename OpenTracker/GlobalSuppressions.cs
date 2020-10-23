﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "WebSocketSharp requires URI in string format.", Scope = "member", Target = "~P:OpenTracker.ViewModels.AutoTrackerDialogVM.UriString")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "StringComparison is unnecessary for this method.", Scope = "member", Target = "~M:OpenTracker.ViewLocator.Build(System.Object)~Avalonia.Controls.IControl")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.BossSelect.BossSelectButtonVM.ImageSource")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Items.Large.LargeItemControlVMFactory.GetCrystalRequirementLargeItemVM(OpenTracker.ViewModels.Items.Large.LargeItemType)~OpenTracker.ViewModels.Items.Large.LargeItemVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Items.Large.LargeItemControlVMFactory.GetLargeItemControlVM(OpenTracker.ViewModels.Items.Large.LargeItemType)~OpenTracker.ViewModels.Items.Large.LargeItemVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Items.Large.LargeItemControlVMFactory.GetPairLargeItemVM(OpenTracker.ViewModels.Items.Large.LargeItemType)~OpenTracker.ViewModels.Items.Large.LargeItemVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Items.Large.LargeItemControlVMFactory.GetPrizeLargeItemControlVM(OpenTracker.ViewModels.Items.Large.LargeItemType)~OpenTracker.ViewModels.Items.Large.LargeItemVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Items.Large.LargeItemControlVMFactory.GetSmallKeyLargeItemVM(OpenTracker.ViewModels.Items.Large.LargeItemType)~OpenTracker.ViewModels.Items.Large.LargeItemVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.Items.Small.BossSmallItemVM.ImageSource")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.Items.Small.PrizeSmallItemVM.ImageSource")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.Maps.MapVM.ImageSource")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~M:OpenTracker.ViewModels.Markings.Images.MarkingImageFactory.GetMarkingImageVM(OpenTracker.Models.Markings.MarkType)~OpenTracker.ViewModels.Markings.Images.MarkingImageVMBase")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.PinnedLocations.SectionIcons.BossSectionIconVM.ImageSource")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Filenames are normalized to lowercase.", Scope = "member", Target = "~P:OpenTracker.ViewModels.PinnedLocations.SectionIcons.PrizeSectionIconVM.ImageSource")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "IDataTemplate requires non-static member.", Scope = "member", Target = "~P:OpenTracker.ViewLocator.SupportsRecycling")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Non-static property is required for databinding.", Scope = "member", Target = "~P:OpenTracker.ViewModels.Items.Small.SmallItemPanelVM.ATItemsVisible")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Non-static property is required for databinding.", Scope = "member", Target = "~P:OpenTracker.ViewModels.ModeSettingsVM.StandardOpenWorldState")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.AccessibilityInspectColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.AccessibilityNoneColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.AccessibilityNormalColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.AccessibilityPartialColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.AccessibilitySequenceBreakColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.ConnectorColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ColorSelect.ColorSelectDialog.EmphasisFontColorPickerOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.MainWindow.CurrentFilePathProperty")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "AvaloniaProperty must be public static field.", Scope = "member", Target = "~F:OpenTracker.Views.ModeSettings.ModeSettingsPopupOpenProperty")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Collections in save data classes must be writable to allow for deserialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveLoad.AppSettingsSaveData.AccessibilityColors")]
