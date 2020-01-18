using AutoMapper;
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
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                var results = await _campRepository.GetTalksByMonikerAsync(moniker, includeSpeakers);
                return Ok(_mapper.Map<IEnumerable<TalkModel>>(results));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{id:int}", Name = "GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false)
        {
            try
            {
                var result = await _campRepository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                if (result == null)
                    return NotFound();
                else
                    return Ok(_mapper.Map<TalkModel>(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = await _campRepository.GetCampAsync(moniker);
                    if (camp != null)
                    {
                        var talk = _mapper.Map<Talk>(talkModel);
                        talk.Camp = camp;

                        if (talkModel.Speaker != null)
                        {
                            var speaker = await _campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                            talk.Speaker = speaker;
                        }

                        _campRepository.AddTalk(talk);

                        if (await _campRepository.SaveChangesAsync())
                            return CreatedAtRoute("GetTalk",
                                new { moniker = moniker, id = talk.TalkId },
                                _mapper.Map<TalkModel>(talk));
                    }

                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Put(string moniker, int talkId, TalkModel talkModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talk = await _campRepository.GetTalkByMonikerAsync(moniker, talkId, includeSpeakers: true);
                    if (talk == null)
                        return NotFound();

                    _mapper.Map(talkModel, talk);

                    if (talk.Speaker.SpeakerId != talkModel.Speaker.SpeakerId)
                    {
                        var speaker = await _campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                        talk.Speaker = speaker;
                    }

                    if (await _campRepository.SaveChangesAsync())
                        return Ok(_mapper.Map<TalkModel>(talk));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Delete(string moniker, int talkId)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, talkId);
                if (talk == null)
                    return NotFound();
                _campRepository.DeleteTalk(talk);
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
