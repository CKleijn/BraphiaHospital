using Consultancy.Models;
using Consultancy.Utils;
using System.Text.Json;

namespace Consultancy.Repositories
{
    public class ConsultRepository : IConsultRepository
    {
        public List<Consult> Get()
        {
            // TODO: Retrieve all consults
            return null;
        }

        public Consult GetById(int id)
        {
            // TODO: Get consult by id
            return null;
        }

        public Consult DeleteById(int id)
        {
            // TODO: Delete consult by id
            return null;
        }

        public Consult UpdateById(int id)
        {
            // TODO: Update consult by id
            return null;
        }

        public Consult Create(Consult consult)
        {
            string consultJson = JsonSerializer.Serialize(consult);
            DatabaseEventOperations.AddEvent("Create: " + consultJson);
            return consult;
        }
    }
}
