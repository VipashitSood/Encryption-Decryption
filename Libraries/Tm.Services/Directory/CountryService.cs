﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tm.Core.Caching;
using Tm.Core.Domain.Directory;
using Tm.Data;
using Tm.Services.Caching;
using Tm.Services.Caching.Extensions;

namespace Tm.Services.Directory
{
    /// <summary>
    /// Country service
    /// </summary>
    public partial class CountryService : ICountryService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IRepository<Country> _countryRepository;

        #endregion

        #region Ctor

        public CountryService(ICacheKeyService cacheKeyService,
            IStaticCacheManager staticCacheManager,
            IRepository<Country> countryRepository)
        {
            _cacheKeyService = cacheKeyService;
            _staticCacheManager = staticCacheManager;
            _countryRepository = countryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void DeleteCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Delete(country);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountries(int languageId = 0, bool showHidden = false)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(TmDirectoryDefaults.CountriesAllCacheKey, languageId, showHidden);

            return _staticCacheManager.Get(key, () =>
            {
                var query = _countryRepository.Table;

                if (!showHidden)
                    query = query.Where(c => c.Published);

                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                var countries = query.ToList();

                return countries;
            });
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;
            
            return _countryRepository.ToCachedGetById(countryId);
        }

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetCountriesByIds(int[] countryIds)
        {
            if (countryIds == null || countryIds.Length == 0)
                return new List<Country>();

            var query = from c in _countryRepository.Table
                        where countryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Country>();
            foreach (var id in countryIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }

            return sortedCountries;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return null;

            var key = _cacheKeyService.PrepareKeyForDefaultCache(TmDirectoryDefaults.CountriesByTwoLetterCodeCacheKey, twoLetterIsoCode);

            var query = from c in _countryRepository.Table
                where c.TwoLetterIsoCode == twoLetterIsoCode
                select c;

            return query.ToCachedFirstOrDefault(key);
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByThreeLetterIsoCode(string threeLetterIsoCode)
        {
            if (string.IsNullOrEmpty(threeLetterIsoCode))
                return null;

            var key = _cacheKeyService.PrepareKeyForDefaultCache(TmDirectoryDefaults.CountriesByThreeLetterCodeCacheKey, threeLetterIsoCode);

            var query = from c in _countryRepository.Table
                where c.ThreeLetterIsoCode == threeLetterIsoCode
                select c;

            return query.ToCachedFirstOrDefault(key);
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void InsertCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Insert(country);
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void UpdateCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Update(country);
        }

        #endregion
    }
}