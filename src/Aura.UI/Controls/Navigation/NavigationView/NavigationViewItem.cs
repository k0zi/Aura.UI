﻿using Aura.UI.Extensions;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using System;

namespace Aura.UI.Controls.Navigation
{
    public class NavigationViewItem : NavigationViewItemBase
    {
        //Type IStyleable.StyleKey => typeof(NavigationViewItemBase);
        
        protected override Type StyleKeyOverride => typeof(NavigationViewItemBase);

        static NavigationViewItem()
        {
            SelectableMixin.Attach<NavigationViewItem>(IsSelectedProperty);
            FocusableProperty.OverrideDefaultValue<NavigationViewItem>(true);
            ClickModeProperty.OverrideDefaultValue<NavigationViewItem>(ClickMode.Release);
        }

        protected override void OnClosed(object sender, RoutedEventArgs e)
        {
            base.OnClosed(sender, e);

            if (SelectOnClose)
            {
                var parent = this.GetParentTOfLogical<NavigationView>();
                if(parent != null) parent.SelectSingleItem(this);
            }
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            NavigationViewDistance = Extensions.LogicalExtensions.CalculateDistanceFromLogicalParent<NavigationView>(this) - 1;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (ClickMode == ClickMode.Press && this.PointerEffectivelyOver(e))
            {
                Select();
            }
            e.Handled = true;
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            if (ClickMode == ClickMode.Release && IsPointerOver && this.PointerEffectivelyOver(e))
            {
                Select();
            }
            e.Handled = true;
        }

        private void Select()
        {
            if (!IsSelected)
            {
                var parent = this.GetParentTOfLogical<NavigationView>();
                if (parent != null) parent.SelectSingleItem(this);
                //this.GetParentTOfLogical<NavigationView>().SelectSingleItem(this);
            }
        }
    }
}