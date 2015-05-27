using Doctors.Surgery.Contracts.Rest;
using Doctors.Surgery.Contracts.V1;

namespace Data
{
    using Dapper;
    using Doctors.Surgery.Contracts;
    using System;
    using System.Data.SqlClient;
    using System.Linq;


    public class AppointmentRepository
    {
        private const string Databaseconnectionstring = @"Data Source=LPX-14-2797FS\GRACESDATABASE;Initial Catalog=DoctorsSurgery;Integrated Security=True";

        private readonly SqlConnection sqlConnection;

        public AppointmentRepository()
        {
            sqlConnection = new SqlConnection(Databaseconnectionstring);
        }

        public Appointment GetAppointmentByName(int appointmentId)
        {
            using (sqlConnection)
            {
                sqlConnection.Open();
                var people = sqlConnection.Query<Appointment>("SELECT * FROM Appointments WHERE AppointmentId = @ID",
                    new
                    {
                        ID = appointmentId
                    });

                return new Appointment()
                {
                    AppointmentId = people.First().AppointmentId,
                    Name = people.First().Name
                };
            }
        }

        public Appointment MakeAppointment(string name)
        {
            var appointment = new Appointment {Name = name};
            using (sqlConnection)
            {
                sqlConnection.Open();

                var id = sqlConnection.Query<int>("INSERT INTO Appointments (Name) VALUES (@name)" +
                                                  "SELECT CAST(SCOPE_IDENTITY() as int)",
                    new
                    {
                        name = name
                    });

                appointment.AppointmentId = Convert.ToInt32(id.ElementAt(0));

                sqlConnection.Close();
            }

            return appointment;
        }
    }
}
