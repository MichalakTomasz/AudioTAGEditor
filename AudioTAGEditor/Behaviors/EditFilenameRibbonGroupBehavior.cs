using System.Linq;
using System.Windows.Controls.Ribbon;
using System.Windows.Interactivity;

namespace AudioTAGEditor.Behaviors
{
    public class EditFilenameRibbonGroupBehavior
        : Behavior<RibbonTab>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var ribbonTab = AssociatedObject;
            if (ribbonTab != null)
            {
                ribbonGroupCut = ribbonTab.Items
                    .Cast<RibbonGroup>()?
                    .FirstOrDefault(c => c.Header.ToString() == "Cut");

                if (ribbonGroupCut != null)
                {
                    cutSpace = ribbonGroupCut.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Space");
                    cutSpace.Checked += CutSpace_Checked;
                    cutSpace.Unchecked += CutSpace_Unchecked;

                    cutDot = ribbonGroupCut.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Dot");
                    cutDot.Checked += CutDot_Checked;
                    cutDot.Unchecked += CutDot_Unchecked;

                    cutUnderscore = ribbonGroupCut.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Underscore");
                    cutUnderscore.Checked += CutUnderscore_Checked;
                    cutUnderscore.Unchecked += CutUnderscore_Unchecked;

                    cutDash = ribbonGroupCut.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Dash");
                    cutDash.Checked += CutDash_Checked;
                    cutDash.Unchecked += CutDash_Unchecked;
                }

                ribbonGroupReplaceToSpace = ribbonTab.Items
                    .Cast<RibbonGroup>()?
                    .FirstOrDefault(g => g.Header.ToString() == 
                    "Replace to space");
                
                if (ribbonGroupReplaceToSpace != null)
                {
                    dotToSpace = ribbonGroupReplaceToSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Dot to space");
                    dotToSpace.Checked += DotToSpace_Checked;
                    dotToSpace.Unchecked += DotToSpace_Unchecked;

                    underscoreToSpace = ribbonGroupReplaceToSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Underscore to space");
                    underscoreToSpace.Checked += UnderscoreToSpace_Checked;
                    underscoreToSpace.Unchecked += UnderscoreToSpace_Unchecked;

                    dashToSpace = ribbonGroupReplaceToSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Dash to space");
                    dashToSpace.Checked += DashToSpace_Checked;
                    dashToSpace.Unchecked += DashToSpace_Unchecked;
                }

                ribbonGroupReplaceFromSpace = ribbonTab.Items
                                    .Cast<RibbonGroup>()?
                                    .FirstOrDefault(g => g.Header.ToString() == 
                                    "Replace from space");

                if (ribbonGroupReplaceFromSpace != null)
                {
                    spaceToDot = ribbonGroupReplaceFromSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Space to dot");
                    spaceToDot.Checked += SpaceToDot_Checked;
                    spaceToDot.Unchecked += SpaceToDot_Unchecked;

                    spaceToUnderscore = ribbonGroupReplaceFromSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Space to underscore");
                    spaceToUnderscore.Checked += SpaceToUnderscore_Checked;
                    spaceToUnderscore.Unchecked += SpaceToUnderscore_Unchecked;

                    spaceToDash = ribbonGroupReplaceFromSpace.Items
                        .Cast<RibbonCheckBox>()?
                        .FirstOrDefault(c => c.Label == "Space to Dash");
                    spaceToDash.Checked += SpaceToDash_Checked;
                    spaceToDash.Unchecked += SpaceToDash_Unchecked;
                }

                ribbonGroupChange = ribbonTab.Items
                                    .Cast<RibbonGroup>()?
                                    .FirstOrDefault(g => g.Header.ToString() ==
                                    "Change");

                if (ribbonGroupChange != null)
                {
                    firstCapitalLetter = ribbonGroupChange.Items
                        .Cast<RibbonRadioButton>()?
                        .FirstOrDefault(c => c.Label == "First capital letter");

                    allFirstCapitalLetters = ribbonGroupChange.Items
                        .Cast<RibbonRadioButton>()?
                        .FirstOrDefault(c => c.Label == "All first capital letters");

                    upperCase = ribbonGroupChange.Items
                        .Cast<RibbonRadioButton>()?
                        .FirstOrDefault(c => c.Label == "Upper case");

                    lowerCase = ribbonGroupChange.Items
                        .Cast<RibbonRadioButton>()?
                        .FirstOrDefault(c => c.Label == "Lower case");
                }

                ribbonGroupInsertTextFromPosition = ribbonTab.Items
                                    .Cast<RibbonGroup>()?
                                    .FirstOrDefault(g => g.Header.ToString() ==
                                    "Insert from position");

                if (ribbonGroupInsertTextFromPosition != null)
                {
                    insertFromPositionPosition = ribbonGroupInsertTextFromPosition.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label == "Position");
                    insertFromPositionPosition.TextChanged += 
                        InsertFromPositionPosition_TextChanged;

                    insertFormPositionText = ribbonGroupInsertTextFromPosition.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label.Trim() == "Text");
                    insertFormPositionText.TextChanged += InsertFormPositionText_TextChanged;
                }

                ribbonGroupCutTextFromPosition = ribbonTab.Items
                                    .Cast<RibbonGroup>()?
                                    .FirstOrDefault(g => g.Header.ToString() ==
                                    "Cut text from position");

                if (ribbonGroupCutTextFromPosition != null)
                {
                    cutFromPositionPosition = ribbonGroupCutTextFromPosition.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label == "Position");
                    cutFromPositionPosition.TextChanged += CutFromPositionPosition_TextChanged;

                    cutFromPositionCount = ribbonGroupCutTextFromPosition.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label.Trim() == "Count");
                    cutFromPositionCount.TextChanged += CutFromPositionCount_TextChanged;
                }

                ribbonGroupCutText = ribbonTab.Items
                                    .Cast<RibbonGroup>()?
                                    .FirstOrDefault(g => g.Header.ToString() ==
                                    "Cut text");

                if (ribbonGroupCutText != null)
                {
                    cutText = ribbonGroupCutText.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label == "Text to cut");
                    cutText.TextChanged += CutText_TextChanged;
                }

                ribbonGroupReplaceText = ribbonTab.Items
                                   .Cast<RibbonGroup>()?
                                   .FirstOrDefault(g => g.Header.ToString() ==
                                   "Replace text");

                if (ribbonGroupReplaceText != null)
                {
                    replaceTextOldText = ribbonGroupReplaceText.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label == "Text to cut");
                    replaceTextOldText.TextChanged += ReplaceTextOldText_TextChanged;

                    replaceTextNewText = ribbonGroupReplaceText.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label.Trim() == "New text");
                    replaceTextNewText.TextChanged += ReplaceTextNewText_TextChanged;
                }

                ribbonGroupInsertNumbering = ribbonTab.Items
                                   .Cast<RibbonGroup>()?
                                   .FirstOrDefault(g => g.Header.ToString() ==
                                   "Insert numbering");

                if (ribbonGroupInsertNumbering != null)
                {
                    insertNumberingPosition = ribbonGroupInsertNumbering.Items
                        .Cast<RibbonTextBox>()?
                        .FirstOrDefault(c => c.Label == "Position");
                    insertNumberingPosition.TextChanged += InsertNumberingPosition_TextChanged;
                }

                ribbonGroupChangeFromID3 = ribbonTab.Items
                                   .Cast<RibbonGroup>()?
                                   .FirstOrDefault(g => g.Header.ToString() ==
                                   "Change from ID3");

                if (ribbonGroupChangeFromID3 != null)
                {
                    ChangeFromID3Pattern = ribbonGroupChangeFromID3.Items
                        .Cast<RibbonComboBox>()?
                        .FirstOrDefault(c => c.Label == "Pattern");
                    ChangeFromID3Pattern.PreviewKeyUp += ChangeFromID3Pattern_PreviewKeyUp;
                }
            }
        }

        #region Cut dot group

        private void CutSpace_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanCutGroup = false;

        private void CutSpace_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyFromCutGroup)
                AreEnabledAnotherThanCutGroup = true;
        }

        private void CutDot_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanCutGroup = false;

        private void CutDot_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyFromCutGroup)
                AreEnabledAnotherThanCutGroup = true;
        }

        private void CutUnderscore_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanCutGroup = false;

        private void CutUnderscore_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyFromCutGroup)
                AreEnabledAnotherThanCutGroup = true;
        }

        private void CutDash_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanCutGroup = false;

        private void CutDash_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyFromCutGroup)
                AreEnabledAnotherThanCutGroup = true;
        }

        private bool AreEnabledAnotherThanCutGroup
        {
            set
            {
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        private bool IsCheckedAnyFromCutGroup
            => (cutSpace.IsChecked.Value ||
                    cutDot.IsChecked.Value ||
                    cutUnderscore.IsChecked.Value ||
                    cutDash.IsChecked.Value);

        #endregion // Cut dot group

        #region Replace to space group

        private void DotToSpace_Checked(object sender, System.Windows.RoutedEventArgs e)
            =>AreEnabledAnotherThanReplaceToSpaceGroup = false;

        private void DotToSpace_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceToSpaceGroup)
                AreEnabledAnotherThanReplaceToSpaceGroup = true;
        }

        private void UnderscoreToSpace_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanReplaceToSpaceGroup = false;

        private void UnderscoreToSpace_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceToSpaceGroup)
                AreEnabledAnotherThanReplaceToSpaceGroup = true;
        }

        private void DashToSpace_Checked(object sender, System.Windows.RoutedEventArgs e)
            =>AreEnabledAnotherThanReplaceToSpaceGroup = false;

        private void DashToSpace_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceToSpaceGroup)
                AreEnabledAnotherThanReplaceToSpaceGroup = true;
        }

        private bool AreEnabledAnotherThanReplaceToSpaceGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        private bool IsCheckedAnyReplaceToSpaceGroup
            => (dotToSpace.IsChecked.Value ||
                    underscoreToSpace.IsChecked.Value ||
                    dashToSpace.IsChecked.Value);

        #endregion // Replace to space group

        #region Replace from space group

        private void SpaceToDot_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanReplaceFromSpaceGroup = false;

        private void SpaceToDot_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceFromSpaceGroup)
                AreEnabledAnotherThanReplaceFromSpaceGroup = true;
        }

        private void SpaceToUnderscore_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanReplaceFromSpaceGroup = false;

        private void SpaceToUnderscore_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceFromSpaceGroup)
                AreEnabledAnotherThanReplaceFromSpaceGroup = true;
        }

        private void SpaceToDash_Checked(object sender, System.Windows.RoutedEventArgs e)
            => AreEnabledAnotherThanReplaceFromSpaceGroup = false;

        private void SpaceToDash_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsCheckedAnyReplaceFromSpaceGroup)
                AreEnabledAnotherThanReplaceFromSpaceGroup = true;
        }

        private bool AreEnabledAnotherThanReplaceFromSpaceGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        private bool IsCheckedAnyReplaceFromSpaceGroup
            => (spaceToDot.IsChecked.Value ||
                    spaceToUnderscore.IsChecked.Value ||
                    spaceToDash.IsChecked.Value);

        #endregion // Replace from space group

        #region Insert from position

        private void InsertFromPositionPosition_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanInsetTextFromPosition();

        private void InsertFormPositionText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanInsetTextFromPosition();

        private void ActivateDesactivateOtherThanInsetTextFromPosition()
        {
            if (!string.IsNullOrWhiteSpace(insertFromPositionPosition.Text) ||
                !string.IsNullOrWhiteSpace(insertFormPositionText.Text))
                AreEnabledAnotherThanInsertFromPositionGroup = false;
            else AreEnabledAnotherThanInsertFromPositionGroup = true;
        }
        
        private bool AreEnabledAnotherThanInsertFromPositionGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        #endregion //Isert from position

        #region Cut from position

        private void CutFromPositionPosition_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanCutFromPosition();

        private void CutFromPositionCount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanCutFromPosition();

        private void ActivateDesactivateOtherThanCutFromPosition()
        {
            if (!string.IsNullOrWhiteSpace(cutFromPositionPosition.Text) ||
                !string.IsNullOrWhiteSpace(cutFromPositionCount.Text))
                AreEnabledAnotherThanCutFromPositionGroup = false;
            else AreEnabledAnotherThanCutFromPositionGroup = true;
        }

        private bool AreEnabledAnotherThanCutFromPositionGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        #endregion // Cut from position

        #region Cut text

        private void CutText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanCutText();

        private void ActivateDesactivateOtherThanCutText()
        {
            if (!string.IsNullOrWhiteSpace(cutText.Text))
                AreEnabledAnotherThanCutTextGroup = false;
            else AreEnabledAnotherThanCutTextGroup = true;
        }

        private bool AreEnabledAnotherThanCutTextGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        #endregion // Cut text

        #region Replace text

        private void ReplaceTextOldText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanReplaceText();

        private void ReplaceTextNewText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanReplaceText();

        private void ActivateDesactivateOtherThanReplaceText()
        {
            if (!string.IsNullOrWhiteSpace(replaceTextOldText.Text) ||
                !string.IsNullOrWhiteSpace(replaceTextNewText.Text))
                AreEnabledAnotherThanReplaceTextGroup = false;
            else AreEnabledAnotherThanReplaceTextGroup = true;
        }

        private bool AreEnabledAnotherThanReplaceTextGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        #endregion // Replace text

        #region Insert numbering

        private void InsertNumberingPosition_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
            => ActivateDesactivateOtherThanInsertNumbering();

        private void ActivateDesactivateOtherThanInsertNumbering()
        {
            if (!string.IsNullOrWhiteSpace(insertNumberingPosition.Text))
                AreEnabledAnotherThanInsertNumberingGroup = false;
            else AreEnabledAnotherThanInsertNumberingGroup = true;
        }

        private bool AreEnabledAnotherThanInsertNumberingGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupChangeFromID3.IsEnabled = value;
            }
        }

        #endregion // Insert numbering

        #region Change From ID3 pattern

        private void ChangeFromID3Pattern_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
            => ActivateDesactivateOtherChangeFromID3Pattern();

        private void ActivateDesactivateOtherChangeFromID3Pattern()
        {
            if (!string.IsNullOrWhiteSpace(ChangeFromID3Pattern.Text))
                AreEnabledAnotherThanChangeFromID3PatternGroup = false;
            else AreEnabledAnotherThanChangeFromID3PatternGroup = true;
        }

        private bool AreEnabledAnotherThanChangeFromID3PatternGroup
        {
            set
            {
                ribbonGroupCut.IsEnabled = value;
                ribbonGroupReplaceToSpace.IsEnabled = value;
                ribbonGroupReplaceFromSpace.IsEnabled = value;
                ribbonGroupInsertTextFromPosition.IsEnabled = value;
                ribbonGroupCutTextFromPosition.IsEnabled = value;
                ribbonGroupCutText.IsEnabled = value;
                ribbonGroupReplaceText.IsEnabled = value;
                ribbonGroupInsertNumbering.IsEnabled = value;
            }
        }

        #endregion // Change from ID3 pattern

        #region Fields

        private RibbonGroup ribbonGroupCut;
        private RibbonGroup ribbonGroupReplaceToSpace;
        private RibbonGroup ribbonGroupReplaceFromSpace;
        private RibbonGroup ribbonGroupChange;
        private RibbonGroup ribbonGroupInsertTextFromPosition;
        private RibbonGroup ribbonGroupCutTextFromPosition;
        private RibbonGroup ribbonGroupCutText;
        private RibbonGroup ribbonGroupReplaceText;
        private RibbonGroup ribbonGroupInsertNumbering;
        private RibbonGroup ribbonGroupChangeFromID3;

        private RibbonCheckBox cutSpace;
        private RibbonCheckBox cutDot;
        private RibbonCheckBox cutUnderscore;
        private RibbonCheckBox cutDash;
        private RibbonCheckBox dotToSpace;
        private RibbonCheckBox underscoreToSpace;
        private RibbonCheckBox dashToSpace;
        private RibbonCheckBox spaceToDot;
        private RibbonCheckBox spaceToUnderscore;
        private RibbonCheckBox spaceToDash;
        private RibbonRadioButton firstCapitalLetter;
        private RibbonRadioButton allFirstCapitalLetters;
        private RibbonRadioButton upperCase;
        private RibbonRadioButton lowerCase;
        private RibbonTextBox insertFromPositionPosition;
        private RibbonTextBox insertFormPositionText;
        private RibbonTextBox cutFromPositionPosition;
        private RibbonTextBox cutFromPositionCount;
        private RibbonTextBox cutText;
        private RibbonTextBox replaceTextOldText;
        private RibbonTextBox replaceTextNewText;
        private RibbonTextBox insertNumberingPosition;
        private RibbonComboBox ChangeFromID3Pattern;

        #endregion // Fields
    }
}
