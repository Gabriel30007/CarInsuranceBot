using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInsuranceSalesBot
{
    public static class Constants
    {
        //response message constants
        public const string INTRODUCE_MSG = @"Welcome to the Car Insurance Assistant Bot! This simple yet powerful tool is designed to help you seamlessly purchase car insurance right from your Telegram app. Here's how it works:

Document Processing: Submit your necessary documents through the bot. It will securely process your information to ensure a smooth insurance application.

AI-Driven Communication: The bot utilizes advanced AI to interact with you, answering your questions and guiding you through each step of the insurance purchase process.

Transaction Confirmation: Once everything is in order, the bot will confirm your transaction details, ensuring you have all the information you need about your new car insurance policy.

Start your journey towards easy and efficient car insurance purchasing with the Car Insurance Assistant Bot today.
You can share your photo of vehicle identification document and passport";

        public const string GET_PHOTO = "Send a photo of your passport and vehicle information document, please";
        public const string INFORM_PRICE = "We are pleased to inform you that the fixed price for our car insurance is 100 USD!";
        public const string ANOTHER_PHOTO = "Send me another photo!";
        public const string AGREEMENT_FILE = "Your agreement is ready! Download it.";
        public const string APOLOGIZE_PRICE = "You should apologize and explain that the $100 insurance is the only price available";

        //response menu constants
        public const string COMMAND_START = "start";
        public const string COMMAND_INTRODUCE = "introduce";
        public const string COMMAND_GET_INSURANCE = "get insurance";
        public const string COMMAND_AGREED_WITH_DATA = "agreed with data";
        public const string COMMAND_DISAGREED_WITH_DATA = "disagreed with data";
        public const string COMMAND_AGREED_WITH_PRICE = "agreed with price";
        public const string COMMAND_DISAGREED_WITH_PRICE = "disagreed with price";

        //file name  constants
        public const string FILE_NAME = "Insurance.pdf";
        public const string PASSPORT_FILE_NAME = "mockPhotoPassport.jpg";
        public const string VIN_FILE_NAME = "Insurance.pdf";

        //buttons  constants
        public const string BUTTON_AGREED_PRICE = "Agreed with price";
        public const string BUTTON_DISAGREED_PRICE = "Disagreed with price";
        public const string BUTTON_GET_INSURANCE = "Get insurance";

        //mindee endpoints
        public const string NAME_ENDPOINT = "vin";
        public const string ACCOUNT_ENDPOINT = "Gabriel3007";
        public const string VERSION_ENDPOINT = "1";



    }
}
