using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Constants
{
    public class ConstantsValues
    {
        public const string ProductExpiringSoon = "Product is expiring soon - Name: {0}, Code: {1} Expiry date: {2}.";
        public const int TotalWeeksMoreThan = 0;
        public const string UserAlreadyExists = "User already exists.";
        public const string UserNotFound = "User not found. Please contact administrator.";
        public const string ErrorOccurredCreatingUser = "An error occurred when creating user. Please contact administrator.";
        public const string ErrorOccurredUpdatingUser = "An error occurred when updating user. Please contact administrator.";
        public const string ErrorOccurredDeletingUser = "An error occurred when deleting user. Please contact administrator.";
        public const string ErrorOccurredChangingPassword = "An error occurred when changing password. Please contact administrator.";

        public const string ProductAlreadyExists = "Product already exists.";
        public const string ProductNotFound = "Product not found.";
        public const string ErrorOccurredCreatingProduct = "An error occurred when creating Product. Please contact administrator.";
        public const string ErrorOccurredUpdatingProduct = "An error occurred when updating Product. Please contact administrator.";
        public const string ErrorOccurredDeletingProduct = "An error occurred when deleting Product. Please contact administrator.";

        public const string Debug = "Debug";
        public const string Error = "Error";
    }
}
