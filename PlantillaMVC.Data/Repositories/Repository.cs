﻿using PlantillaMVC.Domain.Entities;
using PlantillaMVC.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PlantillaMVC.Data.Repositories {

    public class Repository<T> : IRepository<T> where T : IEntity {

        public void Create(T entity) {
            throw new NotImplementedException();
        }

        public void Delete(int id) {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll() {
            throw new NotImplementedException();
        }

        public T GetById(int id) {
            throw new NotImplementedException();
        }

        public void Update(T entity) {
            throw new NotImplementedException();
        }
    }
}