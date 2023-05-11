using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Data
{
    public static class Constants
    {
        public class SwaggerGenConstants
        {
            public const string Bearer = "Bearer";
            public const string Authorization = "Authorization";
            public const string JWT = "JWT";
        }

        public class DateConstants
        {
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            public const string DateOnlyFormat = "yyyy-MM-dd";
            public const string DateOnlyDefault = "0001-01-01";
            public const string DateTimeDefault = "0001-01-01 00:00:00";
        }

        public class BaseConstants
        {
            public const string CheckExpiration = "check-expiration";
            public const string RetrievedData = "Retrieved Data";
            public const string ErrorList = "Error List";
            public const int PageSize = 15;
        }
        public class SortDirectionConstants
        {
            public const string Ascending = "ASCENDING";
            public const string Descending = "DESCENDING";
            public const string SortDirectionInvalid = "Sort Direction is invalid.";
        }

        public class ClientConstants
        {
            public const string HeaderId = "ID";
            public const string HeaderName = "NAME";
            public const string HeaderEmailAddress = "EMAILADDRESS";
            public const string SortByInvalid = "Sort by is invalid.";

            public const string EntryInvalid = "Client entry is not valid.";
            public const string Exists = "Client name or email already exists";
            public const string NoChanges = "Client has no changes.";
            public const string DoesNotExist = "Client does not exist.";
            public const string NameInvalid = "Client name should be uppercased and not contain numbers and/or special characters";

            public const string SuccessAdd = "Client added successfully.";
            public const string SuccessEdit = "Client updated successfully.";
            public const string SuccessDelete = "Client deleted successfully.";
            public const string SuccessRestore = "Client restored successfully.";
        }

        public class ServiceTypeConstants
        {
            public const string DoesNotExist = "Service Type does not exist.";
            public const string SuccessAdd = "Service Type added successfully.";
            public const string SuccessEdit = "Service Type updated successfully.";
            public const string SuccessDelete = "Service Type deleted successfully.";
            public const string EntryInvalid = "Service Type entry is not valid.";
            public const string Exists = "Service Type name already exists";
            public const string NoChanges = "Service Type has no changes.";
            public const string NameInvalid = "Service Type name should be uppercased and not contain numbers and/or special characters";
        }

        public class UserNotificationConstants
        {
            public const string SuccessDelete = "User notification deleted successfully.";
            public const string SuccessRestore = "User notification deleted successfully.";
        }
        public class NotificationConstants
        {
            public const string SuccessCreate = "Notification added successfully.";
            public const string SuccessUpdate = "Notification updated successfully.";
            public const string DoesNotExist = "Notification does not exist.";
            public const string SuccessDelete = "Notification deleted successfully.";
            public const string CreateSubject = "Subscription Created";
            public const string UpdateSubject = "Subscription Updated";
            public const string ExpiringSubject = "Subscription Expiring";
            public const string ExpiredSubject = "Subscription Expired";
            public const string RestoredSubject = "Subscription Restored";
            public const string TemporaryDeleteSubject = "Subscription Temporarily Deleted";
            public const string PermanentDeleteSubject = "Subscription Permanently Deleted";
            public const string EntryInvalid = "Notification entry is not valid.";
            public const string Exists = "Notification description already exists";
            public const string InvalidDate = "Notification date is invalid";
            public const string NoChanges = "Notification has no changes.";

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

            public static string SuccessRestore(int subscriptionId)
            {
                return "Subscription #" + subscriptionId + " restored";
            }
        }

        public class AuthenticationConstants
        {
            public const string SecretKey = "AllianceVerySecretKey..............";
            public const string PasswordInvalid = "Password is not valid";
            public const string LoggedIn = "User is logged in";
            public const string PasswordChanged = "Password is changed";
            public const string ResetPassword = "Reset Password";
            public const string SuccessResetPassword = "Password Reset Successful";
            public const string EntryInvalid = "Authentication entry is not valid.";
            public const string TokenInvalid = "Token is not valid.";
            public const string TokenRefreshed = "Token is refreshed.";
        }
        public class UserConstants
        {
            public const string HeaderId = "ID";
            public const string HeaderCode = "CODE";
            public const string HeaderEmailAddress = "EMAILADDRESS";
            public const string HeaderFirstName = "FIRSTNAME";
            public const string HeaderMiddleName = "MIDDLENAME";
            public const string HeaderLastName = "LASTNAME";
            public const string HeaderDepartmentName = "DEPARTMENTNAME";
            public const string SortByInvalid = "Sort by is invalid.";

            public const string EntryInvalid = "User entry is not valid.";
            public const string Exists = "User already exists";
            public const string DoesNotExist = "User does not exist.";
            public const string NoChanges = "User has no changes.";
            public const string AccountCredentials = "Account Credentials";
            public const string UpdatedAccountCredentials = "Updated Account Credentials";
            public const string EmailAddressDoesNotExist = "User email address does not exist.";
            public const string FirstNameInvalid = "User first name should be uppercased and not contain numbers and/or special characters";
            public const string MiddleNameInvalid = "User middle name should be uppercased and not contain numbers and/or special characters";
            public const string LastNameInvalid = "User last name should be uppercased and not contain numbers and/or special characters";

            public const string SuccessAdd = "User added successfully.";
            public const string SuccessEdit = "User updated successfully.";
            public const string SuccessDelete = "User deleted successfully.";
            public const string SuccessRestore = "User restored successfully.";
        }


        public class EmailConstants
        {
            public const string From = "jd2925084@gmail.com";
            public const string SmtpServer = "smtp.gmail.com";
            public const string Password = "agyipwwhukvfotyz";
            public const int Port = 465;
            public const bool UseSsl = true;
            public const bool Quit = true;
            public const string Username = "ALLIANCE SOFTWARE INC.";
            public const string CannotReachEmail = "Cannot contact email address";
            public const string ResetPasswordEmailed = "Reset Password Email Sent";
            public const string CheckingEmailSubject = "Checking Email";
        }

        public class ServiceConstants
        {
            public const string HeaderId = "ID";
            public const string HeaderName = "NAME";
            public const string HeaderDescription = "DESCRIPTION";
            public const string HeaderPrice = "PRICE";
            public const string HeaderServiceDurationName = "SERVICEDURATIONNAME";
            public const string SortByInvalid = "Sort by is invalid.";

            public const string EntryInvalid = "Service entry is not valid.";
            public const string Exists = "Service name or description already exists";
            public const string NoChanges = "Service has no changes.";
            public const string DoesNotExist = "Service does not exist.";
            public const string NameInvalid = "Service name should be uppercased and not contain numbers and/or special characters";

            public const string SuccessAdd = "Service added successfully.";
            public const string SuccessEdit = "Service updated successfully.";
            public const string SuccessDelete = "Service deleted successfully.";
            public const string SuccessRestore = "Service restored successfully.";
        }

        public class DepartmentConstants
        {
            public const string DoesNotExist = "Department does not exist.";
            public const string SuccessAdd = "Department added successfully.";
            public const string SuccessEdit = "Department updated successfully.";
            public const string SuccessDelete = "Department deleted successfully.";
            public const string EntryInvalid = "Department entry is not valid.";
            public const string Exists = "Department name already exists";
            public const string NoChanges = "Department has no changes.";
            public const string NameInvalid = "Department name should be uppercased and not contain numbers and/or special characters";
        }

        public class ExcelConstants
        {
            public const string ActualData = "@@ActualData";
            public const string Contettype = "application/octet-stream";
            public const string FileType = ".xls";
        }

        public class SubscriptionConstants
        {
            public const string HeaderId = "ID";
            public const string HeaderStartDate = "STARTDATE";
            public const string HeaderEndDate = "ENDDATE";
            public const string HeaderTotalPrice = "TOTALPRICE";
            public const string HeaderRemainingDays = "REMAININGDAYS";
            public const string HeaderClientName = "CLIENTNAME";
            public const string HeaderServiceName = "SERVICENAME";
            public const string HeaderCreatedByCode = "CREATEDBYCODE";
            public const string HeaderUpdatedByCode = "UPDATEDBYCODE";
            public const string SortByInvalid = "Sort by is invalid.";

            public const string EntryInvalid = "Subscription entry is not valid.";
            public const string Exists = "Subscription already exists";
            public const string NoChanges = "Subscription has no changes.";
            public const string DoesNotExist = "Subscription does not exist.";
            public const string InvalidStartDate = "Start date is invalid";
            public const string InvalidEndDate = "End date is invalid";
            public const string InvalidDates = "End date should not be before or on start date";

            public const string SuccessAdd = "Subscription added successfully.";
            public const string SuccessEdit = "Subscription updated successfully.";
            public const string SuccessDelete = "Subscription deleted successfully.";
            public const string SuccessRestore = "Subscription restored successfully.";

            public static string RowData(SubscriptionViewModel subscription)
            {
                return "<tr><td>" + subscription.Id + "</td><td>" + subscription.StartDate + "</td><td>" + subscription.EndDate + "</td><td>" + subscription.TotalPrice + "</td><td>" + subscription.RemainingDays +
                         "</td><td>" + subscription.ClientName + "</td><td>" + subscription.ServiceName + "</td><td>" + subscription.CreatedOn + "</td><td>" + subscription.CreatedByCode +
                          "</td><td>" + subscription.CreatedByName + "</td><td>" + subscription.UpdatedOn + "</td><td>" + subscription.UpdatedByCode + "</td><td>" + subscription.UpdatedByName + "</td></tr>";
            }
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
            public const string ProfilePicturesPath = "D:\\Subscription Monitoring API\\Subscription Monitoring System Data\\Profile Pictures";
            public const string ProfilePicturesRequestPath = "/profilepictures";
            public const string SubscriptionExcelTemplatePath = "D:\\Subscription Monitoring API\\Subscription Monitoring System\\ExcelTemplate\\Subscriptions.html";
            public const string ExcelFilesPath = "D:\\Subscription Monitoring API\\Subscription Monitoring System\\ExcelFiles";
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


            public static string CreateTemporaryPassword(string lastname)
            {
                return "Alliance" + lastname.Replace(" ", string.Empty) + "@123";
            }
        }
    }
}
