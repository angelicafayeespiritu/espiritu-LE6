using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlogTestUI
{
    class Program
    {
        static IConfiguration GetConnection()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        // GET CURRENT USER
        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username : ");
            string username = Console.ReadLine();

            Console.Write("Password : ");
            string password = Console.ReadLine();

            //UserModel user = db.Authenticate(username, password).Result;
            UserModel user = db.Authenticate(username, password);

            return user;
        }

        // AUTHENTICATE
        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            Console.WriteLine();
            Console.WriteLine("========================================");

            if (user == null)
            {
                Console.WriteLine(" Invalid credentials.");
            }
            else
            {
                Console.WriteLine($" Welcome, {user.UserName}!");
                Console.WriteLine("========================================");

                AddPost(db);
                ListPosts(db);
                ShowPostDetails(db);
            }
        }

        // REGISTER
        public static void Register(SqlData db)
        {
            // Console.WriteLine("========== USER REGISTRATION ==========");

            Console.Write("Enter new username : ");
            var username = Console.ReadLine();

            Console.Write("Enter new password : ");
            var password = Console.ReadLine();

            Console.Write("Enter first name   : ");
            var firstName = Console.ReadLine();

            Console.Write("Enter last name    : ");
            var lastName = Console.ReadLine();

            db.Register(username, firstName, lastName, password);

            Console.WriteLine();
            Console.WriteLine(" User registered successfully!");
            Console.WriteLine("=======================================");
            Console.WriteLine();
        }

        // ADD POST
        private static void AddPost(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            Console.WriteLine();
           // Console.WriteLine("============ ADD NEW POST ============");

            Console.Write("Title      : ");
            string title = Console.ReadLine();

            Console.Write("Write body : ");
            string body = Console.ReadLine();

            PostModel post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            db.AddPost(post);

            Console.WriteLine();
            Console.WriteLine(" Post added successfully!");
            Console.WriteLine("======================================");
            Console.WriteLine();
        }

        // LIST POSTS
        private static void ListPosts(SqlData db)
        {
            Console.WriteLine();
            Console.WriteLine("============= LIST OF POSTS =============");
            Console.WriteLine();

            //List<PostModel> posts = db.ListPosts();
            List<ListPostModel> posts = db.ListPosts();

            //foreach (PostModel post in posts)
            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"Post ID : {post.Id}");
                Console.WriteLine($"Title   : {post.Title}");
                Console.WriteLine($"Author  : {post.UserName}");
                Console.WriteLine($"Date    : {post.DateCreated:MM/dd/yyyy hh:mm tt}");
                Console.WriteLine($"Preview : {post.Body.Substring(0, Math.Min(post.Body.Length, 20))}...");
                Console.WriteLine(new string('-', 42));
            }

            Console.WriteLine();
        }

        // SHOW POST DETAILS
        private static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a post ID : ");
            int id = int.Parse(Console.ReadLine());

            //PostModel post = db.ShowPostDetails(id);
            ListPostModel post = db.ShowPostDetails(id);

            Console.WriteLine();

            if (post == null)
            {
                Console.WriteLine(" Post not found.");
                return;
            }

            Console.WriteLine("=========== POST DETAILS ===========");
            Console.WriteLine($"Author : {post.FirstName} {post.LastName} ({post.UserName})");
            Console.WriteLine($"Date   : {post.DateCreated:MM/dd/yyyy hh:mm tt}");
            Console.WriteLine(new string('-', 38));
            Console.WriteLine($"Title  : {post.Title}");
            Console.WriteLine();
            Console.WriteLine(post.Body);
            Console.WriteLine(new string('=', 38));
            Console.WriteLine();
        }

        // MAIN
        static void Main(string[] args)
        {
            IConfiguration config = GetConnection();

            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dbAccess);

            Register(db);

            Authenticate(db);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}