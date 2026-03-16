using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IAiService
    {
        Task<string> ProcessQuestionAsync(int userId, string role, string question);
    }
}
