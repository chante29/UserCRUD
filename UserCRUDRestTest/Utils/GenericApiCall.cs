using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace UserCRUDRestTest.Utils
{
    public class ObjectGenericApiCall
    {
        public HttpMethod MethodRequest { get; set; }
        public List<string> ParamsResource { get; set; }
        public Dictionary<string, object> QueryStringParams { get; set; }
    }

    public class GenericApiCall
    {
        #region Constants

        private const string _callerResource = "User";

        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        private static string _apiUri;

        #endregion

        #region Constructor

        public GenericApiCall(string apiUri)
        {
            _apiUri = apiUri;
        }

        #endregion
        

        #region GenericTests
        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TU LaunchTest<T, TU>(ObjectGenericApiCall request, T jsonObject)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request, jsonObject);

            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);

                return response.Content.ReadAsAsync<TU>().Result;
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TU LaunchOkCreateTest<T, TU>(ObjectGenericApiCall request, T jsonObject)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request, jsonObject);

            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);

                return response.Content.ReadAsAsync<TU>().Result;
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public T LaunchTest<T>(ObjectGenericApiCall request)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request);


            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                return response.Content.ReadAsAsync<T>().Result;
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void LaunchTest<T>(ObjectGenericApiCall request, T jsonObject)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request, jsonObject);


            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void LaunchTest(ObjectGenericApiCall request)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request);


            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }



        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public T LaunchNoContentTest<T>(ObjectGenericApiCall request)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request);


            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.IsNotNull(response.Content);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                return response.Content.ReadAsAsync<T>().Result;
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void LaunchBadRequestTest<T>(ObjectGenericApiCall request, T jsonObject)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request, jsonObject);

            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        /// <summary>
        /// Launch a generic HttpRequestMessage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void LaunchBadRequestTest(ObjectGenericApiCall request)
        {
            var client = new HttpClient();

            HttpRequestMessage requestMessage = GetRequestMessage(request);

            using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        #endregion

        #region Private Methods


        public static HttpRequestMessage GetRequestMessage<T>(ObjectGenericApiCall request, T jsonObject)
        {
            var requestMessage = GetRequestMessage(request);

            if (jsonObject != null)
            {
                var postObject = _serializer.Serialize(jsonObject);
                requestMessage.Content = new StringContent(postObject, Encoding.UTF8, "application/json");
            }

            return requestMessage;
        }



        public static HttpRequestMessage GetRequestMessage(ObjectGenericApiCall request)
        {
            string paramsResource = (request.ParamsResource == null || request.ParamsResource.Count == 0)
                            ? string.Empty
                            : "/" + string.Join("/", request.ParamsResource.ToArray());
            string queryStringParams = string.Empty;

            if (request.QueryStringParams != null && request.QueryStringParams.Count > 0)
            {
                queryStringParams = string.Format("?{0}",
                                                    string.Join("&", request.QueryStringParams.Select(a => string.Format("{0}={1}", a.Key, a.Value.ToString())))
                                                   );
            }

            string uri = string.Format("{0}/{1}{2}{3}", _apiUri, _callerResource, paramsResource, queryStringParams);
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(uri)
            };
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Method = request.MethodRequest;
            return requestMessage;
        }

        #endregion

        #region Generate Generic Request

        public static ObjectGenericApiCall GenerateGenericRequest(HttpMethod httpMethod)
        {
            return new ObjectGenericApiCall
            {
                MethodRequest = httpMethod,
                ParamsResource = new List<string>()
            };
        }

        public static ObjectGenericApiCall GenerateGenericRequest(HttpMethod httpMethod, List<string> paramsResource)
        {
            var genericRequest = GenerateGenericRequest(httpMethod);
            genericRequest.ParamsResource = paramsResource;
            return genericRequest;
        }
        public static ObjectGenericApiCall GenerateGenericRequest(HttpMethod httpMethod, List<string> paramsResource, Dictionary<string, object> queryStringParams)
        {
            var genericRequest = GenerateGenericRequest(httpMethod);
            genericRequest.ParamsResource = paramsResource;
            genericRequest.QueryStringParams = queryStringParams;
            return genericRequest;
        }

        #endregion
    }
}
