// --------------------------------------------------------------------------
// <copyright file="LogExecSp.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2014.2.25">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class LogExecSp
    {
        public int Id { get; set; }

        public DateTime LogDate { get; set; }

        public string Name { get; set; }

        public string Modifier { get; set; }

        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public string OldData { get; set; }

        public string Data { get; set; }

        public int ActionTypeId { get; set; }

        public Database ToDatabase(string xmlValue)
        {
            var database = new Database();

            if (!string.IsNullOrWhiteSpace(xmlValue))
            {
                var root = XElement.Parse(xmlValue);

                database.Id = Convert.ToInt32(root.Element("i").Value);
                database.Name = root.Element("n").Value;
                database.Description = root.Element("d").Value;
                database.Server = new Server
                {
                    Id = Convert.ToInt32(root.Element("si").Value),
                    Name = root.Element("sn").Value
                };
                database.UserID = root.Element("ul").Value;
                database.Password = root.Element("p").Value;
            }

            return database;
        }

        public Server ToServer(string xmlValue)
        {
            var server = new Server();

            if (!string.IsNullOrWhiteSpace(xmlValue))
            {
                var root = XElement.Parse(xmlValue);
                server.Id = Convert.ToInt32(root.Element("i").Value);
                server.Name = root.Element("n").Value;
                server.Description = root.Element("d").Value;
                server.IPLocal = root.Element("il").Value;
                server.IPUAT = root.Element("iu").Value;
                server.IPProduction = root.Element("ip").Value;
            }

            return server;
        }

        public StoreProcedure ToStoreProcedure(string xmlValue)
        {
            var storeProcedure = new StoreProcedure();

            if (!string.IsNullOrWhiteSpace(xmlValue))
            {
                var root = XElement.Parse(xmlValue);
                storeProcedure.Id = Convert.ToInt32(root.Element("i").Value);
                storeProcedure.Name = root.Element("n").Value;
                storeProcedure.Server = new Server
                {
                    Id = Convert.ToInt32(root.Element("si").Value),
                    Name = root.Element("sn").Value
                };
                storeProcedure.Database = new Database
                {
                    Id = Convert.ToInt32(root.Element("di").Value),
                    Name = root.Element("dn").Value
                };
                storeProcedure.IsFunction = root.Element("if").Value == "1";
                storeProcedure.ActionName = root.Element("a").Value;
                storeProcedure.Status = Convert.ToInt32(root.Element("s").Value);
                storeProcedure.Description = root.Element("d").Value;
                storeProcedure.Issue = new Issue
                {
                    Id = Convert.ToInt32(root.Element("ii").Value)
                };
                storeProcedure.Developer = new User
                {
                    Username = root.Element("dev").Value
                };
            }

            return storeProcedure;
        }
    }
}
