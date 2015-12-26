using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace FOSDEM
{
    public class RichTextBlockHelper : DependencyObject
    {
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string),
            typeof(RichTextBlockHelper),
            new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var control = sender as RichTextBlock;
            if (control != null)
            {
                control.Blocks.Clear();

                var paragraph = new Paragraph();
                string[] values = e.NewValue.ToString().Split(new string[] { "<p>", @"</p>", "<ul>", @"</ul>", "<li>", @"</li>" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var value in values)
                {
                    paragraph.Inlines.Add(new Run { Text = value });
                }
                control.Blocks.Add(paragraph);
            }
        }
    }
}
