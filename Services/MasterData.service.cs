/*
 * File: Master Data Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Master Data management.
*/

using EAD_Backend.Data;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{

    public class MasterDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Role> _roleModel;


        public MasterDataService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            // _userModel = mongoDbService.Database?.GetCollection<User>("users");
            _roleModel = mongoDbService.Database?.GetCollection<Role>("roles");

        }

        //Get Roles
        public List<Role> GetRoles()
        {
            return _roleModel.Find(role => true).ToList();
        }
    }
}