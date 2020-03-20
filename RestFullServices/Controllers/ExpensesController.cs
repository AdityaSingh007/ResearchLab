using RestFullServices.Repository;
using RestFullServices.Repository.Entities;
using RestFullServices.Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestFullServices.Controllers
{
    [RoutePrefix("api")]
    public class ExpensesController : ApiController
    {
        IExpenseTrackerRepository _repository;
        ExpenseFactory _expenseFactory = new ExpenseFactory();

        public ExpensesController()
        {
            _repository = new ExpenseTrackerEFRepository(new ExpenseTrackerContext());
        }

        [Route("expenseGroups/{expenseGroupId}/expenses")]
        public IHttpActionResult Get(int expenseGroupId)
        {
            try
            {
                var expenses = _repository.GetExpenses(expenseGroupId);
                if (expenses == null)
                    return NotFound();

                var expensesResult = expenses.ToList()
                                             .Select(exp => _expenseFactory.CreateExpense(exp));
                return Ok(expensesResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenseGroups/{expenseGroupId}/expenses/{id}")]
        [Route("expenses/{id}")]
        public IHttpActionResult Get(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;
                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);
                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }

                }

                if (expense != null)
                {
                    var returnValue = _expenseFactory.CreateExpense(expense);
                    return Ok(returnValue);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
