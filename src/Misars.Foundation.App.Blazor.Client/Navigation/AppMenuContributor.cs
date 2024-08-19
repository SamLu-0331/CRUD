using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Misars.Foundation.App.Localization;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.LanguageManagement.Blazor.Menus;
using Volo.Abp.TextTemplateManagement.Blazor.Menus;
using Volo.Abp.OpenIddict.Pro.Blazor.Menus;
using Volo.Saas.Host.Blazor.Navigation;

namespace Misars.Foundation.App.Blazor.Client.Navigation;

public class AppMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public AppMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
{
    var l = context.GetLocalizer<AppResource>();

    context.Menu.Items.Insert(
        0,
        new ApplicationMenuItem(
            "App.Home",
            l["Menu:Home"],
            "/",
            icon: "fas fa-home"
        )
    );

    var appMenu = new ApplicationMenuItem(
        "App",
        l["Menu:App"],
        icon: "fa fa-book"
    );

    context.Menu.AddItem(appMenu);

    //CHECK the PERMISSION
    if (await context.IsGrantedAsync(AppPermissions.Patients.Default))
    {
        appMenu.AddItem(new ApplicationMenuItem(
            "App.Patients",
            l["Menu:Patients"],
            url: "/Patients"
        ));
    }
    if (await context.IsGrantedAsync(AppPermissions.Doctors.Default))
    {
        context.Menu.AddItem(new ApplicationMenuItem(
            "App.Doctors",
            l["Menu:Doctors"],
            url: "/doctors"
        ));
    }

}


    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.SecurityLogs",
            accountStringLocalizer["MySecurityLogs"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/SecurityLogs?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-user-shield",
            order: 1001,
            null).RequireAuthenticated());

        await Task.CompletedTask;
    }
}
