using System;
using System.Collections.Generic;
using System.Text;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Domain.Services {

    public class TestUserService : IUserService {

        public void Create(User user) {
            throw new NotImplementedException();
        }

        public void Delete(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll() {
            return new List<User>() {
                new User {
                    Id = 1,
                    Name = "Demo User",
                    Email = "mail@mail.com",
                    Password = "ASMU@I9127DX802",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    RemovedAt = null
                }
            };
        }

        public User GetById(int id) {
            throw new NotImplementedException();
        }

        public void Update(User user) {
            throw new NotImplementedException();
        }
    }
}