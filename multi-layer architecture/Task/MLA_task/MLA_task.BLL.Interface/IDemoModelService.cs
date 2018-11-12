using System.Collections.Generic;
using System.Threading.Tasks;
using MLA_task.BLL.Interface.Models;

namespace MLA_task.BLL.Interface
{
    public interface IDemoModelService
    {
        Task<IEnumerable<DemoModel>> GetAllDemoModelsAsync();

        Task<DemoModel> GetDemoModelByIdAsync(int id);

        Task AddDemoModelAsync(DemoModel model);
    }
}