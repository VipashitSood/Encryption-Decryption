﻿using System.Collections.Generic;
using Tm.Core.Domain.Stores;

namespace Tm.Services.Stores
{
    /// <summary>
    /// Store service interface
    /// </summary>
    public partial interface IStoreService
    {
        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="store">Store</param>
        void DeleteStore(Store store);

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Stores</returns>
        IList<Store> GetAllStores();

        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        Store GetStoreById(int storeId);

        /// <summary>
        /// Inserts a store
        /// </summary>
        /// <param name="store">Store</param>
        void InsertStore(Store store);

        /// <summary>
        /// Updates the store
        /// </summary>
        /// <param name="store">Store</param>
        void UpdateStore(Store store);

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Comma-separated hosts</returns>
        string[] ParseHostValues(Store store);

        /// <summary>
        /// Indicates whether a store contains a specified host
        /// </summary>
        /// <param name="store">Store</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(Store store, string host);

        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        string[] GetNotExistingStores(string[] storeIdsNames);

        /// <summary>
        /// Get store by name
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        Store GetStoreByName(string storeName);
    }
}