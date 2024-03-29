﻿using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Data
{
    public class EmailBody
    {
        public static string ResetEmailBody(string emailAddress, string refreshToken)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Reset your Password</h1>
            <hr>
            <p>Your receiving this email because you requested a password reset for your Subscription Monitoring System Account.</p>
            <p>Please click the link below to go to the reset password page</p>
            <a href=""http://localhost:4200/reset?email={emailAddress}&code={refreshToken}"">Reset Password Link</a>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }


        public static string CreateCredentialsEmailBody(string code, string password)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Account Credentials</h1>
            <hr>
            <p>Below is your account credentials for the Subscription Monitoring Management System.</p>
            <p>Id Number: {code}</p>
            <p>Password: {password}</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string UpdateCredentialsEmailBody(string code)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Updated Account Credentials</h1>
            <hr>
            <p>Below is your updated account credentials for the Subscription Monitoring Management System.</p>
            <p>Id Number: {code}</p>
            <p>Your password is still the same as before.</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string UpdateCredentialsEmailBody(string code, string password)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Updated Account Credentials</h1>
            <hr>
            <p>Below is your updated account credentials for the Subscription Monitoring Management System.</p>
            <p>Id Number: {code}</p>
            <p>Password: {password}</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string CheckEmailBody()
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Checking Email</h1>
            <hr>
            <p>Hi, We're just checking if this address is valid. </p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string SendSubscriptionEmail(string header, string message)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>{header}</h1>
            <hr>
            <p>{message}</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string SendCreatedSubscriptionEmail(string message, SubscriptionViewModel subscription)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Subscription Created</h1>
            <hr>
            <p>{message} Below are the information of the subscription:</p>
            <p>StartDate: {subscription.StartDate}</p>
            <p>EndDate: {subscription.EndDate}</p>
            <p>TotalPrice: {subscription.TotalPrice}</p>
            <p>RemainingDays: {subscription.RemainingDays}</p>
            <p>Client: {subscription.ClientName}</p>
            <p>Service: {subscription.ServiceName}</p>
            <p>CreatedOn: {subscription.CreatedOn}</p>
            <p>CreatedBy: {subscription.CreatedByName}</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }

        public static string SendUpdatedSubscriptionEmail(string emailAddress, string message, SubscriptionViewModel subscription, List<ClientViewModel> clients)
        {
            return $@"<html>
            <head>
            </head>
            <body >
            <div >
            <div>
            <div>
            <h1>Subscription Updated</h1>
            <hr>
            <p>{message} Below are the information of the subscription:</p>
            <p>StartDate: {subscription.StartDate}</p>
            <p>EndDate: {subscription.EndDate}</p>
            <p>TotalPrice: {subscription.TotalPrice}</p>
            <p>RemainingDays: {subscription.RemainingDays}</p>
            <p>Client: {subscription.ClientName}</p>
            <p>Service: {subscription.ServiceName}</p>
            <p>UpdatedOn: {subscription.UpdatedOn}</p>
            <p>UpdatedBy: {subscription.UpdatedByName}</p>
            <p>{(clients.Any(p => p.EmailAddress == emailAddress)? "" : "Due to certain changes you are no longer one of the notification recipients to this subscription. If you are any questions, please contact us.")}</p>
            <br>
            <p>Kind Regards,</p>
            <p>Alliance</p>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";
        }
    }
}
