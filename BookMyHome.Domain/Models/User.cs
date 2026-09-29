using System;
using System.Collections.Generic;
using System.Text;

namespace BookMyHome.Domain.Models
{
    public abstract class User
    {
        public Guid UserId { get; protected set; }

        public string Name { get; protected set; } = string.Empty;

        public string Email { get; protected set; } = string.Empty;

        public string? PasswordHash { get; protected set; }

        public void SetPasswordHash(string passwordHash)
{
    if (string.IsNullOrWhiteSpace(passwordHash))
    {
        throw new ArgumentException(
            "Password hash cannot be empty.",
            nameof(passwordHash));
    }

    PasswordHash = passwordHash;
}

        protected User()
        {
        }

        protected User(string name, string email)
        {
            UserId = Guid.NewGuid();
            Name = name;
            Email = email;
        }
    }
}
