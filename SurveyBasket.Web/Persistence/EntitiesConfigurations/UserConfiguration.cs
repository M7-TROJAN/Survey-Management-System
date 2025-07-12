namespace SurveyBasket.Web.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100);
        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        // prevent duplicated email and username
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();

        // prevent duplicated phone number if not null
        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("IX_ApplicationUser_PhoneNumber")
            .HasFilter("[PhoneNumber] IS NOT NULL"); // only unique if not null (e.g. for users who don't have a phone number)

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        // configure the relationship with RefreshTokens
        builder.OwnsMany(u => u.RefreshTokens)
            .ToTable("RefreshTokens") // specify the table name for refresh tokens
            .WithOwner() // specify that RefreshTokens is owned by ApplicationUser
            .HasForeignKey("UserId"); // specify the foreign key in RefreshTokens that references ApplicationUser
    }
}