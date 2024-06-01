using Mindee;
using Mindee.Http;
using Mindee.Input;
using Mindee.Product.Generated;
using Mindee.Product.Passport;

namespace CarInsuranceSalesBot.Services
{
    public class MindeeService
    {
        private readonly MindeeClient mindeeClient;

        public MindeeService(string apiKey)
        {
            mindeeClient = new MindeeClient(apiKey);
        }

        public async Task<string> GetPhotoInformation(byte[] photo)
        {
            try
            {
                string responseMessage = "Sorry, I can`t find needed data.";
                var inputSource = new LocalInputSource(photo, Constants.PASSPORT_FILE_NAME);
                //get data from passport
                var response = await mindeeClient
                    .ParseAsync<PassportV1>(inputSource);
                //check is it passport data in photo, if not than parse vin data
                if (response.Document.Inference.Prediction.IdNumber.Value == null)
                {
                    var data = await GetVehicleInformation(photo);

                    var vin = data.Prediction.Fields.First().Value.ToString().Replace(":value:", "").Trim();

                    if(String.IsNullOrEmpty(vin))
                    {
                        return responseMessage;
                    }

                    responseMessage = $"Thank you for the photo! This is your information. Your VIN: {vin}";
                }
                else
                {
                    responseMessage = $@"Thank you for the photo! This is your information:

Mrz: {response.Document.Inference.Prediction.Mrz1.Value},
BirthDate: {response.Document.Inference.Prediction.BirthDate.Value},
Country: {response.Document.Inference.Prediction.Country},
BirthPlace: {response.Document.Inference.Prediction.BirthPlace},
ID: {response.Document.Inference.Prediction.IdNumber}";
                }
                return responseMessage;
            }
            catch
            {
                throw new Exception("Something went wrong, try one more time!");
            }
        }

        private async Task<GeneratedV1> GetVehicleInformation(byte[] photo)
        {
            try
            {
                var inputSource = new LocalInputSource(photo, Constants.VIN_FILE_NAME);

                // Set the endpoint configuration
                CustomEndpoint endpoint = new CustomEndpoint(
                    endpointName: Constants.NAME_ENDPOINT,
                    accountName: Constants.ACCOUNT_ENDPOINT,
                    version: Constants.VERSION_ENDPOINT
                );

                // Call the product asynchronously with auto-polling
                var response = await mindeeClient
                    .EnqueueAndParseAsync<GeneratedV1>(inputSource, endpoint);

                return response.Document.Inference;
            }
            catch
            {
                throw new Exception("Something went wrong, try one more time!");
            }
        }
    }
}
