using Consultancy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consultancy.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ConsultController
    {
        [HttpGet]
        public ActionResult<Consult> Get()
        {
            // TODO: Retrieve all consults
            return null;
        }

        [HttpGet("{id}")]
        public ActionResult<Consult> GetById(int id)
        {
            // TODO: Get consult by id
            return null;
        }

        [HttpDelete("{id}")]
        public ActionResult<Consult> DeleteById(int id)
        {
            // TODO: Delete consult by id
            return null;
        }

        [HttpPut("{id}")]
        public ActionResult<Consult> UpdateById(int id)
        {
            // TODO: Update consult by id
            return null;
        }

        [HttpPost]
        public ActionResult<Consult> Create()
        {
            // TODO: Create consult
            return null;
        }
    }
}
