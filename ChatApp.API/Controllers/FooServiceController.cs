using ChatApp.API.DTOs;
using ChatApp.Core.Entities.FooAggregate;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FooController : ControllerBase
{
    private readonly IFooService _fooService;

    public FooController(IFooService fooService)
    {
        _fooService = fooService;
    }

    [HttpGet]
    public async Task<List<Foo>> Get()
    {
        return await _fooService.GetAsync();
    }


    [HttpGet("{filter}")]
    public async Task<List<Foo>> FilterByMessage(string filter)
    {
        return await _fooService.GetFilteredAsync(filter);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Foo>> Get(int id)
    {
        var foo =  await _fooService.GetByIdAsync(id);
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
        
        return await _fooService.AddAsync(foo);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<List<Foo>>> Put(FooDto fooDto, int id)
    {
        var itemToUpdate = await _fooService.GetByIdAsync(id);
        if (itemToUpdate == null)
        {
            return NotFound();
        }
        
        itemToUpdate.TextMessage = fooDto.Message;
        
        return await _fooService.UpdateAsync(itemToUpdate);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<List<Foo>>> DeleteItem(int id)
    {
        var itemToDelete = await _fooService.GetByIdAsync(id);
        if (itemToDelete == null)
        {
            return NotFound();
        }
        return await _fooService.DeleteAsync(itemToDelete);
    }
}