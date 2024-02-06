using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class TestController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public TestController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;

        }

        [HttpGet]
        public async Task<ActionResult<TestEntityDto>> Get()
        {
            return Ok(await _uow.TestEntityRepository.GetTestEntityDtosAsync());
        }
    }
}