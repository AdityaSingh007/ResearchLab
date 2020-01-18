using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using TheCodeCamp.AuthenticationFilter;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [RoutePrefix("api/camps")]
    [EnableCors("*", "*", "*")]
    [Authorize(Roles ="Admin")]
    //[CustomAuthorizationAttribute]
    public class CampsController : ApiController
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        [Route()]
        //[AllowAnonymous]
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsAsync(includeTalks);

                //mapping
                var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

        [MapToApiVersion("1.0")]
        [Route("{moniker}", Name = "GetCamp10")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker, includeTalks);
                if (result == null) return NotFound();
                return Ok(_mapper.Map<CampModel>(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [MapToApiVersion("1.1")]
        [Route("{moniker}", Name = "GetCamp11")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker, true);
                if (result == null) return NotFound();
                return Ok(_mapper.Map<CampModel>(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("SearchByEventDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsByEventDate(eventDate, includeTalks);
                return Ok(_mapper.Map<IEnumerable<CampModel>>(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(CampModel campModel)
        {
            try
            {
                if (await _campRepository.GetCampAsync(campModel.Moniker) != null)
                {
                    ModelState.AddModelError("Moniker", "Moniker in use");
                }
                if (ModelState.IsValid)
                {
                    var camp = _mapper.Map<Camp>(campModel);
                    _campRepository.AddCamp(camp);
                    if (await _campRepository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CampModel>(camp);
                        var location = Url.Link("GetCamp10", new { moniker = newModel.Moniker });
                        return Created(location, newModel);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel campModel)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null) return NotFound();
                _mapper.Map(campModel, camp);
                if (await _campRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CampModel>(camp));
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null)
                    return NotFound();

                _campRepository.DeleteCamp(camp);

                if (await _campRepository.SaveChangesAsync())
                    return Ok();
                else
                    return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
