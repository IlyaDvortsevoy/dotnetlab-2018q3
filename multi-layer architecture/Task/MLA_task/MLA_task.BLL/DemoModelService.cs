using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLA_task.BLL.Interface;
using MLA_task.BLL.Interface.Exceptions;
using MLA_task.BLL.Interface.Models;
using MLA_task.DAL.Interface;
using MLA_task.DAL.Interface.Entities;

namespace MLA_task.BLL
{
    public class DemoModelService : IDemoModelService
    {
        private readonly IDemoDbModelRepository _demoDbModelRepository;

        public DemoModelService(IDemoDbModelRepository demoDbModelRepository)
        {
            _demoDbModelRepository = demoDbModelRepository;
        }

        public async Task<IEnumerable<DemoModel>> GetAllDemoModelsAsync()
        {
            var dbModels = await _demoDbModelRepository.GetAllAsync();

            var demoModels = dbModels.Select(dbM => new DemoModel
            {
                Id = dbM.Id,
                Name = dbM.Name,
                Created = dbM.Created,
                Modified = dbM.Modified,
                CommonInfo = dbM.DemoCommonInfoModel.CommonInfo
            });

            return demoModels;
        }

        public async Task<DemoModel> GetDemoModelByIdAsync(int id)
        {
            if (id == 23) {
                throw new DemoServiceException(DemoServiceException.ErrorType.WrongId);
            }

            var dbModel = await _demoDbModelRepository.GetByIdAsync(id);
            var commonInfo = await _demoDbModelRepository.GetCommonInfoByDemoIdAsync(id);

            var demoModel = new DemoModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Created = dbModel.Created,
                Modified = dbModel.Modified,
                CommonInfo = commonInfo.CommonInfo
            };

            return demoModel;
        }

        public async Task AddDemoModelAsync(DemoModel model)
        {
            var dbModel = new DemoDbModel { Name = model.Name };

            var info = await _demoDbModelRepository
                .GetCommonInfoByCommonInfoAsync(model.CommonInfo);

            dbModel.DemoCommonInfoModelId = info?.Id ?? 1;

            await _demoDbModelRepository.AddAsync(dbModel);
        }
    }
}
