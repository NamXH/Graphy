using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphy.Core
{
    public interface IContactIdRelated
    {
        int ContactId { get; set; }
    }

    public class Contact
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string ImagePath { get; set; }

        public DateTime Birthday { get; set; }
    }

    public class PhoneNumber : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public int ContactId { get; set; }
    }

    public class Country
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Address : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string StreetLine1 { get; set; }

        public string StreetLine2 { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public int ContactId { get; set; }

        public int CountryId { get; set; }
    }

    public class Email : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int ContactId { get; set; }
    }

    public class SpecialDate : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public int ContactId { get; set; }
    }

    public class Url : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public int ContactId { get; set; }
    }

    public class InstantMessage : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Nickname { get; set; }

        public int ContactId { get; set; }
    }

    public class Organization
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ContactOrganizationMap : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int OrganizationId { get; set; }
    }

    public class Tag
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ExtraInfo { get; set; }
    }

    public class ContactTagMap : IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int TagId { get; set; }
    }

    public class ConnectionType
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Connection
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string ExtraInfo { get; set; }

        public int FromContactId { get; set; }

        public int ToContactId { get; set; }

        public int ConnectionTypeId { get; set; }
    }
}