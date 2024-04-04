using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsValidInput(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            if (client == null)
            {
                return false;
            }

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            if (!SetCreditLimit(user, client))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool IsValidInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return IsValidName(firstName) && IsValidName(lastName) && IsValidEmail(email) && IsValidBirthdate(dateOfBirth);
        }

        private bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool IsValidBirthdate(DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth) >= 21;
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth,
                Client = client
            };
        }

        public bool SetCreditLimit(User user, Client client)
        {
            var userCreditService = new UserCreditService();
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
                return true;
            }

            int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            if (client.Type == "ImportantClient")
            {
                creditLimit *= 2;
            }

            user.CreditLimit = creditLimit;
            user.HasCreditLimit = true;

            return user.CreditLimit >= 500;
        }
    }
}


