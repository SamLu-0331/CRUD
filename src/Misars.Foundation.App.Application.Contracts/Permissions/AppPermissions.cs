namespace Misars.Foundation.App.Permissions;

public static class AppPermissions
{
    public const string GroupName = "App";
    
    // other permissions...
    // other permissions...

 	// *** ADDED a NEW NESTED CLASS ***
    public static class Patients
    {
        public const string Default = GroupName + ".Patients";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Doctors
    {
        public const string Default = GroupName + ".Doctors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
