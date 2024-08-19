using System;
using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.Patients;

public class DoctorLookupDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
}
