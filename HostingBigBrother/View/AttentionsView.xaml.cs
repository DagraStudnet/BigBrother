using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HostingBigBrother.Model;

namespace HostingBigBrother.View
{
    /// <summary>
    ///     Interaction logic for AttentionsView.xaml
    /// </summary>
    public partial class AttentionsView : Window
    {
        private IEnumerable<Attention> _attentions;

        public AttentionsView(IEnumerable<Attention> attentions)
        {
            _attentions = attentions;
            InitializeComponent();
            SetAttentionToTextBox();
        }

        private void SetAttentionToTextBox()
        {
            if (!_attentions.Any()) return;
            var sb = new StringBuilder();
            foreach (var attention in _attentions)
            {
                sb.Append(attention.Name + ",");
            }
            var textAttentions = sb.ToString();
            textAttentions = textAttentions.Remove(textAttentions.Length - 1);
            TextBoxAttentions.Text = textAttentions;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void StornoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}