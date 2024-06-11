using Consultancy.Models;

namespace Consultancy.Repositories
{
    public interface IConsultRepository
    {
        List<Consult> Get();
        Consult GetById(int id);
        Consult DeleteById(int id);
        Consult UpdateById(int id);
        Consult Create(Consult consult);
    }
}
