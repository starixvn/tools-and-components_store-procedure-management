// --------------------------------------------------------------------------
// <copyright file="AsyncRequsest.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Client
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// The asynchronous request class.
    /// </summary>
    public static class AsyncRequsest
    {
        private const int _DefaultTimeout = 30000; // 0.5 minutes timeout

        /// <summary>
        /// Makes the request.
        /// </summary>
        /// <param name="url">The URL of service.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The task of request.</returns>
        public static Task<string> MakeRequest(
            string url, 
            string methodName = WebRequestMethods.Http.Get, 
            string contentType = "text/json")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = methodName;
            request.Timeout = _DefaultTimeout;
            request.ContentLength = 0;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        /// <summary>
        /// Reads the stream from response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Return UM content.</returns>
        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    ////Need to return this response 
                    string strContent = sr.ReadToEnd();

                    return strContent;
                }
            }
        }
    }
}
