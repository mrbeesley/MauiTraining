namespace BMICalculator;

public partial class BMICalculatorPage : ContentPage
{
	public BMICalculatorPage()
	{
		InitializeComponent();
	}

    private void TapMale_Tapped(object sender, TappedEventArgs e)
    {
        FrameMale.BorderColor = Color.FromArgb("#0a0e19");
        FrameFemale.BorderColor = Color.FromArgb("#fdfdfd");
    }

    private void TapFemale_Tapped(object sender, TappedEventArgs e)
    {
        FrameFemale.BorderColor = Color.FromArgb("#0a0e19");
        FrameMale.BorderColor = Color.FromArgb("#fdfdfd");
    }

    private void BtnCalculateBMI_Clicked(object sender, EventArgs e)
    {
        var height = Convert.ToDouble(LblHeight.Text);
        var weight = Convert.ToDouble(LblWeight.Text);

        var bmi = (weight / height / height) * 10000;
        DisplayAlert("Your BMI is:", bmi.ToString(), "OK");
    }
}