﻿using Microsoft.Web.Http;
using PlantillaMVC.Domain.Entities;
using PlantillaMVC.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlantillaMVC.API.Controllers.V1 {

    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/users")]
    public class UsersController : ApiController {
        private IUserService _userService;

        public UsersController(IUserService userService) {
            _userService = userService;
        }

        // GET: api/Users
        [Route]
        public IEnumerable<User> Get() {
            return _userService.GetAll().ToList();
        }

        // GET: api/Users/5
        [HttpGet]
        [Route("{id}")]
        public User Get(int id) {
            throw new NotImplementedException();
        }

        // POST: api/Users
        [HttpPost]
        [Route]
        public void Post([FromBody]string value) {
            throw new NotImplementedException();
        }

        // PUT: api/Users/5
        [HttpPut]
        [Route("{id}")]
        public void Put(int id, [FromBody]string value) {
            throw new NotImplementedException();
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id) {
            throw new NotImplementedException();
        }
    }
}