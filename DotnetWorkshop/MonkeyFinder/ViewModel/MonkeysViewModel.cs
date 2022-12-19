using MonkeyFinder.Services;
namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
	private readonly MonkeyService monkeyService;
	private readonly IConnectivity connectivity;
	private readonly IGeolocation geolocation;

	public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
	{
		Title = "Monkey Finder";
		this.monkeyService = monkeyService;
		this.connectivity = connectivity;
		this.geolocation = geolocation;
	}

	[ObservableProperty]
	bool isRefreshing;

	public ObservableCollection<Monkey> Monkeys { get; } = new();

	[RelayCommand]
	async Task GetClosestMonkey()
	{
		if (IsBusy || Monkeys.Count == 0)
			return;

		try
		{
			var location = await geolocation.GetLastKnownLocationAsync();
			if (location is null)
			{
				location = await geolocation.GetLocationAsync(new GeolocationRequest
				{
					DesiredAccuracy = GeolocationAccuracy.Medium,
					Timeout = TimeSpan.FromSeconds(30)
				});
			}

			if (location is null)
			{
                await Shell.Current.DisplayAlert("Location Error!",
					$"Unable to get location.",
					"OK");
                return;
			}

			var first = Monkeys.OrderBy(m => 
					location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Miles)
				).FirstOrDefault();

			if (first is null)
			{
                await Shell.Current.DisplayAlert("Closest Monkey Error!",
					$"Unable to get closest monkey",
					"OK");
                return;
			}

            await Shell.Current.DisplayAlert("Closest Monkey!",
                $"{first.Name} in {first.Location}",
                "OK");

        }
		catch (Exception ex)
		{
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", 
				$"Unable to get closest monkey: {ex.Message}", 
				"OK");
        }
	}

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

		if (IsBusy) 
			return;

		if (connectivity.NetworkAccess != NetworkAccess.Internet)
		{
            await Shell.Current.DisplayAlert("Network Issue!", 
				$"Unable to get monkeys, current network status is {connectivity.NetworkAccess}", 
				"OK");
            return;
		}	

		try
		{

			IsBusy= true;
			var monkeys = await monkeyService.GetMonkeys();

			if (Monkeys.Count != 0)
				Monkeys.Clear();

			foreach (var monkey in monkeys)
			{
				Monkeys.Add(monkey);
			}
		}
		catch (Exception ex) 
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!", 
				$"Unable to get monkeys: {ex.Message}", 
				"OK");
		}
		finally
		{
			IsBusy= false;
			IsRefreshing= false;
		}
	}
}
