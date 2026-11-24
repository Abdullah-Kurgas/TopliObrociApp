using System;
using System.Collections.Generic;

namespace TopliObrociApp.Models;

public class RadniRaspored
{
    public bool[] WorkingDays { get; set; } = [true, true, true, true, true, false, false];
    public List<DateTime> Absences { get; init; } = [];
}