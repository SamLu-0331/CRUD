using System;
using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.Doctors;

public class DoctorDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public string ShortBio { get; set; } = string.Empty;
}
