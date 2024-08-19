using Misars.Foundation.App.SurgeryTimetables;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;
using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Doctors;

namespace Misars.Foundation.App.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class AppDbContext :
    AbpDbContext<AppDbContext>,
    ISaasDbContext,
    IIdentityProDbContext
{
    public DbSet<SurgeryTimetable> SurgeryTimetables { get; set; } = null!;
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureLanguageManagement();
        builder.ConfigureFileManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureGdpr();
        builder.ConfigureBlobStoring();
        builder.ConfigurePermissionManagement();
        /* Configure your own tables/entities inside here */
        builder.Entity<Patient>(b =>
                {
                    b.ToTable(AppConsts.DbTablePrefix + "Patients", AppConsts.DbSchema);
                    b.ConfigureByConvention(); //auto configure for the base class props
                    b.Property(x => x.Name).IsRequired().HasMaxLength(128);
                    b.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).IsRequired();
                });
        builder.Entity<Doctor>(b =>
                {
                    b.ToTable(AppConsts.DbTablePrefix + "Doctors",
                        AppConsts.DbSchema);

                    b.ConfigureByConvention();

                    b.Property(x => x.Name)
                        .IsRequired()
                        .HasMaxLength(DoctorConsts.MaxNameLength);

                    b.HasIndex(x => x.Name);
                });

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(AppConsts.DbTablePrefix + "YourEntities", AppConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SurgeryTimetable>(b =>
            {
                b.ToTable(AppConsts.DbTablePrefix + "SurgeryTimetables", AppConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(SurgeryTimetable.Name)).IsRequired();
                b.Property(x => x.BirthDate).HasColumnName(nameof(SurgeryTimetable.BirthDate));
                b.HasOne<SurgeryTimetable>().WithMany().HasForeignKey(x => x.SurgeryTimetableId).OnDelete(DeleteBehavior.SetNull);
            });

        }
    }
}