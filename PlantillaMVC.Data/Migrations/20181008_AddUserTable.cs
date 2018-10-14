using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantillaMVC.Data.Migrations {

    [Migration(20181008)]
    public class AddUserTable : Migration {

        public override void Down() {
            Create.Table("User")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString()
                .WithColumn("Email").AsString()
                .WithColumn("Password").AsString()
                .WithColumn("CreatedAt").AsDateTime()
                .WithColumn("UpdatedAt").AsDateTime()
                .WithColumn("RemovedAt").AsDateTime();
        }

        public override void Up() {
            Delete.Table("User");
        }
    }
}