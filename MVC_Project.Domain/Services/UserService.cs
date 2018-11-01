using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Project.Domain.Services {

    public interface IUserService {

        IEnumerable<User> GetAll();

        User GetById(int id);

        void Create(User user);

        void Update(User user);

        void Delete(int id);
    }

    public class UserService : IUserService {
        private IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository) {
            _userRepository = userRepository;
        }

        public void Create(User user) {
            _userRepository.Create(user);
        }

        public void Delete(int id) {
            _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll() {
            return _userRepository.GetAll().ToList();
        }

        public User GetById(int id) {
            return _userRepository.GetById(id);
        }

        public void Update(User user) {
            _userRepository.Update(user);
        }
    }
}