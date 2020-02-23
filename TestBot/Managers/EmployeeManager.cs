using MebelTelegramBot.Models;
using MebelTelegramBot.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MebelTelegramBot.Managers {
    public class EmployeeManager {
        private EmployeeXmlManager xmlManager;

        public EmployeeManager() {
            xmlManager = new EmployeeXmlManager();
            if (!MebelRuBotContext.Employees.Any()) {
                xmlManager.GetEmployees().ForEach(e => MebelRuBotContext.Employees.Add(new Manager() { Id = e.Id, Name = e.Name }));
            }
        }

        public void Add(Employee user) {
            MebelRuBotContext.Employees.Add(user);
            xmlManager.Add(user.Name);
        }

        public List<Employee> Get() {
            return MebelRuBotContext.Employees;
        }

        public Employee Get(long id) {
            return MebelRuBotContext.Employees.FirstOrDefault(e => e.Id == id);
        }

        public bool Bind(long id, string name) {
            var success = xmlManager.Bind(id, name);
            var userToBind = MebelRuBotContext.Employees.FirstOrDefault(e => e.Name == name);
            if (success && userToBind != null) {
                userToBind.Id = id;
            }

            return success;
        }

        public void Remove(string name) {
            xmlManager.Remove(name);
            var userToDelete = MebelRuBotContext.Employees.FirstOrDefault(e => e.Name == name);
            if (userToDelete != null) {
                MebelRuBotContext.Employees.Remove(userToDelete);
            }
        }
    }
}
