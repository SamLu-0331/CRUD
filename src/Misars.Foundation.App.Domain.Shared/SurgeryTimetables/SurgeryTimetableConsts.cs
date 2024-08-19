namespace Misars.Foundation.App.SurgeryTimetables
{
    public static class SurgeryTimetableConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "SurgeryTimetable." : string.Empty);
        }

    }
}