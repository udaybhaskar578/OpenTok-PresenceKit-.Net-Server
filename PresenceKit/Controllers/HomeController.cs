using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.WebPages;
using Newtonsoft.Json;
using OpenTokSDK;

/***************************************************************************
 * Source: Presence Kit - PHP ( https://tokbox.com/developer/starter-kits/)
 * Modified by : Sai Uday Bhaskar Mudivarty
 * Uses Backbone.Js,Lodash.js,OpenTok SDK
 * All the routings are defined in Global.asax file
 ***************************************************************************/
namespace PresenceKit.Controllers
{
    public class HomeController : Controller
    {
        public OpenTok Tk;
        // Replace these values with the values from your TokBox Dashboard
        const string Secret = "";
        // Make sure that you get a Routed SessionId from your OpenTok Dashboard if you are trying to archive the session.
        const string SessionId = "";
        const int ApiKey = 45273832;
        [HttpGet]
        public ActionResult Presence()
        {
            var responseData = new { apiKey = ApiKey, sessionId = SessionId };
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Users()
        {
            Stream s = Request.InputStream;
            var sr = new StreamReader(s);
            Newtonsoft.Json.Linq.JObject jObj = Newtonsoft.Json.Linq.JObject.Parse(sr.ReadLine());
            var name = (string)jObj["name"];
            int nameLen = name.Length;
            if (nameLen == 0 || nameLen > 25)
            {
                Response.SetStatus(400);
            }
            string token = Settings.Tk.GenerateToken(SessionId, Role.SUBSCRIBER, data:JsonConvert.SerializeObject((new { name})));
            var responseData = new { token };
            return Json(responseData, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Chats()
        {
            /*********************************************************************************************************
             * Use this if you would like to archive the chat session.
             * var chatSession = Settings.Tk.CreateSession(mediaMode: MediaMode.ROUTED, archiveMode: ArchiveMode.ALWAYS);
             *********************************************************************************************************/
            var chatSession = Settings.Tk.CreateSession();
            var sessionId = chatSession.Id;
            var token = chatSession.GenerateToken();
            var responseData = new
            {
                apiKey = ApiKey,
                sessionId,
                token
            };
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("Chats")]
        public ActionResult ChatsGet()
        {
            string token;
            string sessionId;
            try
            {
                
                Stream s = Request.InputStream;
                sessionId = Request.Params["sessionId"];
                if (sessionId.IsEmpty())
                {
                    Response.SetStatus(404);
                }
                token = Settings.Tk.GenerateToken(sessionId);
            }
            catch (Exception)
            {
                Response.SetStatus(404);
                throw;
            }
            var responseData = new { ApiKey, sessionId, token };
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}