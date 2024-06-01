using CarInsuranceSalesBot.Services;

TelegramService telegramService = new TelegramService();

telegramService.StartBot();

Console.WriteLine("Press any key to exit");
Console.ReadKey();