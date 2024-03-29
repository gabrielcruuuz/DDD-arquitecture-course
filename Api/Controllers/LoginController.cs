﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto login, [FromServices] ILoginService service)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //codigo 400 - solicitação invalida
            if (login == null)
                return BadRequest(); //codigo 400 - solicitação invalida

            try
            {
                var result = await service.FindByLogin(login);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();

            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}