using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Misars.Foundation.App.Doctors;

public class Doctor : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public DateTime BirthDate { get; set; }
    public string ShortBio { get; set; }

    private Doctor()
    {
        /* This constructor is for deserialization / ORM purpose */
    }

    internal Doctor(
        Guid id,
        string name,
        DateTime birthDate,
        string? shortBio = null)
        : base(id)
    {
        SetName(name);
        BirthDate = birthDate;
        ShortBio = shortBio;
    }

    internal Doctor ChangeName(string name)
    {
        SetName(name);
        return this;
    }

    private void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name, 
            nameof(name), 
            maxLength: DoctorConsts.MaxNameLength
        );
    }
}
