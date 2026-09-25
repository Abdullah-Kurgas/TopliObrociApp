using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Enums;
using TopliObrociApp.Models;
using TopliObrociApp.Services;

namespace TopliObrociApp.Windows;

public partial class AddEditUser : Window
{
    private readonly GarsonService _garsonService = new();
    private readonly bool _isEdit;

    private readonly List<CReprezent> _reprezenti = [];
    private readonly User? _user;
    private readonly UserService _userService = new();

    public AddEditUser()
    {
        InitializeComponent();

        _isEdit = false;
        DataContext = this;
        Title = WindowTitle;
        Naslov.Text = WindowTitle;

        RoleComboBox.SelectedIndex = 1;
        IsActiveCheckBox.IsChecked = true;

        ReprezentAutoComplete.PropertyChanged += ReprezentAutoComplete_PropertyChanged;
    }

    public AddEditUser(User user)
    {
        InitializeComponent();

        _user = user;
        _isEdit = true;
        DataContext = this;
        Title = WindowTitle;
        Naslov.Text = WindowTitle;

        LoadUser();
    }

    private string WindowTitle => _isEdit ? "Uredi korisnika" : "Novi korisnik";

    private async void ReprezentAutoComplete_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var search = ReprezentAutoComplete.Text?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(search) || search.Length < 2)
        {
            ReprezentAutoComplete.ItemsSource = null;
            return;
        }

        var results = await _garsonService.GetReprezenteByNameAsync(search);
        ReprezentAutoComplete.ItemsSource = results;
    }

    private void ReprezentAutoComplete_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != AutoCompleteBox.SelectedItemProperty) return;
        if (e.NewValue is not CReprezent reprezent) return;

        var parts = reprezent.Ime.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        var lastName = parts[0];
        var firstName = string.Join(" ", parts.Skip(1));

        LastNameTextBox.Text = lastName;
        FirstNameTextBox.Text = firstName;
        GarsonIdTextBox.Text = reprezent.IdReprezenta.ToString();
        UsernameTextBox.Text = GenerateUsername(firstName, lastName);
    }

    private void LoadUser()
    {
        if (_user == null) return;

        FirstNameTextBox.Text = _user.FirstName;
        LastNameTextBox.Text = _user.LastName;
        UsernameTextBox.Text = _user.Username;
        GarsonIdTextBox.Text = _user.GarsonId.ToString();

        RoleComboBox.SelectedIndex = (int)(_user.Role + 1);
        IsActiveCheckBox.IsChecked = _user.IsActive;
    }

    private static string GenerateUsername(string firstName, string lastName)
    {
        return $"{Normalize(firstName.ToLower())}.{Normalize(lastName.ToLower())}".ToLowerInvariant();
    }

    private static string Normalize(string value)
    {
        return value
            .Replace("č", "c")
            .Replace("ć", "c")
            .Replace("ž", "z")
            .Replace("š", "s")
            .Replace("đ", "d")
            .Trim();
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var firstName = FirstNameTextBox.Text?.Trim() ?? "";
            var lastName = LastNameTextBox.Text?.Trim() ?? "";
            var username = UsernameTextBox.Text?.Trim() ?? "";
            var password = PasswordTextBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(firstName))
                // await ShowErrorAsync("Ime je obavezno.");
                return;

            if (string.IsNullOrWhiteSpace(lastName))
                // await ShowErrorAsync("Prezime je obavezno.");
                return;

            if (string.IsNullOrWhiteSpace(username))
                // await ShowErrorAsync("Username je obavezan.");
                return;

            if (string.IsNullOrWhiteSpace(password))
                // await ShowErrorAsync("Lozinka je obavezna.");
                return;

            if (!int.TryParse(GarsonIdTextBox.Text, out var garsonId))
                // await ShowErrorAsync("Garson ID nije ispravan.");
                return;

            var user = new User
            {
                Id = 0,
                GarsonId = garsonId,
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = (Role)(RoleComboBox.SelectedIndex + 1),
                IsActive = true
            };

            await _userService.AddUserAsync(user);

            Close(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            // await ShowErrorAsync(
            //     $"Greška prilikom spremanja korisnika:\n{ex.Message}");
        }
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private static async Task MessageBox(string title, string message)
    {
        // Za sada placeholder.
        // Možemo napraviti lijepi Avalonia dialog kasnije.
        await Task.CompletedTask;
    }
}