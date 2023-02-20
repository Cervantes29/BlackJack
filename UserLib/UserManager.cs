using DataAccessDAL;
using Microsoft.EntityFrameworkCore;
using ModelsEL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackLogicBLL.ViewModel
{
    /// <summary>
    /// Handles all user related logic
    /// </summary>
    public partial class UserManager
    {
        private List<User> users = new();

        public UserManager()
        {
            GetUsers();
        }

        /// <summary>
        /// Gets a user from database
        /// </summary>
        /// <param name="searchByID"></param>
        /// <returns></returns>
        public User GetUser(int searchByID)
        {
            User user = new User();
            List<User> userList = GetUsers();
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].Id.Equals(searchByID))
                {
                    user = userList[i];
                }
            }
            return user;
        }

        /// <summary>
        /// Gets a list of all users from database
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using var db = new GameDbContext(optionsBuilder.Options);
            var Users = (from dbUser in db.Users
                         select new User(
                dbUser.Username,
                dbUser.Balance,
                dbUser.Id
                         )).ToList();
            return Users;
        }

        /// <summary>
        /// Method for saving multiple users to database
        /// </summary>
        /// <param name="users"></param>
        private void SaveUsersToDatabase(List<User> users)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            int count = GetUsers().Count;

            for (int i = 0; i < count; i++)
            {
                User user = users[i];
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Adds a user to database
        /// </summary>
        /// <param name="user"></param>
        public void Add(User user)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            context.Update(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Clears all users from database
        /// </summary>
        private void Clear()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            int count = GetUsers().Count;

            foreach (var entity in context.Users)
                context.Users.Remove(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Replaces a user in database
        /// </summary>
        /// <param name="user"></param>
        public void Insert(User user)
        {
            GetUser(user.Id);
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            {
                User result = context.Users.SingleOrDefault(IdCompare => IdCompare.Id == user.Id);
                if (result != null)
                {
                    result.Id = user.Id;
                    result.Balance = user.Balance;
                    result.Username = user.Username;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Removes a user from databse at a provided index
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            context.Remove(GetUsers()[index]);
            context.SaveChanges();
        }

        /// <summary>
        /// Searches for a user using a string
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public User SearchUser(string search)
        {
            User user = new();
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            return user = context.Users.FirstOrDefault(user => user.Username == search);
        }
    }
}