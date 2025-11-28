using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Entities
{
    public class UpdatedUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}

