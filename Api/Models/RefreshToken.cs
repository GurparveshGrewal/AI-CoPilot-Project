namespace Api.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public String TokenHash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; } // when this token will be revoked, this field wont be null, so we can detect theft
        public Guid? ReplacedByToken { get; set; } // useful to know which token is now new
    }
}
