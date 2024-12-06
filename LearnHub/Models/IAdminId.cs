using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public interface IAdminId
    {
        Guid? AdminId { get; set; }
        Admin Admin { get; set; }
    }
}
