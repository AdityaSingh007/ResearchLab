using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullServices.Repository
{
    public interface IExpenseTrackerRepository
    {
        RepositoryActionResult<RestFullServices.Repository.Entities.Expense> DeleteExpense(int id);
        RepositoryActionResult<RestFullServices.Repository.Entities.ExpenseGroup> DeleteExpenseGroup(int id);
        RestFullServices.Repository.Entities.Expense GetExpense(int id, int? expenseGroupId = null);
        RestFullServices.Repository.Entities.ExpenseGroup GetExpenseGroup(int id);
        RestFullServices.Repository.Entities.ExpenseGroup GetExpenseGroup(int id, string userId);
        System.Linq.IQueryable<RestFullServices.Repository.Entities.ExpenseGroup> GetExpenseGroups();
        System.Linq.IQueryable<RestFullServices.Repository.Entities.ExpenseGroup> GetExpenseGroups(string userId);
        RestFullServices.Repository.Entities.ExpenseGroupStatus GetExpenseGroupStatus(int id);
        System.Linq.IQueryable<RestFullServices.Repository.Entities.ExpenseGroupStatus> GetExpenseGroupStatusses();
        System.Linq.IQueryable<RestFullServices.Repository.Entities.ExpenseGroup> GetExpenseGroupsWithExpenses();
        RestFullServices.Repository.Entities.ExpenseGroup GetExpenseGroupWithExpenses(int id);
        RestFullServices.Repository.Entities.ExpenseGroup GetExpenseGroupWithExpenses(int id, string userId);
        System.Linq.IQueryable<RestFullServices.Repository.Entities.Expense> GetExpenses();
        System.Linq.IQueryable<RestFullServices.Repository.Entities.Expense> GetExpenses(int expenseGroupId);

        RepositoryActionResult<RestFullServices.Repository.Entities.Expense> InsertExpense(RestFullServices.Repository.Entities.Expense e);
        RepositoryActionResult<RestFullServices.Repository.Entities.ExpenseGroup> InsertExpenseGroup(RestFullServices.Repository.Entities.ExpenseGroup eg);
        RepositoryActionResult<RestFullServices.Repository.Entities.Expense> UpdateExpense(RestFullServices.Repository.Entities.Expense e);
        RepositoryActionResult<RestFullServices.Repository.Entities.ExpenseGroup> UpdateExpenseGroup(RestFullServices.Repository.Entities.ExpenseGroup eg);
    }
}
