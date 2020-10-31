﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GpsTracker.Database;
using Unity;

namespace GpsTracker
{
    public class LocationService
    {
        private readonly DatabaseService _databaseService;

        public LocationService()
        {
            _databaseService = DependencyInjection.Container.Resolve<DatabaseService>();
        }

        public void AddLocation(Location location)
        {
            var locationEntity = new LocationEntity(
                provider: location.Provider,
                longitude: location.Longitude,
                latitude: location.Latitude,
                accuracy: location.Accuracy,
                altitude: location.Altitude,
                speed: location.Speed,
                time: location.Time
            );

            _databaseService.Insert(locationEntity);
        }

        public List<LocationEntity> QueryLocation(DateTime from, DateTime to)
        {
            return _databaseService.Query<LocationEntity>()
                .Where(i => i.DateTime >= from && i.DateTime <= to)
                .ToList();
        }

        public List<LocationEntity> QueryNotUploadedLocations()
        {
            return _databaseService.Query<LocationEntity>()
              .Where(i => !i.IsUploaded)
              .ToList();
        }

        public void UpdateLocations(List<LocationEntity> locations)
        {
            _databaseService.UpdateAll(locations);
        }
    }
}