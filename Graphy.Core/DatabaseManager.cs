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
                    var createContact = "CREATE TABLE \"Contact\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"FirstName\" VARCHAR, \"MiddleName\" VARCHAR, \"LastName\" VARCHAR, \"Organization\" VARCHAR, \"ImagePath\" VARCHAR, \"Birthday\" DATETIME, \"Favourite\" BOOL DEFAULT 0)";
                    db.Execute(createContact);
                    var createPhoneNumber = "CREATE TABLE \"PhoneNumber\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Number\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(\"ContactId\") REFERENCES \"Contact\"(\"Id\") ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createPhoneNumber);
                    var createAddress = "CREATE TABLE \"Address\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"StreetLine1\" VARCHAR, \"StreetLine2\" VARCHAR, \"City\" VARCHAR, \"Province\" VARCHAR, \"PostalCode\" VARCHAR, \"Country\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createAddress);
                    var createEmail = "CREATE TABLE \"Email\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Address\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createEmail);
                    var createSpecialDate = "CREATE TABLE \"SpecialDate\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Date\" DATETIME, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createSpecialDate);
                    var createUrl = "CREATE TABLE \"Url\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL, \"Type\" VARCHAR, \"Link\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createUrl);
                    var createInstantMessage = "CREATE TABLE \"InstantMessage\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Type\" VARCHAR, \"Nickname\" VARCHAR, \"ContactId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createInstantMessage);
                    var createTag = "CREATE TABLE \"Tag\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR, \"ExtraInfo\" VARCHAR)";
                    db.Execute(createTag);
                    var createContactTagMap = "CREATE TABLE \"ContactTagMap\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL, \"ContactId\" INTEGER, \"TagId\" INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(TagId) REFERENCES Tag(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
                    db.Execute(createContactTagMap);
                    var createConnectionType = "CREATE TABLE \"ConnectionType\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"Name\" VARCHAR)";
                    db.Execute(createConnectionType);
                    var createConnection = "CREATE TABLE \"Connection\" (\"Id\" INTEGER PRIMARY KEY  NOT NULL , \"ExtraInfo\" VARCHAR, \"FromContactId\" INTEGER, \"ToContactId\" INTEGER, \"ConnectionTypeId\" INTEGER, FOREIGN KEY(FromContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ToContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ConnectionTypeId) REFERENCES ConnectionType(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
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
                contact2.Favourite = true;
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
                contact9.Organization = "Null Org.";
                contact9.Birthday = new DateTime(1999, 9, 9);
                db.Insert(contact9);

                var contact10 = new Contact();
                contact10.Id = 10;
                contact10.FirstName = "";
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
                number2.Number = "0123456789";
                number2.Type = "Office";
                db.Insert(number2);
                                    
                var number3 = new PhoneNumber();
                number3.Id = 3;
                number3.ContactId = 2;
                number3.Number = "0987654321";
                number3.Type = "Home";
                db.Insert(number3);

                // Address
                var address1 = new Address();
                address1.Id = 1;
                address1.Type = "Home";
                address1.StreetLine1 = "1 Capitol Hill";
                address1.StreetLine2 = "Apt 1";
                address1.City = "Seattle";
                address1.Province = "WA";
                address1.Country = "United States";
                address1.ContactId = 2;
                db.Insert(address1);

                var address2 = new Address();
                address2.Id = 2;
                address2.Type = "Work";
                address2.StreetLine1 = "1 Microsoft Way";
                address2.City = "Redmond";
                address2.ContactId = 2;
                db.Insert(address2);

                // Email
                var email1 = new Email();
                email1.Id = 2;
                email1.Address = "bill@microsoft.com";
                email1.Type = "Work";
                email1.ContactId = 2;
                db.Insert(email1);

                // Url
                var url1 = new Url();
                url1.Id = 1;
                url1.Type = "Blog";
                url1.Link = "billgates.com";
                url1.ContactId = 2;
                db.Insert(url1);

                var url2 = new Url();
                url2.Id = 2;
                url2.Type = "Philanthropy";
                url2.Link = "billandmelinda.org";
                url2.ContactId = 2;
                db.Insert(url2);

                // IMs
                var im1 = new InstantMessage();
                im1.Id = 1;
                im1.Type = "Skype";
                im1.Nickname = "billgates";
                im1.ContactId = 2;
                db.Insert(im1);

                // Special Dates
                var date1 = new SpecialDate();
                date1.Id = 1;
                date1.Type = "Founded Microsoft";
                date1.Date = new DateTime(1975, 4, 4);
                date1.ContactId = 2;
                db.Insert(date1);

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