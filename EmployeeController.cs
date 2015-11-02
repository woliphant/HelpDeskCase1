using System.Web.Http;
using System;
using HelpdeskViewModels;
using System.Collections.Generic;

namespace ExercisesWebsite
{
    public class EmployeeController : ApiController
    {
        // passes the employees GetAll method
        [Route("api/employees")]
        public IHttpActionResult Get()
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = emp.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        // passes the employees GetById method
        [Route("api/employees/{empId}")]
        public IHttpActionResult Get(string empId)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.EmployeeId = empId;
                emp.GetById(empId);
                return Ok(emp);
            }
            catch(Exception e)
            {
                return BadRequest("Retrieve failed - " + e.Message);
            }
        }

        // passes the employee Create Method
        [Route("api/employees/")]
        public IHttpActionResult Post(EmployeeViewModel emp)
        {
            try
            {
                emp.Create();
                return Ok(emp);
            }
            catch(Exception e)
            {
                return BadRequest("Create failed - " + e.Message);
            }
        }

        // passes the employee delete method
        [Route("api/employees/")]
        public IHttpActionResult Delete(string empId)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.GetById(empId);
                emp.Delete();
                return Ok(emp.Lastname + " has been deleted");
            }
            catch (Exception e)
            {
                return BadRequest("Delete failed - " + e.Message);
            }
        }

        // passes the employees update method
        [Route("api/employees/")]
        public IHttpActionResult Put(EmployeeViewModel emp)
        {
            try
            {
                if (emp.Update() == 1)
                {
                    return Ok("Employee " + emp.Lastname + " updated.");
                }
                else if (emp.Update() == -1)
                {
                    return Ok("Employee " + emp.Lastname + " not updated!");
                }
                else if (emp.Update() == -2)
                {
                    return Ok("Employee " + emp.Lastname + " not updated due to stale data.");
                }
                else
                {
                    return Ok("Update Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - " + ex.Message);
            }
        }
    }
}
