using System;
using System.Linq;
using System.Threading.Tasks;
using PrintMyLife.Core.Common.Dependencies;
using PrintMyLife.Core.Common.Repositories;
using PrintMyLife.Core.Common.UnitOWork;
using PrintMyLife.Core.Sample.Entities;

namespace PrintMyLife.Core.Sample
{
  public class SampleManager : IManager
  {
    private ISampleService _sampleService;
    private IUnitOfWork _unitOfWork;
    private IRepository<MySample> _sampleRepository;

    public SampleManager(
      ISampleService sampleService,
      IUnitOfWork unitOfWork)
    {
      _sampleService = sampleService;
      _unitOfWork = unitOfWork;
      _sampleRepository = _unitOfWork.GetRepository<MySample>();
    }

    public IQueryable<MySample> GetAllSamples()
    {
      return _sampleRepository.GetAll();
    }

    public async Task<MySample> GetSampleByIdAsync(Guid id)
    {
      var result = await _sampleRepository.FirstOrDefaultAsync(s => s.Id == id);
      if (result == null)
      {
        throw new ArgumentException();
      }

      return result;
    }

    public async Task<MySample> CreateAsync(MySample mySample)
    {
      var result = await _sampleRepository.InsertAsync(mySample);
      _unitOfWork.SaveChanges();
      return result;
    }

    public async Task<MySample> UpdateAsync(MySample mySample)
    {
      var result = await _sampleRepository.UpdateAsync(mySample);
      _unitOfWork.SaveChanges();
      return result;
    }

    public async Task DeleteAsync(MySample mySample)
    {
      await _sampleRepository.DeleteAsync(mySample);
      _unitOfWork.SaveChanges();
    }
  }
}
