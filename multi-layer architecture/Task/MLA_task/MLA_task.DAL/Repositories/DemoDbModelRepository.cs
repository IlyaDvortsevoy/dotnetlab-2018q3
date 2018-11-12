using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MLA_task.DAL.EF;
using MLA_task.DAL.Interface;
using MLA_task.DAL.Interface.Entities;

namespace MLA_task.DAL.Repositories
{
    public class DemoDbModelRepository : IDemoDbModelRepository
    {
        private readonly DemoContext _context;

        public DemoDbModelRepository(DemoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DemoDbModel>> GetAllAsync()
        {
            return await _context.DemoDbModels.ToListAsync();
        }

        public async Task<DemoDbModel> GetByIdAsync(int id)
        {
            var model = await _context.DemoDbModels.SingleAsync(item => item.Id == id);

            return model;
        }

        public async Task<DemoCommonInfoDbModel> GetCommonInfoByDemoIdAsync(int demoDbModelId)
        {
            var demoModel = await _context.DemoDbModels.SingleAsync(item => item.Id == demoDbModelId);

            var commonInfo = await _context.DemoCommonInfoModels.SingleAsync(item => item.Id == demoModel.DemoCommonInfoModelId);

            return commonInfo;
        }

        public async Task<DemoCommonInfoDbModel> GetCommonInfoByCommonInfoAsync(string commonInfo)
        {
            if (commonInfo == null)
            {
                return null;
            }

            var info = await _context.DemoCommonInfoModels.SingleAsync(
                item => item.CommonInfo == commonInfo);

            return info;
        }

        public async Task AddAsync(DemoDbModel model)
        {
            _context.DemoDbModels.Add(model);

            await _context.SaveChangesAsync();
        }
    }
}