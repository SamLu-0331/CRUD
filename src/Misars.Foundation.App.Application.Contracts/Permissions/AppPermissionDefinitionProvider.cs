using Misars.Foundation.App.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Misars.Foundation.App.Permissions;

public class AppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var AppGroup = context.AddGroup(AppPermissions.GroupName, L("Permission:App"));

        var patientsPermission = AppGroup.AddPermission(AppPermissions.Patients.Default, L("Permission:Patients"));
        patientsPermission.AddChild(AppPermissions.Patients.Create, L("Permission:Patients.Create"));
        patientsPermission.AddChild(AppPermissions.Patients.Edit, L("Permission:Patients.Edit"));
        patientsPermission.AddChild(AppPermissions.Patients.Delete, L("Permission:Patients.Delete"));
        var doctorsPermission = AppGroup.AddPermission(
            AppPermissions.Doctors.Default, L("Permission:Doctors"));
        doctorsPermission.AddChild(
            AppPermissions.Doctors.Create, L("Permission:Doctors.Create"));
        doctorsPermission.AddChild(
            AppPermissions.Doctors.Edit, L("Permission:Doctors.Edit"));
        doctorsPermission.AddChild(
            AppPermissions.Doctors.Delete, L("Permission:Doctors.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AppResource>(name);
    }
}
