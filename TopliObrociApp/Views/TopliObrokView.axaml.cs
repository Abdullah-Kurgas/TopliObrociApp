using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views;

public partial class TopliObrokView : UserControl
{
    private readonly AuthSession _authSession;
    private readonly GarsonService _garsonService = new();
    private DateTime _currentMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public TopliObrokView(AuthSession authSession)
    {
        InitializeComponent();
        ToggleLoading(true);

        _authSession = authSession;
        Loaded += async (_, __) => { await UpdateScreen(); };
    }

    private async Task UpdateScreen()
    {
        ToggleLoading(true);

        try
        {
            MonthNameText.Text = _currentMonth.ToString("MMMM yyyy");

            var workingDays = CalculateWorkingDays(_currentMonth);
            var absentDays = (int)(AbsentDaysInput.Value ?? 0);
            var billableDays = Math.Max(0, workingDays - absentDays);

            var racuni = await _garsonService.GetRacuniAsync(_currentMonth, _authSession.CurrentUser!.GarsonId);
            var total = await _garsonService.GetUkupnoAsync(_currentMonth, _authSession.CurrentUser!.GarsonId);
            const decimal iznosPoDanu = 10m;
            var ukupnoNaRaspolaganju = billableDays * iznosPoDanu;
            var potroseno = total;
            var preostalo = ukupnoNaRaspolaganju - potroseno;
            var procenat = ukupnoNaRaspolaganju > 0 ? potroseno / ukupnoNaRaspolaganju * 100 : 0;
            var progress = Math.Clamp(procenat, 0, 100);

            WorkingDaysText.Text = workingDays.ToString();
            VacationDaysText.Text = absentDays.ToString();
            CalculatedDaysText.Text = billableDays.ToString();
            AvailableAmountText.Text = $"{ukupnoNaRaspolaganju:N2} KM";
            SpentAmountText.Text = $"{potroseno:N2} KM";
            RemainingAmountText.Text = $"{preostalo:N2} KM";
            UsageSummaryText.Text = $"Potrošeno {potroseno:N2} KM od {ukupnoNaRaspolaganju:N2} KM";
            UsageProgressBar.Value = (double)progress;
            ReceiptCountHeaderText.Text = racuni.Count == 1 ? "1 račun" : $"{racuni.Count} računa";

            ReceiptsGrid.ItemsSource = racuni;
        }
        finally
        {
            ToggleLoading(false);
        }
    }


    private static int CalculateWorkingDays(DateTime month)
    {
        var days = 0;
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

        for (var i = 1; i <= daysInMonth; i++)
        {
            var date = new DateTime(month.Year, month.Month, i);

            if (date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday)
                days++;
        }

        return days;
    }


    private void Input_ValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        // UpdateScreen();
    }

    private void PreviousMonth_Click(object? sender, RoutedEventArgs e)
    {
        ChangeMonth(-1);
    }


    private void NextMonth_Click(object? sender, RoutedEventArgs e)
    {
        ChangeMonth(1);
    }

    private void ChangeMonth(int months)
    {
        _currentMonth = _currentMonth.AddMonths(months);
        _ = UpdateScreen();
    }

    private void ToggleLoading(bool loading)
    {
        ContentGrid.Effect = loading ? new BlurEffect { Radius = 10 } : null;
        LoadingOverlay.IsVisible = loading;
    }
}