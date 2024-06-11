using Consultancy.Models;
using Consultancy.Repositories;
using System.Text.Json;

namespace Consultancy.Services
{
    public class ConsultService : IConsultService
    {
        private readonly IConsultRepository _consultRepository;

        public ConsultService(IConsultRepository consultRepository)
        {
            _consultRepository = consultRepository;
        }

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
            return _consultRepository.Create(consult);
        }
    }
}
