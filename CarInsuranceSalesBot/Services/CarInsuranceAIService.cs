using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInsuranceSalesBot.Services
{
    public class CarInsuranceAIService
    {
        private readonly OpenAIAPI openAiApi;

        public CarInsuranceAIService(string token)
        {
            APIAuthentication aPIAuthentication = new APIAuthentication(token); // login to open ai
            openAiApi = new OpenAIAPI(aPIAuthentication);
        }

        public async Task<string> GetMessageAsync(string inputText)
        {
            try
            {  
                string prompt = $"Imagine you're an auto insurance sales bot. You need to reply to this message :{inputText}"; //promt to open ai
                var conversation = openAiApi.Chat.CreateConversation(); //creating conversation
                conversation.AppendUserInput(prompt); //add promt to conversation

                return await conversation.GetResponseFromChatbotAsync(); //get response from open ai
            }
            catch(Exception)
            {
                throw new Exception("Oops, something wrong with conversation, try again later");
            }
        }
    }
}
