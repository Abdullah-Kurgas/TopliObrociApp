using System;
using Avalonia.Media;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public string Label { get; set; } = "";
    public bool IsEnabled { get; set; }
    public IBrush Background { get; set; } = Brushes.Transparent;
    public IBrush Foreground { get; set; } = Brushes.White;
}