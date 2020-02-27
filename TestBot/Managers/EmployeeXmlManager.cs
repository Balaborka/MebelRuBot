using MebelTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MebelTelegramBot {
    public class EmployeeXmlManager {
        string filePath = ".\\Data\\Employees.xml";

        public bool Add(long id, string name) {
            var employees = GetEmployees();
            Employee employee = new Employee() { Id = id, Name = name };
            var empl = ContainsUser(employees, employee);
            if (empl != null)
                return false;

            employees.Add(employee);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>), new XmlRootAttribute("employees"));

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate)) {
                serializer.Serialize(fileStream, employees);
            }
            return true;
        }

        public bool Remove(string name) {
            var employees = GetEmployees();
            Employee employee = new Employee() { Name = name };
            var empl = ContainsUser(employees, employee);
            if (empl == null)
                return false;

            employees.Remove(empl);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>), new XmlRootAttribute("employees"));

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate | FileMode.Truncate)) {
                serializer.Serialize(fileStream, employees);
            }
            return true;
        }

        public List<Employee> GetEmployees() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>), new XmlRootAttribute("employees"));

            using (var reader = new StreamReader(filePath)) {
                var result = (List<Employee>)serializer.Deserialize(reader);
                return result;
            }
        }

        public Employee ContainsUser(List<Employee> employees, Employee employee) {
            foreach (var empl in employees) {
                if (empl.Name == employee.Name)
                    return empl;
            }
            return null;
        }

        public bool Bind(long id, string name) {
            var employees = GetEmployees();
            var employee = employees.FirstOrDefault(e => e.Name == name);
            if (employee != null && employee.Id == 0) {
                employee.Id = id;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>), new XmlRootAttribute("employees"));

                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate)) {
                    serializer.Serialize(fileStream, employees);
                }
                return true;
            }
            return false;
        }
    }
}
