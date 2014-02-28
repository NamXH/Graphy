﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Graphy.Core
{
    public static class DatabaseManager
    {
        private const string c_databaseName = "graphy.db";
        private static string m_dbPath;

        static DatabaseManager()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            m_dbPath = Path.Combine(folderPath, c_databaseName);

            // ## Delete if exist (For test)
            if (File.Exists(m_dbPath))
            {
                File.Delete(m_dbPath);
            }

            //Create DB if not exists
            if (!File.Exists(m_dbPath))
            {
                using (var db = new SQLite.SQLiteConnection(m_dbPath))
                {
                    // Turn on Foreign Key support
                    var foreignKeyOn = "PRAGMA foreign_keys = ON";
                    db.Execute(foreignKeyOn);

                    // Create tables using SQL commands
                    var createContact = "CREATE TABLE \"Contact\" (\"ContactId\" INTEGER PRIMARY KEY  NOT NULL , \"FirstName\" VARCHAR, \"MiddleName\" VARCHAR, \"LastName\" VARCHAR, \"Organization\" VARCHAR, \"ImagePath\" VARCHAR, \"Birthday\" DATETIME)";
                    db.Execute(createContact);
                    var createPhoneNumber = "CREATE TABLE \"PhoneNumber\" (\"PhoneNumberId\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Number\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(\"ContactId\") REFERENCES \"Contact\"(\"ContactId\") ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createPhoneNumber);
                    var createCountry = "CREATE TABLE \"Country\" (\"CountryId\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createCountry);
                    var createAddress = "CREATE TABLE \"Address\" (\"AddressId\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"StreetLine1\" VARCHAR, \"StreetLine2\" VARCHAR, \"City\" VARCHAR, \"Province\" VARCHAR, \"PostalCode\" VARCHAR, \"ContactId\" INTEGER, \"CountryId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(CountryId) REFERENCES Country(CountryId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createAddress);
                    var createEmail = "CREATE TABLE \"Email\" (\"EmailId\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Address\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createEmail);
                    var createSpecialDate = "CREATE TABLE \"SpecialDate\" (\"SpecialDateId\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Date\" DATETIME, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createSpecialDate);
                    var createUrl = "CREATE TABLE \"Url\" (\"UrlId\" INTEGER PRIMARY KEY  NOT NULL  DEFAULT (null) ,\"Type\" VARCHAR,\"Address\" VARCHAR,\"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createUrl);
                    var createInstantMessage = "CREATE TABLE \"InstantMessage\" (\"InstantMessageId\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Nickname\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createInstantMessage);
                    var createOrganization = "CREATE TABLE \"Organization\" (\"OrganizationId\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createOrganization);
                    var createContactOrganizationMap = "CREATE TABLE \"ContactOrganizationMap\" (\"ContactOrganizationMapId\" INTEGER PRIMARY KEY  NOT NULL , \"ContactId\" INTEGER NOT NULL , \"OrganizationId\" INTEGER NOT NULL, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(OrganizationId) REFERENCES Organization(OrganizationId) ON DELETE CASCADE ON UPDATE CASCADE )";
                    db.Execute(createContactOrganizationMap);
                    var createTag = "CREATE TABLE \"Tag\" (\"TagId\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR, \"ExtraInfo\" VARCHAR)";
                    db.Execute(createTag);
                    var createContactTagMap = "CREATE TABLE \"ContactTagMap\" (\"ContactTagMapId\" INTEGER PRIMARY KEY  NOT NULL, \"ContactId\" INTEGER NOT NULL, \"TagId\" INTEGER NOT NULL, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(TagId) REFERENCES Tag(TagId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createContactTagMap);
                    var createConnectionType = "CREATE TABLE \"ConnectionType\" (\"ConnectionTypeId\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createConnectionType);
                    var createConnection = "CREATE TABLE \"Connection\" (\"ConnectionId\" INTEGER PRIMARY KEY  NOT NULL , \"ExtraInfo\" VARCHAR, \"FromContactId\" INTEGER NOT NULL , \"ToContactId\" INTEGER, \"ConnectionTypeId\" INTEGER, FOREIGN KEY(FromContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ToContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ConnectionTypeId) REFERENCES ConnectionType(ConnectionTypeId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createConnection);
                }

                // ## For tests
                CreateDummyData(m_dbPath);
            }
        }

        // ## For tests
        public static void CreateDummyData(string dbPath)
        {
            using (var db = new SQLite.SQLiteConnection(dbPath))
            {
                Debug.WriteLine("start add data");

                // Contacts
                var contact1 = new Contact();
                contact1.ContactId = 1;
                contact1.FirstName = "Andy";
                contact1.LastName = "Rubin";
                db.Insert(contact1);
                
                var contact2 = new Contact();
                contact2.ContactId = 2;
                contact2.FirstName = "Bill";
                contact2.MiddleName = "Henry";
                contact2.LastName = "Gates";
                contact2.Organization = "Microsoft";
                contact2.Birthday = new DateTime(1955, 11, 28);
                db.Insert(contact2);

                // Phone numbers
                var number1 = new PhoneNumber();
                number1.PhoneNumberId = 1;
                number1.ContactId = 1;
                number1.Number = "111";
                number1.Type = "Office";
                db.Insert(number1);
                                    
                var number2 = new PhoneNumber();
                number2.PhoneNumberId = 2;
                number2.ContactId = 2;
                number2.Number = "222";
                number2.Type = "Office";
                db.Insert(number2);
                                    
                var number3 = new PhoneNumber();
                number3.PhoneNumberId = 3;
                number3.ContactId = 2;
                number3.Number = "333";
                number3.Type = "Home";
                db.Insert(number3);

                Debug.WriteLine("stop add data");
            }
        }

        public static void DoNothing()
        {
        
    }
}
