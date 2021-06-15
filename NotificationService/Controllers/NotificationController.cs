using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationService.Controllers
{

    [Route("api/notificationcontroller")]
    public class NotificationController : Controller
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IConfiguration configuration;

        public NotificationController(ITelegramBotClient telegramBotClient, IConfiguration configuration)
        {
            this.telegramBotClient = telegramBotClient;
            this.configuration = configuration;

        }

        [HttpPost]
        [Route("sendNotification", Name = "SendNotification")]
        public ActionResult<Message> SendNotification(string chatId ,string msg)
        {
            try
            {
                return this.telegramBotClient.SendTextMessageAsync(chatId, msg).Result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
