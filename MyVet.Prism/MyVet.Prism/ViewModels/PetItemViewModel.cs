using MyVet.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVet.Prism.ViewModels
{
    public class PetItemViewModel : PetResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCommand;

        public PetItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectPetCommand => _selectCommand ?? (_selectCommand = new DelegateCommand(SelectPet));

        private async void SelectPet()
        {
            var parameters = new NavigationParameters
            {
                { "pet", this}
            };

            await _navigationService.NavigateAsync("PetPage", parameters);

        }
    }
}
