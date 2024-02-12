namespace ExpenseTrackingSystem.Entities.Models
{
    public class CustomUser : Microsoft.AspNetCore.Identity.IdentityUser<string>
    {
        public string RoleId { get; set; }
        public CustomUserRole UserRole { get; set; }
    }
}
