using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TopliObrociApp.Models;
using TopliObrociApp.Services;

namespace TopliObrociApp.Views.Controls;

public partial class RacunDetailsPanel : UserControl
{
    private readonly GarsonService _garsonService;

    public RacunDetailsPanel()
    {
        InitializeComponent();

        _garsonService = new GarsonService();
    }

    public event EventHandler? CloseRequested;

    public async Task ShowReceiptAsync(RReprezentiKartice receipt)
    {
        ReceiptDateText.Text = receipt.Datum.ToString("dd.MM.yyyy");
        ReceiptStornoBadge.IsVisible = false;

        if (!receipt.IdDokumentaVeze.HasValue) return;
        ReceiptItemsGrid.ItemsSource = null;

        var stavke = await _garsonService.GetStavkeRacunaAsync(
            receipt.IdDokumentaVeze.Value);
        var cijeliRacunStorniran = stavke.Count > 0;
        var aktivneStavke = stavke.ToList();

        ReceiptItemsGrid.ItemsSource = stavke;
        ReceiptStornoBadge.IsVisible = cijeliRacunStorniran;
        ItemsCountText.Text = aktivneStavke.Count.ToString();

        TotalAmountText.Text = $"{aktivneStavke.Sum(x => x.IznosSaPopustom):N2} KM";
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}