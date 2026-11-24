using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class RadniRasporedService
{
    private readonly string _filePath;
    private RadniRaspored _data = new();

    private RadniRasporedService()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TopliObrociApp"
        );

        Directory.CreateDirectory(appDataPath);
        _filePath = Path.Combine(appDataPath, "work_schedule.json");
    }

    public static RadniRasporedService Instance { get; } = new();

    // ---------- Schedule ----------

    public bool[] WorkingDays => _data.WorkingDays;

    public int WorkingDaysPerWeek => _data.WorkingDays.Count(x => x);


    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath)) return;

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            _data = JsonSerializer.Deserialize<RadniRaspored>(json) ?? new RadniRaspored();

            if (_data.WorkingDays is not { Length: 7 })
                _data.WorkingDays = new RadniRaspored().WorkingDays;
        }
        catch (JsonException)
        {
            // Oštećen fajl: vrati default raspored
            _data = new RadniRaspored();
        }
    }

    public async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(
            _data, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(_filePath, json);
    }

    public void SetWorkingDay(int index, bool value)
    {
        _data.WorkingDays[index] = value;
    }

    public bool IsWorkingDay(DateTime date)
    {
        return _data.WorkingDays[((int)date.DayOfWeek + 6) % 7];
    }

    // ---------- Absences ----------

    public bool IsAbsent(DateTime date)
    {
        return _data.Absences.Contains(date.Date);
    }

    public void ToggleAbsence(DateTime date)
    {
        date = date.Date;

        if (!IsWorkingDay(date)) return;

        if (!_data.Absences.Remove(date))
            _data.Absences.Add(date);
    }

    // ---------- Counts ----------

    /// Dani po rasporedu (bez izostanaka)
    public int GetWorkingDays(int year, int month)
    {
        return MonthDays(year, month).Count(IsWorkingDay);
    }

    /// Izostanci na radnim danima
    public int GetAbsenceCount(int year, int month)
    {
        return _data.Absences.Count(d =>
            d.Year == year && d.Month == month && IsWorkingDay(d));
    }

    /// Dani koje si stvarno radio
    public int GetBillableDays(int year, int month)
    {
        return GetWorkingDays(year, month) - GetAbsenceCount(year, month);
    }

    /// Svi dani u mjesecu koje nisi radio (neradni po rasporedu + izostanci)
    public int GetNonWorkingDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month) - GetBillableDays(year, month);
    }

    private static IEnumerable<DateTime> MonthDays(int year, int month)
    {
        return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
            .Select(d => new DateTime(year, month, d));
    }
}