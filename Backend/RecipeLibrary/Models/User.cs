using System;
using System.Collections.Generic;

namespace RecipeLibrary.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// A unique username chosen by the user.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// A plain text password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The set of recipe identifiers this user has marked as favourite.
        /// </summary>
        public HashSet<Guid> FavouriteRecipeIds { get; set; } = new HashSet<Guid>();
    }
}