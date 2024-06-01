using CarInsuranceSalesBot.Services.IServices;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CarInsuranceSalesBot.Services
{
    public class TelegramService
    {
        private static ITelegramBotClient _botClient;
        private static IPdfService _pdfService;

        private static MindeeService _mindeeService;
        private static CarInsuranceAIService _carInsuranceAIService;

        public TelegramService()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            var configuration = builder.Build();

            var mindeeAPIToken = configuration["MindeeAPIToken"];
            var openAIToken = configuration["OpenAIToken"];
            var telegramAPIToken = configuration["TelegramAPIToken"];

            _pdfService = new PdfService();
            _botClient = new TelegramBotClient(telegramAPIToken);
            _mindeeService = new MindeeService(mindeeAPIToken);
            _carInsuranceAIService = new CarInsuranceAIService(openAIToken);
        }

        public void StartBot()
        {
            _botClient.StartReceiving(Update, Error);
        }

        private async Task Update(ITelegramBotClient botClient, Update e, CancellationToken arg3)
        {
            try
            {
                if (e.Message != null && e.Message.Text != null)
                {
                    await HandleTextMessageAsync(botClient, e.Message);
                }
                else if (e.Message?.Photo != null)
                {
                    await HandlePhotoMessageAsync(botClient, e.Message);
                }
            }
            catch (Exception ex)
            {
                await SendTextMessageAsync(botClient, e.Message.Chat.Id, ex.Message, new ReplyKeyboardRemove());
            }
        }
        private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message message)
        {
            var formatedText = FormatCommand(message.Text); //format message to normal 

            switch (formatedText)
            {
                case Constants.COMMAND_START:
                case Constants.COMMAND_INTRODUCE:
                    await SendTextMessageAsync(
                        botClient,
                        message.Chat.Id,
                        Constants.INTRODUCE_MSG,
                        new ReplyKeyboardMarkup(new[]
                            {
                                new KeyboardButton(Constants.BUTTON_GET_INSURANCE),
                            })
                        { ResizeKeyboard = true });
                    break;

                case Constants.COMMAND_GET_INSURANCE:
                    await SendTextMessageAsync(botClient, message.Chat.Id, Constants.GET_PHOTO, new ReplyKeyboardRemove());
                    break;

                case Constants.COMMAND_AGREED_WITH_DATA:
                    await SendTextMessageAsync(
                        botClient,
                        message.Chat.Id,
                        Constants.INFORM_PRICE,
                        new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton(Constants.COMMAND_AGREED_WITH_PRICE),
                            new KeyboardButton(Constants.COMMAND_DISAGREED_WITH_PRICE)
                        })
                        { ResizeKeyboard = true });
                    break;

                case Constants.COMMAND_DISAGREED_WITH_DATA:
                    await SendTextMessageAsync(botClient, message.Chat.Id, Constants.ANOTHER_PHOTO, new ReplyKeyboardRemove());
                    break;

                case Constants.COMMAND_AGREED_WITH_PRICE:
                    await SendAgreementDocumentAsync(botClient, message.Chat.Id);
                    break;

                case Constants.COMMAND_DISAGREED_WITH_PRICE:
                    await HandleDisagreementAsync(botClient, message.Chat.Id);
                    break;

                default:
                    await HandleDefaultMessageAsync(botClient, message.Chat.Id, message.Text);
                    break;
            }
        }

        private async Task HandlePhotoMessageAsync(ITelegramBotClient botClient, Message message)
        {
            var fileId = message.Photo[^1].FileId;
            var file = await botClient.GetFileAsync(fileId);
            byte[] photoPassport;

            using (var fileStream = new MemoryStream())
            {
                await botClient.DownloadFileAsync(file.FilePath, fileStream);
                photoPassport = fileStream.ToArray();
            }

            var passportInfo = await _mindeeService.GetPhotoInformation(photoPassport);

            await SendTextMessageAsync(
                botClient,
                message.Chat.Id,
                passportInfo,
                new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton(Constants.COMMAND_AGREED_WITH_DATA),
                    new KeyboardButton(Constants.COMMAND_DISAGREED_WITH_DATA)
                })
                { ResizeKeyboard = true });
        }

        private async Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string text, IReplyMarkup replyMarkup)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyMarkup
            );
        }

        private async Task SendAgreementDocumentAsync(ITelegramBotClient botClient, long chatId)
        {
            byte[] file = _pdfService.GetInsuranceAgreement();

            using (MemoryStream stream = new MemoryStream(file))
            {
                await botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: InputFile.FromStream(stream, Constants.FILE_NAME),
                    caption: Constants.AGREEMENT_FILE,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }
        }

        private async Task HandleDisagreementAsync(ITelegramBotClient botClient, long chatId)
        {
            string responseMsg = await _carInsuranceAIService.GetMessageAsync(Constants.APOLOGIZE_PRICE);

            await SendTextMessageAsync(botClient, chatId, responseMsg, new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton(Constants.BUTTON_AGREED_PRICE),
                new KeyboardButton(Constants.BUTTON_DISAGREED_PRICE)
            })
            { ResizeKeyboard = true });
        }

        private async Task HandleDefaultMessageAsync(ITelegramBotClient botClient, long chatId, string text)
        {
            string responseMsg = await _carInsuranceAIService.GetMessageAsync(text);
            await SendTextMessageAsync(botClient, chatId, responseMsg, new ReplyKeyboardRemove());
        }

        private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw arg2;
        }

        private string FormatCommand(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new Exception("Message is empty");
            }

            // Convert to lowercase Remove slash Remove underline
            return message.ToLower()
                .Replace("/", "")
                .Replace("_", " ");
        }
    }
}
