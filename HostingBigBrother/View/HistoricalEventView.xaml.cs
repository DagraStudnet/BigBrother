using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BigBrotherViewer.Model;
using BigBrotherViewer.ViewModel;

namespace BigBrotherViewer.View
{
    /// <summary>
    ///     Interaction logic for HistoricalEventView.xaml
    /// </summary>
    public partial class HistoricalEventView : Window
    {
        private HistoricalEventDataViewModel historicalEventDataViewModel;

        public HistoricalEventView(List<Attention> attentions)
        {
            InitializeComponent();
            historicalEventDataViewModel = new HistoricalEventDataViewModel(attentions);
            DataContext = historicalEventDataViewModel;
        }

        private void FilterClear_OnClick(object sender, RoutedEventArgs e)
        {
            EventComboBoxFilter.SelectedValue = 0;
            OserverComboBoxFilter.SelectedValue = 0;
            UserComboBoxFilter.SelectedValue = 0;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
        }

        private void DatePickerStart_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            historicalEventDataViewModel.SelectedStartEvent = picker.SelectedDate;
        }

        private void DatePickerEnd_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            historicalEventDataViewModel.SelectedEndEvent = picker.SelectedDate;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            historicalEventDataViewModel.FilterData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            historicalEventDataViewModel.DeleteAllDb();
            FilterClear_OnClick(sender,e);
            Filter_Click(sender,e);
        }
    }
}