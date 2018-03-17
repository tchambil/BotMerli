using Newtonsoft.Json;
using SimpleEchoBot.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Services
{
    [Serializable]
    public class PeopeAppService
    {
        private static readonly string UrlApiBritanico = ConfigurationManager.AppSettings["URL_SERVICE"];


        public async Task<ResultAutenticate> Autenticate(UserLogin input)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "THIS TOKEN");
                httpClient.BaseAddress = new Uri(UrlApiBritanico);
                var response = await httpClient.GetAsync("api/trabajador/autenticacion?user=" + input.UserOrEmailAdrees + "&pass=" + input.Password);
                if (response.StatusCode.ToString() != "OK")
                {
                    return null;
                }
                else
                {
                    var resultContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ResultAutenticate>(resultContent);
                }
            }
        }
        public async Task<Holiday> GetRRHHHolidays(ResultAutenticate input)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "THIS TOKEN");
                httpClient.BaseAddress = new Uri(UrlApiBritanico);
                var response = await httpClient.GetAsync("api/vacaciones/consultar?codigo=" + input.Codigo);
                if (response.StatusCode.ToString() != "OK")
                {
                    return null;
                }
                else
                {
                    var resultContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Holiday>(resultContent);
                }
            }
        }
        public async Task<People> SearchByNameProfesor(string People)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "THIS TOKEN");
                httpClient.BaseAddress = new Uri(UrlApiBritanico);
                var response = await httpClient.GetAsync("api/trabajador/consultar?tipo=1&nombres=" + People);
                if (response.StatusCode.ToString() != "OK")
                {
                    return null;
                }
                else
                {
                    var resultContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<People>(resultContent);
                }
            }
        }
        public async Task<People> CreateSurvey(string vote)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "THIS TOKEN");
                httpClient.BaseAddress = new Uri(UrlApiBritanico);
                var response = await httpClient.GetAsync("api/trabajador/survey?vote=" + vote);
                if (response.StatusCode.ToString() != "OK")
                {
                    return null;
                }
                else
                {
                    var resultContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<People>(resultContent);
                }
            }
        }
        public async Task<List<People>> SearchByNamePeople(string People)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "THIS TOKEN");
                httpClient.BaseAddress = new Uri(UrlApiBritanico);
                var response = await httpClient.GetAsync("api/trabajador/consultar?tipo=0&nombres=" + People);
                if (response.StatusCode.ToString() != "OK")
                {
                    return null;
                }
                else
                {
                    var resultContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<People>>(resultContent);
                }

            }
        }
    }
}