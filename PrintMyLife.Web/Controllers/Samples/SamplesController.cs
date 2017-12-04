using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintMyLife.Core.Sample;
using PrintMyLife.Core.Sample.Entities;
using Microsoft.EntityFrameworkCore;
using PrintMyLife.Web.Controllers.Samples.Dto;
using AutoMapper;
using System.Net.Http;
using PrintMyLife.Web.Controllers;
using PrintMyLife.Core.Runtime.Session;

namespace PrintMyLife.Controllers.Samples
{
  [Route("api/v1/[controller]")]
  public class SamplesController : BaseController
  {
    private SampleManager _sampleManager;

    public SamplesController(
      [FromServices]IAppSession session,
      SampleManager sampleManager
    ) : base()
    {
      _sampleManager = sampleManager;
    }

    // GET
    [HttpGet]
    [ProducesResponseType(typeof(MySampleDto), 200)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetSampleById([FromRoute]Guid id)
    {
      if (!(ModelState.IsValid))
      {
        return BadRequest(ModelState);
      }

      var result = await _sampleManager.GetSampleByIdAsync(id);
      return new ObjectResult(Mapper.Map<MySample, MySampleDto>(result));
    }

    // GET
    [HttpGet]
    [ProducesResponseType(typeof(List<MySampleDto>), 200)]
    public async Task<IActionResult> GetSamples()
    {
      var result = await _sampleManager.GetAllSamples().ToListAsync();
      return new ObjectResult(Mapper.Map<List<MySample>, List<MySampleDto>>(result));
    }

    // POST
    [HttpPost]
    [ProducesResponseType(typeof(MySampleDto), 200)]
    public async Task<IActionResult> CreateSample([FromBody]MySampleDto dto)
    {
      if (!(ModelState.IsValid))
      {
        return BadRequest(ModelState);
      }

      var mySample = new MySample(
        dto.Value
      );
      var result = await _sampleManager.CreateAsync(mySample);

      return new ObjectResult(Mapper.Map<MySample, MySampleDto>(result));
    }

    // PUT
    [HttpPut]
    [ProducesResponseType(typeof(MySampleDto), 200)]
    public async Task<IActionResult> UpdateSample([FromBody]MySampleDto dto)
    {
      if (!(ModelState.IsValid))
      {
        return BadRequest(ModelState);
      }
      var mySample = await _sampleManager.GetSampleByIdAsync(dto.Id);
      if (mySample == null)
      {
        return NotFound(dto.Id);
      }

      mySample.Update(
        dto.Value
      );

      var result = await _sampleManager.UpdateAsync(mySample);

      return new ObjectResult(Mapper.Map<MySample, MySampleDto>(result));
    }

    // DEL
    [HttpDelete]
    [ProducesResponseType(typeof(object), 200)]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteSample([FromRoute]Guid id)
    {
      var mySample = await _sampleManager.GetSampleByIdAsync(id);
      if (mySample == null)
      {
        return NotFound(id);
      }

      await _sampleManager.DeleteAsync(mySample);

      return Ok();
    }
  }
}
