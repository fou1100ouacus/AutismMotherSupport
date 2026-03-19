
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Models.IdentityUser
{
    public class AppUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Navigation properties
        public List<RefreshToken> RefreshTokens { get; set; } = new();
        
        public string TimeZoneId { get; private set; } = "Egypt Standard Time";

        public Child Child { get; private set; } = null!;   // one child per mother

        // ──────────────────────────────────────────────────────────────
        // NEW: Required by the project spec (Onboarding & Legal Safety)
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Legal safety – must be true before any other features are unlocked
        /// </summary>
        public bool HasAcceptedDisclaimer { get; private set; } = false;

        public DateTime? DisclaimerAcceptedAt { get; private set; }

        /// <summary>
        /// Bilingual support (English / Arabic)
        /// </summary>

        /// <summary>
        /// Account creation timestamp (explicit, even though IdentityUser has some dates)
        /// </summary>
        public DateTime AccountCreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Status used for banning / deactivation (not just LockoutEnabled)
        /// </summary>
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        /// <summary>
        /// Last successful login (used for analytics + security)
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        // Optional but highly recommended for Egypt project
        public string? PhoneNumber { get; set; }           // WhatsApp / contact
        public bool IsPhoneVerified { get; set; } = false;
    }

    // Enums (clean & type-safe)
    public enum PreferredLanguage
    {
        English,
        Arabic
    }

    public enum AccountStatus
    {
        Active,
        Inactive,
        Banned
    }
}