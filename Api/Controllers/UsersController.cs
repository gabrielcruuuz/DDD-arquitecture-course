using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Api.Domain.Dtos;
using Api.Domain.Dtos.User;

namespace Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida

            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}", Name = "GetWithId")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida

            try
            {
                var user = await _service.Get(id);

                if (user != null)
                    return Ok(await _service.Get(id));
                else
                    return StatusCode((int)HttpStatusCode.NotFound, "Usuario não encontrado");

            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDtoCreate user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida

            try
            {
                var result = await _service.Post(user);
                if (result != null)
                {
                    Uri redirectLink = new Uri(Url.Link("GetWithId", new { id = result.Id }));
                    return Created(redirectLink, result);
                }
                else
                    return BadRequest();

            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserDtoUpdate user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida

            try
            {
                var result = await _service.Put(user);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest();

            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida

            try
            {
                bool result = await _service.Delete(id);
                if (result != false)
                    return Ok(result);
                else
                    return BadRequest();
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
