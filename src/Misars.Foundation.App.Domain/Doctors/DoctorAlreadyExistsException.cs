using Volo.Abp;

namespace Misars.Foundation.App.Doctors;

public class DoctorAlreadyExistsException : BusinessException
{
    public DoctorAlreadyExistsException(string name)
        : base(AppDomainErrorCodes.DoctorAlreadyExists)
    {
        WithData("name", name);
    }
}
