using System;

namespace Graphy.Core
{
    public static class DatabaseHelper
    {
        public static string GetFullName(Contact contact)
        {
            string firstName = !string.IsNullOrEmpty(contact.FirstName) ? contact.FirstName+" " : contact.FirstName;
            string middleName = !string.IsNullOrEmpty(contact.MiddleName) ? contact.MiddleName+" " : contact.MiddleName;
            string lastName = !string.IsNullOrEmpty(contact.LastName) ? contact.LastName : contact.LastName;

            return firstName + middleName + lastName;
        }
    }
}

