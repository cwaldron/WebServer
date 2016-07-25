using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

using ProviderHost.Annotations;

using Vensa.Components.Addon;
using Vensa.Data;
using Vensa.Data.Model;
using Vensa.Data.Model.AddOn;

namespace ProviderHost
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    [UsedImplicitly]
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private CompositionContainer _container;

        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        [UsedImplicitly]
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            // Initialize database.
            Storage database;
            TinyIoCContainer.Current.TryResolve(out database);
            if (database == null)
            {
                database = new Storage("mongodb://localhost:27017/models");
                RegisterModels(database);
                TinyIoCContainer.Current.Register(database);
                StageUser(TinyIoCContainer.Current);
            }

            // Initialize the addons (MEF modules)
            var catalog = new AggregateCatalog();
            foreach (var path in GetAddonPaths())
            {
                var addonCatelog = new DirectoryCatalog(path);
                var filteredCatalog = new FilteredCatalog(addonCatelog, cpd => cpd.Exports<IAddonModule>());
                catalog.Catalogs.Add(filteredCatalog);
            }

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
            var addons = _container.GetExportedValues<IAddonModule>();
            TinyIoCContainer.Current.Register(addons);
        }

        /// <summary>
        /// Retrieves the addon paths.
        /// </summary>
        /// <returns>collection of addon paths</returns>
        private IEnumerable<string> GetAddonPaths()
        {
            var uri = new UriBuilder(GetType().Assembly.CodeBase);
            var hostPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            if (hostPath == null) return new string[0];
            var addonRootPath = Path.Combine(hostPath, @"Content\Addons");
            return Directory.GetDirectories(addonRootPath);
        }

        private static void RegisterModels(Storage storage)
        {
            storage.CreateRepository<User>();
            storage.CreateRepository<UserAccount>();
            storage.CreateRepository<AddOn>();
            storage.CreateRepository<Authentication>();
            storage.CreateRepository<Content>();
            storage.CreateRepository<Vendor>();
        }

        private void StageUser(TinyIoCContainer container)
        {
            var database = container.Resolve<Storage>();

            var admin = new User
            {
                Name = "admin",
                Authorization = new UserAuthorization
                {
                    Password = "P@ssw0rd"
                },

                Profile = new UserProfile
                {
                    FirstName = "admin",
                    LastName = "admin",
                    PreferredName = "Admin"
                },

                Accounts = new[]
                {
                    new UserAccount
                    {
                        ImageUrl = "http://www.imageurl/mystormphoto.jpg",
                        ProviderId = Storage.GenerateId(),
                        FacilityId = Storage.GenerateId(),
                        MobileNumber = "0273006667",
                        PhoneNumber = "095229522",
                        EmailAddress = "admin@storm.vensa.com",
                        Description = "Admin Account"
                    }
                }
            };

            database.Save(admin);

            var user = new User
            {
                Name = "jackharris",
                Authorization = new UserAuthorization
                {
                    Password = "Password12",                                    
                },
                Profile = new UserProfile
                {
                    FirstName = "Jack",
                    LastName = "Harris",
                    PreferredName = "Jake"
                },
                Accounts = new[]
                {
                    new UserAccount
                    {
                        ImageUrl = "http://www.imageurl/mystormphoto.jpg",
                        ProviderId = Storage.GenerateId(),
                        FacilityId = Storage.GenerateId(),
                        MobileNumber = "0273006667",
                        PhoneNumber = "095229522",
                        EmailAddress = "jharris@storm.vensa.com",
                        Description = "Jack Harris Storm Account"
                    },
                    new UserAccount
                    {
                        ImageUrl = "http://www.imageurl/myemployeephoto.jpg",
                        ProviderId = ObjectId.GenerateNewId(),
                        FacilityId = ObjectId.GenerateNewId(),
                        MobileNumber = "0252502500",
                        PhoneNumber = "094277777",
                        EmailAddress = "jackharris@employer.com",
                        Description = "Jack Harris Employee Account"
                    },
                    new UserAccount
                    {
                        ImageUrl = "http://www.imageurl/myphoto.jpg",
                        ProviderId = ObjectId.GenerateNewId(),
                        FacilityId = ObjectId.GenerateNewId(),
                        MobileNumber = "0271003333",
                        PhoneNumber = "094158883",
                        EmailAddress = "jackharris@facility.com",
                        Description = "Jack Harris Facility Account"
                    }
                }
            };

            database.Save(user);
        }
    }
}