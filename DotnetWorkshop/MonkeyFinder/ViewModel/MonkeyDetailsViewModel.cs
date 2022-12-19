namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
	private readonly IMap map;

	public MonkeyDetailsViewModel(IMap map)
	{
		this.map = map;
	}

	[ObservableProperty]
	Monkey monkey;


	[RelayCommand]
	async Task OpenMapAsync()
	{
		try
		{
			await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, 
				new MapLaunchOptions
				{
					Name = Monkey.Name,
					NavigationMode = NavigationMode.None
				});
		}
		catch (Exception ex)
		{

            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Map Error!",
                $"Unable to display monkey map: {ex.Message}",
                "OK");
        }
	}
}
