using ChatApp.Core.Entities.FooAggregate;
using ChatApp.Core.Interfaces;

namespace ChatApp.Services;

    public class FooService : IFooService
{
    private IUnitOfWork _unitOfWork;

    public FooService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Foo>> GetAsync()
    {
        var repo = _unitOfWork.GetRepository<Foo>();
        return await repo.GetAsync();
    }
    
    public async Task<List<Foo>> GetFilteredAsync(string filter)
    {
        return await _unitOfWork.Foos.GetFilteredAsync(filter);
    }

    public async Task<int> CountAsync()
    {
        return await _unitOfWork.Foos.CountAsync();
    }

    public async Task<Foo?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Foos.GetByIdAsync(id);
    }

    public async Task<List<Foo>> AddAsync(Foo foo)
    {
        _unitOfWork.Foos.Add(foo);
        await _unitOfWork.SaveChangesAsync();
      
        return await _unitOfWork.Foos.GetAsync();
    }

    public async Task<List<Foo>> UpdateAsync(Foo foo)
    {
        _unitOfWork.Foos.Update(foo);
        await _unitOfWork.SaveChangesAsync();

        return await _unitOfWork.Foos.GetAsync();
    }



    public async Task<List<Foo>> DeleteAsync(Foo foo)
    {
        _unitOfWork.Foos.Delete(foo);
        await _unitOfWork.SaveChangesAsync();

        return await _unitOfWork.Foos.GetAsync();
    }
}