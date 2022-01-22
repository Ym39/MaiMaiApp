using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaiMaiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("id") == false)
                Application.Current.Properties["id"] = "";

            if (Application.Current.Properties.ContainsKey("password") == false)
                Application.Current.Properties["password"] = "";

            IDEntry.Text = (string)Application.Current.Properties["id"];
            PasswordEntry.Text = (string)Application.Current.Properties["password"];

            LoginButton.Clicked += OnLoginButton;
        }

        private async void OnLoginButton(object sender, EventArgs args)
        {
            Application.Current.Properties["id"] = IDEntry.Text;
            Application.Current.Properties["password"] = PasswordEntry.Text;
            await Navigation.PushAsync(new LoadingPage());
        }
    }
}