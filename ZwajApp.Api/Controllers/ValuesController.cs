using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZwajApp.Api.Data;

namespace ZwajApp.Api.Controllers
{
 [Authorize]
 [Route("api/[controller]")]
 [ApiController]
 public class ValuesController : ControllerBase
 {
  public DataContext _Context;
  public ValuesController(DataContext context)
  {
   _Context = context;

  }

  // GET api/values
  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetValues()
  {
     var values  = await _Context.Values.ToListAsync();
   return Ok(values);
  }
  [AllowAnonymous]
  // GET api/values/5
  [HttpGet("{id}")]
  public async Task<IActionResult> GetValue(int id)
  {
   var value = await _Context.Values.FirstOrDefaultAsync(x => x.Id == id);
   return Ok(value);
  }

  // POST api/values
  [HttpPost]
  public void Post([FromBody] string value)
  {
  }

  // PUT api/values/5
  [HttpPut("{id}")]
  public void Put(int id, [FromBody] string value)
  {
  }

  // DELETE api/values/5
  [HttpDelete("{id}")]
  public void Delete(int id)
  {
  }
 }
}
