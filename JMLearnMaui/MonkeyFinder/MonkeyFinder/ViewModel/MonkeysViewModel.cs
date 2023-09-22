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

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }

    public ObservableCollection<Monkey> Monkeys { get; } = new();

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
        }
    }
}
