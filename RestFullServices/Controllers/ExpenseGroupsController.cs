using Marvin.JsonPatch;
using RestFullServices.Repository;
using RestFullServices.Repository.Entities;
using RestFullServices.Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestFullServices.Helpers;
using System.Linq.Dynamic;
using System.Web.Http.Routing;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace RestFullServices.Controllers
{
    [EnableCors("*","*","GET,POST")]
    [Authorize]
    public class ExpenseGroupsController : ApiController
    {
        IExpenseTrackerRepository _expenseTrackerRepository;
        ExpenseGroupFactory _expenseGroupFactory = new ExpenseGroupFactory();

        public ExpenseGroupsController()
        {
            _expenseTrackerRepository = new ExpenseTrackerEFRepository(
                new ExpenseTrackerContext());
        }

        [HttpGet]
        [Route("api/ExpenseGroups", Name = "ExpenseGroupList")]
        public IHttpActionResult Get(string sort = "id", string status = null, string userId = null, string fields = null, int page = 1, int pageSize = 5)
        {
            try
            {
                bool includeExpenses = false;
                var listOfFields = new List<string>();
                if (!string.IsNullOrEmpty(fields))
                    listOfFields = fields.ToLower().Split(',').ToList();

                includeExpenses = listOfFields.Any(f => f.Contains("expenses"));

                int statusId = -1;
                if (status != null)
                {
                    switch (status.ToLower())
                    {
                        case "open":
                            statusId = 1;
                            break;
                        case "confirmed":
                            statusId = 2;
                            break;
                        case "processed":
                            statusId = 3;
                            break;
                        default:
                            break;
                    }
                }

                IQueryable<ExpenseGroup> expenseGroups = null;
                if (includeExpenses)
                    expenseGroups = _expenseTrackerRepository.GetExpenseGroupsWithExpenses();
                else
                    expenseGroups = _expenseTrackerRepository.GetExpenseGroups();

                expenseGroups = expenseGroups.ApplySort(sort)
                                             .Where(eg => (statusId == -1 || eg.ExpenseGroupStatusId == statusId))
                                             .Where(eg => (userId == null || eg.UserId == userId));


                var totalCount = expenseGroups.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("ExpenseGroupList", new
                {
                    page = page - 1,
                    pageSize = pageSize,
                    sort = sort,
                    status = status,
                    userId = userId,
                    fields=fields
                }) : "";

                var nextLink = page > 1 ? urlHelper.Link("ExpenseGroupList", new
                {
                    page = page + 1,
                    pageSize = pageSize,
                    sort = sort,
                    status = status,
                    userId = userId,
                    fields = fields
                }) : "";

                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

                return Ok(expenseGroups.Skip(pageSize * (page - 1))
                                       .Take(pageSize)
                                       .ToList()
                                       .Select(eg => _expenseGroupFactory.CreateDataShapedObject(eg, listOfFields)));
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                var expenseGroup = _expenseTrackerRepository.GetExpenseGroup(id);
                if (expenseGroup == null)
                    return NotFound();
                else
                    return Ok(_expenseGroupFactory.CreateExpenseGroup(expenseGroup));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                    return BadRequest();
                else
                {
                    var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);
                    var result = _expenseTrackerRepository.InsertExpenseGroup(eg);
                    if (result.Status == RepositoryActionStatus.Created)
                    {
                        var newExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);
                        return Created(Request.RequestUri + "/" + newExpenseGroup.Id.ToString(),
                            newExpenseGroup);
                    }
                    else
                        return BadRequest();

                }
            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }

        public IHttpActionResult Put([FromBody] DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                    return BadRequest();
                var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);
                var result = _expenseTrackerRepository.UpdateExpenseGroup(eg);
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var updatedExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);
                    return Ok(updatedExpenseGroup);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                    return NotFound();
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody] JsonPatchDocument<DTO.ExpenseGroup> jsonPatchDocument)
        {
            try
            {
                if (jsonPatchDocument == null)
                    return BadRequest();
                var expenseGroup = _expenseTrackerRepository.GetExpenseGroup(id);
                if (expenseGroup == null)
                    return NotFound();
                var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);
                jsonPatchDocument.ApplyTo(eg);
                var result = _expenseTrackerRepository.UpdateExpenseGroup(_expenseGroupFactory.CreateExpenseGroup(eg));
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var patchedExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);
                    return Ok(patchedExpenseGroup);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var result = _expenseTrackerRepository.DeleteExpenseGroup(id);
                if (result.Status == RepositoryActionStatus.Deleted)
                    return StatusCode(HttpStatusCode.NoContent);
                else if (result.Status == RepositoryActionStatus.NotFound)
                    return NotFound();
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
