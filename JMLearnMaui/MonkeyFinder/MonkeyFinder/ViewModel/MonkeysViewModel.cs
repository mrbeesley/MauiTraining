using MonkeyFinder.Services;
using MonkeyFinder.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{

    private readonly MonkeyService monkeyService;
    private readonly IConnectivity connectivity;
    private readonly IGeolocation geolocation;

    public MonkeysViewModel(MonkeyService monkeyService, 
        IConnectivity connectivity, 
        IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    public ObservableCollection<Monkey> Monkeys { get; } = new();

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, 
            new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }


    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy) return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No Internet!", "Please check your internet connection.", "OK");
                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();
            if (Monkeys.Count != 0)
                monkeys.Clear();

            foreach(var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0) return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync() 
                ?? await geolocation.GetLocationAsync(new GeolocationRequest 
                { 
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

            if (location is null)
            {
                await Shell.Current.DisplayAlert("Unable to get location", "Please try again later.", "OK");
                return;
            }

            var closest = Monkeys
                .OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Miles))
                .FirstOrDefault();

            if (closest is null) return;

            await Shell.Current.DisplayAlert("Closest Monkey", 
                $"{closest.Name} in {closest.Location}", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey: {ex.Message}", "OK");
        }
    }
}
