﻿using MyVet.Common.Models;
using MyVet.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _logicCommand;
        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
        }

        //public DelegateCommand LogicCommand
        //{
        //    get
        //    {
        //        if(_logicCommand == null)
        //        {
        //            _logicCommand = new DelegateCommand(Login);
        //        }
        //        return _logicCommand;
        //    }
        //}

        public DelegateCommand LogicCommand => _logicCommand ?? (_logicCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an email", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a passwoed", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

         

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email or password incorrect", "Accept");
                Password = string.Empty;
                IsEnabled = true;
                return;
            }

            var token = (TokenResponse)response.Result;

            var response2 = await _apiService.GetOwnerByEmailAsync(url, "api", "/Owners/GetOwnerByEmail", "bearer",token.Token, Email);

            if (!response2.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "This user have a big problem, call support.", "Accept");
                return;
            }

            var owner = (OwnerResponse)response2.Result;
            var parameters = new NavigationParameters
            {
                { "owner", owner }
            };

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("PetsPage",parameters);
            Password = string.Empty;
        }
    }
}
