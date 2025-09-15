using Microsoft.AspNetCore.Identity;

namespace Domain.Enums;

public enum Roles
{ 
     Patient, 
     Doctor,
     Reception
}

public class UserRole : IdentityRole<Guid>;