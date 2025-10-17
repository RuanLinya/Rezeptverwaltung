using System;
using System.Collections.Generic;

namespace RecipeLibrary.Models
{
    /// <summary>
    /// Represents a user of the recipe system.  Each user has a globally unique
    /// identifier and a user‑supplied name.  For simplicity this example stores
    /// the password in clear text.  In a real system you should use a secure
    /// hashing algorithm such as PBKDF2, bcrypt or Argon2 to store password
    /// hashes instead of plain text passwords.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// A unique username chosen by the user.  The library enforces
        /// uniqueness across all registered users.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// A plain text password.  This sample uses clear text for brevity but
        /// this should never be done in a production environment.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The set of recipe identifiers this user has marked as favourite.
        /// </summary>
        public HashSet<Guid> FavouriteRecipeIds { get; set; } = new HashSet<Guid>();
    }
}