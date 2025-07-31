using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.BestPractices.DTOs
{   
    /// <summary>
    /// Represents the data required to create a new user.
    /// </summary>
    public record CreateUserDto
    {
        /// <summary>
        /// The desired username for the new account. Must be alphanumeric.
        /// </summary>
        /// <example>johndoe123</example>
        public required string Username { get; set; }

        /// <summary>
        /// The user's email address. Must be a valid email format.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public required string Email { get; set; }

        /// <summary>
        /// The user's password. Must be at least 8 characters long and contain an uppercase letter, a lowercase letter, and a number.
        /// </summary>
        /// <example>Password123</example>
        public required string Password { get; set; }
    }
}