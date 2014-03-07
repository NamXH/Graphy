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
        private const string DatabaseName = "graphy.db";
        private static string _dbPath;

        static DatabaseManager()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _dbPath = Path.Combine(folderPath, DatabaseName);

            // ## Delete if exist (For test)
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }

            //Create DB if not exists
            if (!File.Exists(_dbPath))
            {
                using (var db = new SQLite.SQLiteConnection(_dbPath))
                {
                    // Turn on Foreign Key support
                    var foreignKeyOn = "PRAGMA foreign_keys = ON";
                    db.Execute(foreignKeyOn);

                    // Create tables using SQL commands
                    var createContact = "CREATE TABLE \"Contact\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"FirstName\" VARCHAR, \"MiddleName\" VARCHAR, \"LastName\" VARCHAR, \"Organization\" VARCHAR, \"ImagePath\" VARCHAR, \"Birthday\" DATETIME)";
                    db.Execute(createContact);
                    var createPhoneNumber = "CREATE TABLE \"PhoneNumber\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Number\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(\"ContactId\") REFERENCES \"Contact\"(\"ContactId\") ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createPhoneNumber);
                    var createCountry = "CREATE TABLE \"Country\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createCountry);
                    var createAddress = "CREATE TABLE \"Address\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"StreetLine1\" VARCHAR, \"StreetLine2\" VARCHAR, \"City\" VARCHAR, \"Province\" VARCHAR, \"PostalCode\" VARCHAR, \"ContactId\" INTEGER, \"CountryId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(CountryId) REFERENCES Country(CountryId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createAddress);
                    var createEmail = "CREATE TABLE \"Email\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Address\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createEmail);
                    var createSpecialDate = "CREATE TABLE \"SpecialDate\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Date\" DATETIME, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createSpecialDate);
                    var createUrl = "CREATE TABLE \"Url\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL  DEFAULT (null) ,\"Type\" VARCHAR,\"Address\" VARCHAR,\"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createUrl);
                    var createInstantMessage = "CREATE TABLE \"InstantMessage\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Nickname\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createInstantMessage);
                    var createOrganization = "CREATE TABLE \"Organization\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createOrganization);
                    var createContactOrganizationMap = "CREATE TABLE \"ContactOrganizationMap\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"ContactId\" INTEGER NOT NULL , \"OrganizationId\" INTEGER NOT NULL, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(OrganizationId) REFERENCES Organization(OrganizationId) ON DELETE CASCADE ON UPDATE CASCADE )";
                    db.Execute(createContactOrganizationMap);
                    var createTag = "CREATE TABLE \"Tag\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR, \"ExtraInfo\" VARCHAR)";
                    db.Execute(createTag);
                    var createContactTagMap = "CREATE TABLE \"ContactTagMap\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL, \"ContactId\" INTEGER NOT NULL, \"TagId\" INTEGER NOT NULL, FOREIGN KEY(ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(TagId) REFERENCES Tag(TagId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createContactTagMap);
                    var createConnectionType = "CREATE TABLE \"ConnectionType\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createConnectionType);
                    var createConnection = "CREATE TABLE \"Connection\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"ExtraInfo\" VARCHAR, \"FromContactId\" INTEGER NOT NULL , \"ToContactId\" INTEGER, \"ConnectionTypeId\" INTEGER, FOREIGN KEY(FromContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ToContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ConnectionTypeId) REFERENCES ConnectionType(ConnectionTypeId) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createConnection);
                }

                // ## For tests
                CreateDummyData(_dbPath);
            }
        }
        // ## For tests
        public static void CreateDummyData(string dbPath)
        {
            using (var db = new SQLite.SQLiteConnection(dbPath))
            {
                Debug.WriteLine("start adding data");

                // Contacts
                var contact1 = new Contact();
                contact1.Id = 1;
                contact1.FirstName = "Andy";
                contact1.LastName = "Rubin";
                db.Insert(contact1);
                
                var contact2 = new Contact();
                contact2.Id = 2;
                contact2.FirstName = "Bill";
                contact2.MiddleName = "Henry";
                contact2.LastName = "Gates";
                contact2.Organization = "Microsoft";
                contact2.Birthday = new DateTime(1955, 11, 28);
                contact2.ImagePath = "BillGates.JPG";
                db.Insert(contact2);

                var contact3 = new Contact();
                contact3.Id = 3;
                contact3.FirstName = "ben";
                contact3.LastName = "Afflect";
                db.Insert(contact3);

                var contact4 = new Contact();
                contact4.Id = 4;
                contact4.FirstName = "Howard";
                contact4.Birthday = new DateTime(1911, 11, 11);
                db.Insert(contact4);

                var contact5 = new Contact();
                contact5.Id = 5;
                contact5.FirstName = "Zelda";
                db.Insert(contact5);

                var contact6 = new Contact();
                contact6.Id = 6;
                contact6.FirstName = "Young";
                db.Insert(contact6);

                var contact7 = new Contact();
                contact7.Id = 7;
                contact7.FirstName = "Maple";
                db.Insert(contact7);

                var contact8 = new Contact();
                contact8.Id = 8;
                contact8.FirstName = "George";
                db.Insert(contact8);

                var contact9 = new Contact();
                contact9.Id = 9;
                contact9.FirstName = "9";
                db.Insert(contact9);

                var contact10 = new Contact();
                contact10.Id = 10;
                contact10.FirstName = "101010";
                db.Insert(contact10);

                // Phone numbers
                var number1 = new PhoneNumber();
                number1.Id = 1;
                number1.ContactId = 1;
                number1.Number = "111";
                number1.Type = "Office";
                db.Insert(number1);
                                    
                var number2 = new PhoneNumber();
                number2.Id = 2;
                number2.ContactId = 2;
                number2.Number = "222";
                number2.Type = "Office";
                db.Insert(number2);
                                    
                var number3 = new PhoneNumber();
                number3.Id = 3;
                number3.ContactId = 2;
                number3.Number = "333";
                number3.Type = "Home";
                db.Insert(number3);

                // Email
                var email1 = new Email();
                email1.Id = 2;
                email1.Address = "bill@microsoft.com";
                email1.ContactId = 2;
                db.Insert(email1);

                Debug.WriteLine("stop adding data");
            }
        }

        public static IList<T> GetRows<T>() where T : new()
        {
            using (var db = new SQLite.SQLiteConnection(_dbPath))
            {
                return db.Table<T>().ToList();
            }
        }

        public static T GetRows<T>(int primaryKey) where T : IPrimaryKeyContainer, new()
        {
            using (var db = new SQLite.SQLiteConnection(_dbPath))
            {
                return db.Table<T>().Where(v => v.Id == primaryKey).FirstOrDefault();
            }
        }

        public static IList<T> GetRowsRelatedToContact<T>(int contactId) where T : IContactIdRelated, new()
        {
            using (var db = new SQLite.SQLiteConnection(_dbPath))
            {
                return db.Table<T>().Where(v => v.ContactId == contactId).ToList();
            }
        }
    }
}