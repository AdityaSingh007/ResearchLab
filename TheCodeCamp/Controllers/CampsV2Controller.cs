using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/camps")]
    public class CampsV2Controller : ApiController
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        public CampsV2Controller(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker, includeTalks);
                if (result == null) return NotFound();
                return Ok(new { success = true, data = _mapper.Map<CampModel>(result) });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

    }
}
