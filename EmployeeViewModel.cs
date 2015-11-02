using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;
using MongoDB.Bson;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private EmployeeDAO _dao;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Entity64 { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        public void GetById(string id)
        {
            try
            {
                Employee emp = _dao.GetById(id);
                id = emp._id.ToString();
                Title = emp.Title;
                Firstname = emp.Firstname;
                Lastname = emp.Lastname;
                Phoneno = emp.Phoneno;
                Email = emp.Email;
                DepartmentId = emp.DepartmentId.ToString();
                Entity64 = Convert.ToBase64String(ViewModelUtils.Serializer(emp));
            }
            catch (Exception e)
            {
                ViewModelUtils.ErrorRoutine(e, "EmployeeViewModel", "GetById");
            }
        }

        public int Update()
        {
            int rowsUpdated = -1;
            try
            {
                byte[] BytEmp = Convert.FromBase64String(Entity64);
                Employee emp = (Employee)ViewModelUtils.Deserializer(BytEmp);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = new ObjectId(DepartmentId);
                rowsUpdated = _dao.Update(emp);
            }
            catch (Exception e)
            {
                ViewModelUtils.ErrorRoutine(e, "EmployeeViewModel", "Update");
            }
            return rowsUpdated;
        }

        public void Create()
        {
            try
            {
                Employee emp = new Employee();
                emp.DepartmentId = new ObjectId(DepartmentId);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                EmployeeId = _dao.Create(emp);
            }
            catch (Exception e)
            {
                ViewModelUtils.ErrorRoutine(e, "EmployeeViewModel", "Create");
            }
        }

        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> viewModels = new List<EmployeeViewModel>();
            try
            {
                List<Employee> employees = _dao.GetAll();
                foreach (Employee e in employees)
                {
                    // return only fields for display, subsequent get will other fields
                    EmployeeViewModel viewModel = new EmployeeViewModel();
                    viewModel.EmployeeId = e._id.ToString();
                    viewModel.Title = e.Title;
                    viewModel.Firstname = e.Firstname;
                    viewModel.Lastname = e.Lastname;
                    viewModels.Add(viewModel); // add to list
                }
            }
            catch (Exception e)
            {
                ViewModelUtils.ErrorRoutine(e, "EmployeeViewModel", "GetAll");
            }
            return viewModels;
        }

        public bool Delete()
        {
            bool deleteOk = false;
            try
            {
                deleteOk = _dao.Delete(EmployeeId);
            }
            catch (Exception e)
            {
                ViewModelUtils.ErrorRoutine(e, "EmployeeViewModel", "Delete");
            }
            return deleteOk;
        }
    }
}
