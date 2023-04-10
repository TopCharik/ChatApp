using ChatApp.API.DTOs;
using ChatApp.Core.Entities.FooAggregate;
using ChatApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FooUowController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public FooUowController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet]
    public async Task<List<Foo>> Get()
    {
        return await _unitOfWork.Foos.GetAsync();
    }


    [HttpGet("{filter}")]
    public async Task<List<Foo>> FilterByMessage(string filter)
    {
        return await _unitOfWork.Foos.GetFilteredAsync(filter);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Foo>> Get(int id)
    {
        var foo =  await _unitOfWork.Foos.GetByIdAsync(id);
        if (foo == null)
        {
            return NotFound();
        }

        return foo;
    }
    
    [HttpPost]
    public async Task<ActionResult<List<Foo>>> Post(FooDto fooDto)
    {
        var foo = new Foo
        {
            TextMessage = fooDto.Message,
        };
        
        _unitOfWork.Foos.Add(foo);
        await _unitOfWork.CompleteAsync();
      
        return await _unitOfWork.Foos.GetAsync();
    }

    [HttpPut]
    public async Task<ActionResult<List<Foo>>> Put(FooDto fooDto, int id)
    {
        var itemToUpdate = await _unitOfWork.Foos.GetByIdAsync(id);
        if (itemToUpdate == null)
        {
            return NotFound();
        }

        itemToUpdate.TextMessage = fooDto.Message;
        
        _unitOfWork.Foos.Update(itemToUpdate);
        await _unitOfWork.CompleteAsync();

        return await _unitOfWork.Foos.GetAsync();
    }

    [HttpDelete]
    public async Task<ActionResult<List<Foo>>> DeleteItem(int id)
    {
        var itemToDelete = await _unitOfWork.Foos.GetByIdAsync(id);
        if (itemToDelete == null)
        {
            return NotFound();
        }
        
        _unitOfWork.Foos.Delete(itemToDelete);
        await _unitOfWork.CompleteAsync();

        return await _unitOfWork.Foos.GetAsync();
    }
}