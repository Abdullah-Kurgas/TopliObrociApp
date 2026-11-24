using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class RadniRasporedView : UserControl
{
    private readonly RadniRasporedService _service = RadniRasporedService.Instance;

    private DateTime _currentMonth =
        new(DateTime.Today.Year, DateTime.Today.Month, 1);

    private bool _isInitializing = true;
    private bool _loaded;

    public RadniRasporedView()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (_loaded) return;
            _loaded = true;

            await _service.LoadAsync();
            ApplyScheduleToCheckBoxes();

            _isInitializing = false;
            RefreshView();
        };
    }

    private void ApplyScheduleToCheckBoxes()
    {
        _isInitializing = true;

        foreach (var cb in DaysPanel.Children.OfType<CheckBox>())
            if (cb.Tag is string tag && int.TryParse(tag, out var i))
                cb.IsChecked = _service.WorkingDays[i];

        _isInitializing = false;
    }

    private void PreviousMonth_Click(object? sender, RoutedEventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        RefreshView();
    }

    private void NextMonth_Click(object? sender, RoutedEventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        RefreshView();
    }

    private void GenerateCalendar()
    {
        var days = new List<CalendarDay>();
        var daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
        var offset = ((int)_currentMonth.DayOfWeek + 6) % 7;

        for (var i = 0; i < offset; i++)
            days.Add(Empty());

        for (var d = 1; d <= daysInMonth; d++)
        {
            var date = new DateTime(_currentMonth.Year, _currentMonth.Month, d);
            var working = _service.IsWorkingDay(date);
            var absent = working && _service.IsAbsent(date);

            days.Add(new CalendarDay
            {
                Date = date,
                Label = d.ToString(),
                IsEnabled = working,
                Background = absent ? Brush("#783D32")
                    : working ? Brush("#214B36")
                    : Brush("#292929"),
                Foreground = absent ? Brush("#FFD2C5")
                    : working ? Brushes.White
                    : Brush("#777777")
            });
        }

        while (days.Count < 42)
            days.Add(Empty());

        CalendarItems.ItemsSource = days;
    }

    private static CalendarDay Empty()
    {
        return new CalendarDay
        {
            Label = "",
            IsEnabled = false,
            Background = Brushes.Transparent,
            Foreground = Brushes.Transparent
        };
    }

    private void CalendarDay_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: CalendarDay day } || !day.IsEnabled)
            return;

        _service.ToggleAbsence(day.Date);
        RefreshView();
    }

    private void ScheduleChanged(object? sender, RoutedEventArgs e)
    {
        if (_isInitializing) return;

        if (sender is not CheckBox cb ||
            cb.Tag is not string tag ||
            !int.TryParse(tag, out var index) ||
            index is < 0 or > 6)
            return;

        _service.SetWorkingDay(index, cb.IsChecked == true);
        RefreshView();
    }

    private void RefreshView()
    {
        if (_isInitializing) return;

        var y = _currentMonth.Year;
        var m = _currentMonth.Month;

        var working = _service.GetWorkingDays(y, m);
        var absences = _service.GetAbsenceCount(y, m);
        var billable = _service.GetBillableDays(y, m);

        MonthTitle.Text = _currentMonth.ToString("MMMM yyyy");

        WorkingDaysText.Text = working.ToString();
        AbsencesCountText.Text = absences.ToString();
        BillableDaysText.Text = billable.ToString();

        GenerateCalendar();
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            await _service.SaveAsync();
            StatusText.Foreground = Brush("#4ADE80");
            StatusText.Text = "Raspored je sačuvan.";
        }
        catch (Exception ex)
        {
            StatusText.Foreground = Brush("#F87171");
            StatusText.Text = $"Greška pri čuvanju: {ex.Message}";
        }
    }

    private static SolidColorBrush Brush(string color)
    {
        return new SolidColorBrush(Color.Parse(color));
    }
}