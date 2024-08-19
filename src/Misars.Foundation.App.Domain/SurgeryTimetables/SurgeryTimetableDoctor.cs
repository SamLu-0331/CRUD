using System;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableDoctor : Entity
    {

        public string SurgeryTimetableId { get; protected set; }

        public Guid DoctorId { get; protected set; }

        private SurgeryTimetableDoctor()
        {

        }

        public SurgeryTimetableDoctor(string surgeryTimetableId, Guid doctorId)
        {
            SurgeryTimetableId = surgeryTimetableId;
            DoctorId = doctorId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    SurgeryTimetableId,
                    DoctorId
                };
        }
    }
}