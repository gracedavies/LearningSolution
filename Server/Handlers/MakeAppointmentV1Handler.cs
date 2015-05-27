using System;

namespace Server.Handlers
{
    using Data;
    using Doctors.Surgery.Contracts.V1;
    using NServiceBus;

    public class MakeAppointmentV1Handler : IHandleMessages<MakeAppointmentV1>
    {
        public IBus Bus { get; set; }


        public void Handle(MakeAppointmentV1 message)
        {
            Console.Write("Im in the handler!");
            var repository = new AppointmentRepository();
            var appointment = repository.MakeAppointment(message.Name);
            Bus.Reply(new MakeAppointmentV1() { AppointmentId = appointment.AppointmentId, Name = appointment.Name });
        }
    }
}
