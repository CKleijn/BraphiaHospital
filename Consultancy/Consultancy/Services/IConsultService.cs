using Consultancy.Models;

namespace Consultancy.Services
{
    public interface IConsultService
    {
        List<Consult> Get();
        Consult GetById(int id);
        Consult DeleteById(int id);
        Consult UpdateById(int id);
        Consult Create(Consult consult);
    }
}
