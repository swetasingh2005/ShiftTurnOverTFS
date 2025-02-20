using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTurnover.Components
{
    /// <summary>
    /// This is a generic component used by the developers to validate.
    /// </summary>
    public class Validation
    {
        public Validation() { }
        /// <summary>
        /// Test for Positive Integers.
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a natural number.</returns>
        public bool IsNaturalNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");

            return !objNotNaturalPattern.IsMatch(strNumber) &&
                objNaturalPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// Test for Positive Integers with zero inclusive
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a whole number.</returns>
        public bool IsWholeNumber(string strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");

            return !objNotWholePattern.IsMatch(strNumber);
        }
        /// <summary>
        /// Test for Integers both Positive & Negative
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a integer.</returns>
        public bool IsInteger(string strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");

            return !objNotIntPattern.IsMatch(strNumber) &&
                objIntPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Test for Floats/Decimals
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a float/decimal</returns>
        public bool IsDecimal(string strNumber)
        {
            Regex objDecPattern = new Regex(@"(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)");

            return objDecPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Test for Positive Number both Integer & Real
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a positive number.</returns>
        public bool IsPositiveNumber(string strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !objNotPositivePattern.IsMatch(strNumber) &&
                objPositivePattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// Test whether the string is valid number or not
        /// </summary>
        /// <param name="strNumber">String to check.</param>
        /// <returns>True/False if the string is a number.</returns>
        public bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// Check for AlphaNumeric.
        /// </summary>
        /// <param name="strToCheck">String to check.</param>
        /// <returns>True/False if the string is alpha-numeric.</returns>
        public bool IsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }
        /// <summary>
        /// Check for valid date format.
        /// </summary>
        /// <param name="strDate">String to check.</param>
        /// <returns>True/False if the string is a valid date.</returns>
        public bool IsDate(string strDate)
        {
            DateTime dtDate;
            bool bValid = true;
            try
            {
                dtDate = DateTime.Parse(strDate);
            }
            catch (FormatException)
            {
                // the Parse method failed => the string strDate cannot be converted to a date.
                bValid = false;
            }
            return bValid;
        }
        /// <summary>
        /// Check for valid ip address.
        /// </summary>
        /// <param name="strDate">String to check.</param>
        /// <returns>True/False if the string is a valid ip address.</returns>
        public bool IsIPAddress(string strIP)
        {
            Regex objIPPattern = new Regex(@"(?<First>[01]?\d\d?|2[0-4]\d|25[0-5])\.(?<Second>[01]?\d\d?|2[0-4]\d|25[0-5])\.(?<Third>[01]?\d\d?|2[0-4]\d|25[0-5])\.(?<Fourth>[01]?\d\d?|2[0-4]\d|25[0-5])(?x)");

            return objIPPattern.IsMatch(strIP);
        }
        /// <summary>
        /// Check for valid email address.
        /// </summary>
        /// <param name="strEmail">String to check.</param>
        /// <returns>True/False if the string is a valid email address.</returns>
        public bool IsEmail(string strEmail)
        {
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");

            return objIntPattern.IsMatch(strEmail);
        }
        /// <summary>
        /// Check for valid phone number.
        /// </summary>
        /// <param name="strPhone">String to check.</param>
        /// <returns>True/False if the string is a valid phone number.</returns>
        public bool IsPhone(string strPhone)
        {
            Regex objIntPattern = new Regex(@"^\(\d{3}\)\s\d{3}-\d{4}$");

            return objIntPattern.IsMatch(strPhone);
        }
    }
}
