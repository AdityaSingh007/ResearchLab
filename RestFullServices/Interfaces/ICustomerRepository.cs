using RestFullServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullServices.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(int id);
        bool UpdateCustomer(Customer cust);
        Customer InsertCustomer(Customer cust);
        bool DeleteCustomer(int id);
        List<Order> GetOrders(int id);
    }
}
