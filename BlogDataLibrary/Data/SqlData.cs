using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlogDataLibrary.Data
{
    public class SqlData : ISqlData
    {
        private readonly ISqlDataAccess _db;
        private readonly string _connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public UserModel Authenticate(string username, string password)
        {
            var result = _db.LoadData<UserModel, dynamic>(
                "spUsers_Authenticate",
                new { username, password },
                _connectionStringName).Result;

            return result.FirstOrDefault();
        }

        public void Register(string username, string firstName, string lastName, string password)
        {
            _db.SaveData(
                "spUsers_Register",
                new { username, firstName, lastName, password },
                _connectionStringName);
        }

        public void AddPost(PostModel post)
        {
            _db.SaveData("spPosts_Insert", post, _connectionStringName);
        }

        public List<ListPostModel> ListPosts()
        {
            return _db.LoadData<ListPostModel, dynamic>(
                "spPosts_List",
                new { },
                _connectionStringName).Result;
        }

        public ListPostModel ShowPostDetails(int id)
        {
            var results = _db.LoadData<ListPostModel, dynamic>(
                "spPosts_Detail",
                new { id = id },
                _connectionStringName).Result;

            return results.FirstOrDefault();
        }
    }
}