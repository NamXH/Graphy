using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphy.Core
{
    public class Contact
    {
        [PrimaryKey]
        public int ContactId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string ImagePath { get; set; }

        public DateTime Birthday { get; set; }
    }

    public class PhoneNumber
    {
        [PrimaryKey]
        public int PhoneNumberId { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public int ContactId { get; set; }
    }

    public class Country
    {
        [PrimaryKey]
        public int CountryId { get; set; }

        public string Name { get; set; }
    }

    public class Address
    {
        [PrimaryKey]
        public int AddressId { get; set; }

        public string Type { get; set; }

        public string StreetLine1 { get; set; }

        public string StreetLine2 { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public int ContactId { get; set; }

        public int CountryId { get; set; }
    }

    public class Email
    {
        [PrimaryKey]
        public int EmailId { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int ContactId { get; set; }
    }

    public class SpecialDate
    {
        [PrimaryKey]
        public int SpecialDateId { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public int ContactId { get; set; }
    }

    public class Url
    {
        [PrimaryKey]
        public int UrlId { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public int ContactId { get; set; }
    }

    public class InstantMessage
    {
        [PrimaryKey]
        public int InstantMessageId { get; set; }

        public string Type { get; set; }

        public string Nickname { get; set; }

        public int ContactId { get; set; }
    }

    public class Organization
    {
        [PrimaryKey]
        public int OrganizationId { get; set; }

        public string Name { get; set; }
    }

    public class ContactOrganizationMap
    {
        [PrimaryKey]
        public int ContactOrganizationMapId { get; set; }

        public int ContactId { get; set; }

        public int OrganizationId { get; set; }
    }

    public class Tag
    {
        [PrimaryKey]
        public int TagId { get; set; }

        public string Name { get; set; }

        public string ExtraInfo { get; set; }
    }

    public class ContactTagMap
    {
        [PrimaryKey]
        public int ContactTagMapId { get; set; }

        public int ContactId { get; set; }

        public int TagId { get; set; }
    }

    public class ConnectionType
    {
        [PrimaryKey]
        public int ConnectionTypeId { get; set; }

        public string Name { get; set; }
    }

    public class Connection
    {
        [PrimaryKey]
        public int ConnectionId { get; set; }

        public string ExtraInfo { get; set; }

        public int FromContactId { get; set; }

        public int ToContactId { get; set; }

        public int ConnectionTypeId { get; set; }
    }
}