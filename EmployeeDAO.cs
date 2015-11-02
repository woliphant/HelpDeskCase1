using System;
using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.Kennedy;
using MongoDB.Bson;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        //  Method Name: Create
        //      Purpose: Add an Employee document to the employees collection
        //      Accepts: An employee object
        //      Returns: A string
        public string Create(Employee emp)
        {
            string newid = "";
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save(emp, "employees");
                newid = emp._id.ToString();
            }
            catch(Exception e)
            {
                DALUtils.ErrorRoutine(e, "EmployeeDAO", "Create");
            }
            return newid;
        }

        //  Method Name: GetById
        //      Purpose: Retrieve an employee by Employee._id
        //      Accepts: An ObjectId
        //      Returns: An Employee Object
        public Employee GetById(string id)
        {
            Employee retEmp = null;
            DbContext _ctx;
            ObjectId _id = new ObjectId(id);
            try
            {
                _ctx = new DbContext();
                var employees = _ctx.Employees;
                var employee = employees.AsQueryable<Employee>().Where(emp => emp._id == _id).FirstOrDefault();
                retEmp = (Employee)employee;
            }
            catch(Exception e)
            {
                DALUtils.ErrorRoutine(e, "EmployeeDAO", "GetById");
            }
            return retEmp;
        }

        //  Method Name: GetAll
        //      Purpose: Retrieves a straight list of Employee Objects
        //      Accepts: Nothing
        //      Returns: List of Employees
        public List<Employee> GetAll()
        {
            List<Employee> allEmps = new List<Employee>();
            try
            {
                DbContext ctx = new DbContext();
                allEmps = ctx.Employees.ToList();
            }
            catch(Exception e)
            {
                DALUtils.ErrorRoutine(e, "EmployeDAO", "GetAll");

            }
            return allEmps;
        }

        //  Method Name: Update
        //      Purpose: Updates an Employee in the MongoDatabase, and checks for concurrency
        //      Accepts: An Employee Object
        //      Returns: An int
        public int Update(Employee emp)
        {
            int updateOk = -1;
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save<Employee>(emp, "employees");
                updateOk = 1;
            }
            catch(MongoConcurrencyException e)
            {
                updateOk = -2;
                // DALUtils.ErrorRoutine(e, "EmployeeDAO", "Update");
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                DALUtils.ErrorRoutine(e, "EmployeeDAO", "Update");
            }
            return updateOk;
        }

        //  Method Name: Delete
        //      Purpose: Deletes an employee object from the database
        //      Accepts: a String
        //      Returns: bool
        public bool Delete(string id)
        {
            bool deleteOk = false;
            ObjectId empid = new ObjectId(id);
            try
            {
                DbContext ctx = new DbContext();
                Employee emp = ctx.Employees.FirstOrDefault(e => e._id == empid);
                ctx.Delete<Employee>(emp, "employees");
                deleteOk = true;
            }
            catch(Exception e)
            {
                DALUtils.ErrorRoutine(e, "EmployeeDAO", "Delete");
            }
            return deleteOk;
        }
    }
}
