using System;

namespace Graphy.Core
{
    public static class DatabaseHelper
    {
        static DatabaseHelper()
        {
        }

        public static string GetFullName(Contact contact)
        {
            string firstName = contact.FirstName != null ? contact.FirstName+" " : "";
            string middleName = contact.MiddleName != null ? contact.MiddleName+" " : "";
            string lastName = contact.LastName != null ? contact.LastName : "";

            return firstName + middleName + lastName;
        }
    }
}

