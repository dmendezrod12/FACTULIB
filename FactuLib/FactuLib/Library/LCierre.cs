using FactuLib.Data;
using Microsoft.EntityFrameworkCore;

namespace FactuLib.Library
{
    public class LCierre : ListObject
    {
        public LCierre(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
