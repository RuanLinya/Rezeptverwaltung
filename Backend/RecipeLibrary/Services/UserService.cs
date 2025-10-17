using System;
using System.Linq;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    /// <summary>
    /// Provides functionality to manage users including registration and
    /// authentication.  User names are unique across the system.
    /// </summary>
    public class UserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user.  Throws an exception if the user name is
        /// already taken or if any argument is null or empty.
        /// </summary>
        public User Register(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("User name must not be empty.", nameof(userName));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password must not be empty.", nameof(password));
            if (_context.Users.Any(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"User name '{userName}' is already in use.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                Password = password
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Attempts to authenticate a user with the provided credentials.  If
        /// successful the matching User instance is returned; otherwise null.
        /// </summary>
        public User? Authenticate(string userName, string password)
        {
            return _context.Users.FirstOrDefault(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase)
                                                     && u.Password == password);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.  Returns null if not
        /// found.
        /// </summary>
        public User? GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}