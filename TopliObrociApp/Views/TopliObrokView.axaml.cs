using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TopliObrociApp.Views;

public partial class TopliObrokView : UserControl
{
    private readonly List<TopliObrokReceipt> _receipts = new()
    {
        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 2),
            ReceiptNumber = "R-10284",
            ObjectName = "Restoran Central",
            Amount = 12.00m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 3),
            ReceiptNumber = "R-10291",
            ObjectName = "Bistro Most",
            Amount = 9.50m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 4),
            ReceiptNumber = "R-10305",
            ObjectName = "Restoran Central",
            Amount = 11.00m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 7),
            ReceiptNumber = "R-10344",
            ObjectName = "Fast Food 24",
            Amount = 8.50m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 9),
            ReceiptNumber = "R-10381",
            ObjectName = "Bistro Most",
            Amount = 10.00m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 9, 10),
            ReceiptNumber = "R-10402",
            ObjectName = "Restoran Central",
            Amount = 12.00m
        },

        // Prethodni mjesec
        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 8, 3),
            ReceiptNumber = "R-9811",
            ObjectName = "Restoran Central",
            Amount = 11.00m
        },

        new TopliObrokReceipt
        {
            Date = new DateTime(2026, 8, 5),
            ReceiptNumber = "R-9842",
            ObjectName = "Bistro Most",
            Amount = 10.00m
        }
    };

    private DateTime _currentMonth = new(2026, 9, 1);


    public TopliObrokView()
    {
        InitializeComponent();

        UpdateScreen();
    }


    private void UpdateScreen()
    {
        MonthNameText.Text =
            _currentMonth.ToString("MMMM yyyy");

        var workingDays =
            CalculateWorkingDays(_currentMonth);

        var vacationDays =
            (int)(VacationInput.Value ?? 0);

        var workDaysPerWeek =
            (int)(WorkDaysInput.Value ?? 5);

        /*
         * Ako korisnik radi 5 dana sedmično,
         * koristimo standardne pon-pet radne dane.
         *
         * Ako radi manje/više dana,
         * kasnije možemo napraviti precizniji
         * raspored po danima u sedmici.
         */

        if (workDaysPerWeek != 5)
            workingDays =
                CalculateWorkingDaysForWeek(
                    _currentMonth,
                    workDaysPerWeek);


        var billableDays =
            Math.Max(
                0,
                workingDays - vacationDays);


        // Računi za trenutno izabrani mjesec
        var monthReceipts =
            _receipts
                .Where(x =>
                    x.Date.Year == _currentMonth.Year &&
                    x.Date.Month == _currentMonth.Month)
                .ToList();


        var total =
            monthReceipts.Sum(x => x.Amount);


        // UI
        WorkingDaysText.Text =
            workingDays.ToString();

        VacationDaysText.Text =
            $"{vacationDays} dana";

        CalculatedDaysText.Text =
            billableDays.ToString();

        BillableDaysText.Text =
            $"{billableDays} obračunatih dana";

        TotalAmountText.Text =
            $"{total:N2} KM";

        ReceiptCountText.Text =
            monthReceipts.Count == 1
                ? "1 račun"
                : $"{monthReceipts.Count} računa";


        ReceiptsGrid.ItemsSource =
            monthReceipts;
    }


    private static int CalculateWorkingDays(DateTime month)
    {
        var days = 0;

        var daysInMonth =
            DateTime.DaysInMonth(
                month.Year,
                month.Month);

        for (var i = 1; i <= daysInMonth; i++)
        {
            var date =
                new DateTime(
                    month.Year,
                    month.Month,
                    i);

            if (date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday)
                days++;
        }

        return days;
    }


    private static int CalculateWorkingDaysForWeek(
        DateTime month,
        int daysPerWeek)
    {
        /*
         * Za sada jednostavna procjena.
         *
         * Kasnije možemo napraviti pravi
         * raspored radnih dana, npr:
         *
         * PON ✓
         * UTO ✓
         * SRI ✓
         * ČET ✓
         * PET ✓
         * SUB ✗
         * NED ✗
         */

        var standardWorkingDays =
            CalculateWorkingDays(month);

        return (int)Math.Round(
            standardWorkingDays *
            daysPerWeek / 5.0);
    }


    private void Input_ValueChanged(
        object? sender,
        NumericUpDownValueChangedEventArgs e)
    {
        UpdateScreen();
    }


    private void PreviousMonth_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth =
            _currentMonth.AddMonths(-1);

        UpdateScreen();
    }


    private void NextMonth_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth =
            _currentMonth.AddMonths(1);

        UpdateScreen();
    }


    private void July_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth = new DateTime(2026, 7, 1);

        UpdateScreen();
    }


    private void August_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth = new DateTime(2026, 8, 1);

        UpdateScreen();
    }


    private void September_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth = new DateTime(2026, 9, 1);

        UpdateScreen();
    }


    private void October_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _currentMonth = new DateTime(2026, 10, 1);

        UpdateScreen();
    }
}

public class TopliObrokReceipt
{
    public DateTime Date { get; set; }

    public string ReceiptNumber { get; set; }
        = string.Empty;

    public string ObjectName { get; set; }
        = string.Empty;

    public decimal Amount { get; set; }
}