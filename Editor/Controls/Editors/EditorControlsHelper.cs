using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    internal static class EditorControlsHelper
    {
        public static Button CreateMoveUpButton(int rowIndex, int columnIndex, bool isEnabled, RoutedEventHandler clickHandler)
        {
            Button moveUpButton = new Button();
            moveUpButton.Content = new Image() { Source = new BitmapImage(new Uri(@"/Resources/Images/up.png", UriKind.Relative)) };
            moveUpButton.ToolTip = "Move Up";
            moveUpButton.Margin = new Thickness(5, 5, 1, 0);
            moveUpButton.Tag = rowIndex;
            moveUpButton.Focusable = false;
            moveUpButton.IsEnabled = isEnabled;
            moveUpButton.SetValue(Grid.RowProperty, rowIndex);
            moveUpButton.SetValue(Grid.ColumnProperty, columnIndex);
            moveUpButton.Click += new RoutedEventHandler(clickHandler);

            return moveUpButton;
        }

        public static Button CreateMoveDownButton(int rowIndex, int columnIndex, bool isEnabled, RoutedEventHandler clickHandler)
        {
            Button moveDownButton = new Button();
            moveDownButton.Content = new Image() { Source = new BitmapImage(new Uri(@"/Resources/Images/down.png", UriKind.Relative)) };
            moveDownButton.ToolTip = "Move Down";
            moveDownButton.Margin = new Thickness(1, 5, 5, 0);
            moveDownButton.Tag = rowIndex;
            moveDownButton.Focusable = false;
            moveDownButton.IsEnabled = isEnabled;
            moveDownButton.SetValue(Grid.RowProperty, rowIndex);
            moveDownButton.SetValue(Grid.ColumnProperty, columnIndex);
            moveDownButton.Click += new RoutedEventHandler(clickHandler);

            return moveDownButton;
        }

        public static Button CreateRemoveChoiceButton(int rowIndex, int columnIndex, RoutedEventHandler clickHandler)
        {
            Button removeButton = new Button();
            removeButton.Content = new Image() { Source = new BitmapImage(new Uri(@"/Resources/Images/close.png", UriKind.Relative)) };
            removeButton.ToolTip = "Remove choice";
            removeButton.Margin = new Thickness(5, 5, 5, 0);
            removeButton.Tag = rowIndex;
            removeButton.Focusable = false;
            removeButton.SetValue(Grid.RowProperty, rowIndex);
            removeButton.SetValue(Grid.ColumnProperty, columnIndex);
            removeButton.Click += new RoutedEventHandler(clickHandler);

            return removeButton;
        }

        public static TextBox CreateChoiceTextBox(int rowIndex, int columnIndex, string text, TextChangedEventHandler textChangedHandler)
        {
            TextBox choiceTextBox = new TextBox();
            choiceTextBox.Text = text;
            choiceTextBox.Margin = new Thickness(5, 5, 5, 0);
            choiceTextBox.Tag = rowIndex;
            choiceTextBox.SetValue(Grid.RowProperty, rowIndex);
            choiceTextBox.SetValue(Grid.ColumnProperty, columnIndex);
            choiceTextBox.TextChanged += new TextChangedEventHandler(textChangedHandler);

            return choiceTextBox;
        }
    }
}
