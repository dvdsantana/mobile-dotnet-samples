﻿using System;
using System.Collections.Generic;
using Windows.UI.Popups;

namespace CartoMap.Windows.Pages.Map
{
    public partial class CartoVisPage : MapBasePage
    {
        const string _base = "https://documentation.cartodb.com/api/v2/viz/";

        Dictionary<string, string> items = new Dictionary<string, string> {
            { "Circle", _base + "836e37ca-085a-11e4-8834-0edbca4b5057/viz.json" },
            { "Test", _base + "3ec995a8-b6ae-11e4-849e-0e4fddd5de28/viz.json" },
            { "Countries", _base + "2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json" },
            { "Dots", _base + "236085de-ea08-11e2-958c-5404a6a683d5/viz.json" }
        };
            
        public CartoVisPage()
        {
            // Load the initial visJSON
            string url = items["Dots"];
            MapView.UpdateVisWithGridEvent(url, OnError);
        }

        async void OnError(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }
    }
}
