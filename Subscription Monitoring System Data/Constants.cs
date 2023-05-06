using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data
{
    public static class Constants
    {

        public class BaseConstants
        {
            public const string retrievedData = "Retrieved Data";
            public const string errorList = "Error List";
            public const int PageSize = 2;
        }
        public class SortDirectionConstants
        {
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }

        public class ClientConstants
        {
            public const string HeaderId = "Id";
            public const string HeaderName = "Name";
            public const string HeaderEmailAddress = "EmailAddress";

            public const string EntryInvalid = "Client entry is not valid.";
            public const string Exists = "Client already exists";
            public const string DoesNotExist = "Client does not exist.";

            public const string SuccessAdd = "Client added successfully.";
            public const string SuccessEdit = "Client updated successfully.";
            public const string SuccessDelete = "Client deleted successfully.";
            public const string SuccessRestore = "Client restored successfully.";
        }

        public class SubscriptionTypeConstants
        {
            public const string DAILY = "DAILY";
            public const string WEEKLY = "WEEKLY";
            public const string MONTHLY = "MONTHLY";
            public const string YEARLY = "YEARLY";
        }

        public class NotificationConstants
        {
            public const string DoesNotExist = "Notification does not exist.";
            public const string SuccessDelete = "Notification deleted successfully.";
            public const string SuccessRestore = "Notification restored successfully.";

            public static string SuccessAdd(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " added successfully.";
            }

            public static string SuccessEdit(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " updated successfully.";
            }

            public static string SuccessTemporaryDelete(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " deleted temporarily.";
            }

            public static string SubscriptionExpired(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " is expired.";
            }

            public static string SubscriptionExpiring(int subscriptionId, int remainingDays)
            {
                return "Subscription #" + subscriptionId + " will expire in the next " + remainingDays + " days.";
            }

            public static string SuccessPermanentDelete(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " deleted permanently";
            }
        }

        public class AuthenticationConstants
        {
            public const string SecretKey = "AllianceVerySecretKey..............";
            public const string PasswordInvalid = "Password is not valid";
            public const string LoggedIn = "User is logged in";
            public const string PasswordChanged = "Password is changed";
            public const string EntryInvalid = "Authentication entry is not valid.";
            public const string TokenInvalid = "Token is not valid.";
        }
        public class UserConstants
        {
            public const string HeaderId = "Id";
            public const string HeaderCode = "Code";
            public const string HeaderEmailAddress = "EmailAddress";
            public const string HeaderFirstName = "FirstName";
            public const string HeaderMiddleName = "MiddleName";
            public const string HeaderLastName = "LastName";
            public const string HeaderDepartmentName = "DepartmentName";

            public const string EntryInvalid = "User entry is not valid.";
            public const string Exists = "User already exists";
            public const string DoesNotExist = "User does not exist.";
            public const string EmailAddressDoesNotExist = "User email address does not exist.";

            public const string SuccessAdd = "User added successfully.";
            public const string SuccessEdit = "User updated successfully.";
            public const string SuccessDelete = "User deleted successfully.";
            public const string SuccessRestore = "User restored successfully.";
        }


        public class EmailConstants
        {
            public const string From = "corralmarcjohn@gmail.com";
            public const string SmtpServer = "smtp.gmail.com";
            public const string Password = "zjigoixnywuljwev";
            public const int Port = 465;
            public const bool UseSsl = true;
            public const bool Quit = true;
            public const string Username = "ALLIANCE SOFTWARE INC.";
            public const string CannotReachEmail = "Cannot contact email address";
            public const string ResetPasswordEmailed = "Reset Password Email Sent";
        }

        public class ServiceConstants
        {
            public const string HeaderId = "Id";
            public const string HeaderName = "Name";
            public const string HeaderDescription = "Description";
            public const string HeaderPrice = "Price";
            public const string HeaderServiceTypeName = "ServiceTypeName";

            public const string EntryInvalid = "Service entry is not valid.";
            public const string Exists = "Service already exists";
            public const string DoesNotExist = "Service does not exist.";

            public const string SuccessAdd = "Service added successfully.";
            public const string SuccessEdit = "Service updated successfully.";
            public const string SuccessDelete = "Service deleted successfully.";
            public const string SuccessRestore = "Service restored successfully.";
        }

        public class DepartmentConstants
        {
            public const string EntryInvalid = "Department entry is not valid.";
            public const string Exists = "Department already exists";
            public const string DoesNotExist = "Department does not exist.";

            public const string SuccessAdd = "Department added successfully.";
            public const string SuccessEdit = "Department updated successfully.";
            public const string SuccessDelete = "Department deleted successfully.";
        }

        public class SubscriptionConstants
        {
            public const string HeaderId = "Id";
            public const string HeaderStartDate = "StartDate";
            public const string HeaderEndDate = "EndDate";
            public const string HeaderTotalPrice = "TotalPrice";
            public const string HeaderRemainingDays = "RemainingDays";
            public const string HeaderClientName = "ClientName";
            public const string HeaderServiceName = "ServiceName";
            public const string HeaderCreatedByCode = "CreatedByCode";
            public const string HeaderUpdatedByCode = "UpdatedByCode";

            public const string EntryInvalid = "Subscription entry is not valid.";
            public const string Exists = "Subscription already exists";
            public const string DoesNotExist = "Subscription does not exist.";

            public const string SuccessAdd = "Subscription added successfully.";
            public const string SuccessEdit = "Subscription updated successfully.";
            public const string SuccessDelete = "Subscription deleted successfully.";
            public const string SuccessRestore = "Subscription restored successfully.";
        }

        public class SubscriptionHistoryConstants
        {
            public const string SuccessAdd = "Subscription history added successfully.";
        }

        public class ImageConstants
        {
            public const string DefaultImage = "default_image.jpg";
        }

        public class PathConstants
        {
            public const string ProfilePicturesPath = "D:\\ALLIANCE LAST PROJECT\\Subscription Monitoring System Data\\Profile Pictures";
        }


        public class DatabaseConstants
        {
            public const string AssemblyName = "Subscription Monitoring System API";
            public const string ConnectionStringName = "DefaultConnection";
        }

        public class PasswordConstants
        {
            public const int SaltSize = 16;
            public const int HashSize = 16;
            public const int Iterations = 10000;
            public const string PasswordLengthError = "Minimum password length should be 8";
            public const string PasswordCharacterError = "Password should contain atleast one Uppercase Letter, one Lowercase Letter, one Number and one Special Character";
            public const string EntryInvalid = "Password entry is not valid.";
            public const string RefreshTokenError = "Refresh token had an error.";
        }
    }
}
