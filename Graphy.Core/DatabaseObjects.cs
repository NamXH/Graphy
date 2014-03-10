using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphy.Core
{
    public interface IPrimaryKeyContainer
    {
        int Id { get; set; }
    }

    public interface IContactIdRelated
    {
        int ContactId { get; set; }
    }

    public class Contact : IPrimaryKeyContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string ImagePath { get; set; }

        public DateTime Birthday { get; set; }

        public bool Favourite { get; set; }

        public string GetFullName()
        {
            string firstName = !string.IsNullOrEmpty(FirstName) ? FirstName+" " : FirstName;
            string middleName = !string.IsNullOrEmpty(MiddleName) ? MiddleName+" " : MiddleName;
            string lastName = !string.IsNullOrEmpty(LastName) ? LastName : LastName;

            return firstName + middleName + lastName;
        }
    }

    public class PhoneNumber : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public int ContactId { get; set; }
    }

    public class Address : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string StreetLine1 { get; set; }

        public string StreetLine2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public int ContactId { get; set; }
    }

    public class Email : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int ContactId { get; set; }
    }

    public class SpecialDate : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public int ContactId { get; set; }
    }

    public class Url : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public int ContactId { get; set; }
    }

    public class InstantMessage : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Nickname { get; set; }

        public int ContactId { get; set; }
    }

    public class Organization : IPrimaryKeyContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ContactOrganizationMap : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int OrganizationId { get; set; }
    }

    public class Tag : IPrimaryKeyContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ExtraInfo { get; set; }
    }

    public class ContactTagMap : IPrimaryKeyContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int TagId { get; set; }
    }

    public class ConnectionType : IPrimaryKeyContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Connection : IPrimaryKeyContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string ExtraInfo { get; set; }

        public int FromContactId { get; set; }

        public int ToContactId { get; set; }

        public int ConnectionTypeId { get; set; }
    }
}