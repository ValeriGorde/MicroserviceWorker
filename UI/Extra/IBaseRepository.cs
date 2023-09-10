using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Extra
{
    public interface IBaseRepository
    {
        public Task GetAll();
    }
}
