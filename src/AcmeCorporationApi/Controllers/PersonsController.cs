using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces.Services;
using AcmeCorporationApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeCorporationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly ILogger<PersonsController> _logger;
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;

        public PersonsController(ILogger<PersonsController> logger, IPersonService personService, IMapper mapper)
        {
            _logger = logger;
            _personService = personService;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                await _personService.DeletePerson(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<PersonModel>> Get()
        {
            var persons = await _personService.GetAll();
            return _mapper.Map<IEnumerable<Person>, IEnumerable<PersonModel>>(persons);
        }

        [HttpGet("{id:int}")]
        public async Task<PersonModel> Get(int id)
        {
            var person = await _personService.GetPersonById(id);
            return _mapper.Map<Person, PersonModel>(person);
        }

        [HttpPost]
        public async Task<ActionResult<PersonModel>> Post(PersonSaveModel person)
        {
            try
            {
                var newPerson = await _personService.CreatePerson(_mapper.Map<PersonSaveModel, Person>(person));
                return _mapper.Map<Person, PersonModel>(newPerson);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}