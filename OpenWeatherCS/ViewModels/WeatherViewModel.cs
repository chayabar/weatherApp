﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;
using System.Net.Http;
using BE;
using OpenWeatherCS.Utils;

namespace OpenWeatherCS.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private IWeatherService weatherService;
        private IDialogService dialogService;

        public WeatherViewModel(IWeatherService ws, IDialogService ds)
        {
            weatherService = ws;
            dialogService = ds;
        }

        private List<DailyForeCast> _forecast;
        public List<DailyForeCast> Forecast
        {
            get { return _forecast; }
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        private DailyForeCast _currentWeather = new DailyForeCast();
        public DailyForeCast CurrentWeather
        {
            get { return _currentWeather; }
            set
            {
                _currentWeather = value;
                OnPropertyChanged();
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private ICommand _getWeatherCommand;
        public ICommand GetWeatherCommand
        {
            get
            {
                if (_getWeatherCommand == null) _getWeatherCommand = 
                        new RelayCommandAsync(() => GetWeather(), (o) => CanGetWeather());               
                return _getWeatherCommand;
            }
        }

        public async Task GetWeather()
        {
            try
            {
                var weather = await weatherService.GetForecastAsync(Location, 40);
                CurrentWeather = weather.First();
                Forecast = weather.Skip(1).ToList();
            }
            catch (HttpRequestException ex) {
                dialogService.ShowNotification("Ensure you're connected to the internet!", "Open Weather");
            }
            catch (LocationNotFoundException ex)
            {
                dialogService.ShowNotification("Location not found!", "Open Weather");
            }
            
        }

        public Boolean CanGetWeather()
        {
            return Location != string.Empty;
        }
    }
}
