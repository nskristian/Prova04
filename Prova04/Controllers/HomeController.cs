using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Prova04.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prova04.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ConvertModel model)
        {

            var requisicaoWeb = WebRequest.CreateHttp("https://economia.awesomeapi.com.br/last/USD-BRL");
            requisicaoWeb.Method = "GET";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                dynamic response = JObject.Parse(objResponse.ToString());

                float high = response.USDBRL.high;
                float low = response.USDBRL.low;

                float avg = (high + low) / 2;

                float result = avg * model.Money;

                TempData["result"] = result.ToString("c2");

                streamDados.Close();
                resposta.Close();
            }

            return View();
        }
    }
}
