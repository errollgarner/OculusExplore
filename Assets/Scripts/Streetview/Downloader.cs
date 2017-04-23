﻿//  =====================================================================
//  OculusExplore
//  Copyright(C)                                      
//  2017 Maksym Perepichka
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  GNU General Public License for more details.
//            
//  You should have received a copy of the GNU General Public License 
//  along with this program.If not, see<http://www.gnu.org/licenses/>.
//  =====================================================================

using System;
using System.IO;
using System.Net;
using System.Threading;
using Boo.Lang;
using UnityEngine;

namespace Streetview
{
    public class Downloader : MonoBehaviour
    {

        //
        // Constants
        //

        // Tokens for finding the id from a streetview url
        private const string UrlTokenStart = "!1s";
        private const string UrlTokenEnd = "!2e";

        // Base URL from which to download the images
        private const string UrlDownloadBase =
            @"http://geo1.ggpht.com/cbk?cb_client=maps_sv.tactile&authuser=0&hl=en&panoid=";

        // Other tags that we will need
        private const string UrlDownloadOutput = @"&output=tile";
            
        private const string UrlDownloadValues = "&x=X&y=Y&zoom=Z";

        // Base file path where to store the images
        private const string FilePathBase = @"Assets\Resources\Streetview\Tiles\";

        //
        // Members
        //
        public int XMax;
        public int YMax;

        public int Size = 512;

        public string BaseUrl;
        public string FileName; 

        // Zoom level controls how zoomed in each individual tile is
        // and subsequently how many tiles are needed in total
        public int ZoomLevel;

        // Memory stream with images
        public List<List<Texture2D>> Images;

        // Sempahore for reading/writing images

        public bool ImagesReady;

        // Use this for initialization
        void Start ()
        {
            // Starts images stream
            Images = new List<List<Texture2D>>();
           
            // Sets up images ready
            ImagesReady = false;

            // Downloads the images
            Download();
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        //
        // Privates methods methods
        //

        private void Download()
        {
            // Gets the URL
            string tempUrl = ParseUrl(BaseUrl);

            // Starts downloading the images
            DownloadImages(tempUrl, ZoomLevel);

            // Sets imagesReady
            ImagesReady = true;

        }

        // Parses URL and extracts the key that we will need to download individual images
        private static string ParseUrl(string url)
        { 
            // Gets starting index
            var index1 = url.IndexOf(UrlTokenStart) + UrlTokenStart.Length;
            var index2 = url.IndexOf(UrlTokenEnd);

            return url.Substring(index1, (index2-index1));
        }

        // Sets up image for download
        private bool DownloadImages(string url, int zoomLevel)
        {
            XMax = 0;
            YMax = 0;

            // Sets up our values
            switch (zoomLevel)
            {
                case 3:
                    XMax = 7;
                    YMax = 3;
                    break;
                case 4:
                    XMax = 13;
                    YMax = 6;
                    break;
                default:
                    return false;
            }

            // Loops through creating the urls we will need
            for (int y = 0; y < YMax; y++)
            {
                List<Texture2D> imageRow = new List<Texture2D>();

                for (int x = 0; x < XMax; x++)
                {
                
                    string tempDownloadValues = UrlDownloadValues.Replace("X", x.ToString()).Replace("Y", y.ToString()).Replace("Z", zoomLevel.ToString());
                    string tempUrl = UrlDownloadBase + url + UrlDownloadOutput + tempDownloadValues;
                    string tempFileName = FilePathBase + "tile-x" + x + "-y" + y + ".jpg";

                    Debug.Log(tempUrl);
                    imageRow.Add(DownloadRemoteImageFile(tempUrl, tempFileName));
                    
                }

                Images.Add(imageRow);

            }
            
            return true;
        }

        // Downloads image from remote server to the memory stream
        private Texture2D DownloadRemoteImageFile(string uri, string fileName)
        {
            MemoryStream imageStream = new MemoryStream();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                using (Stream inputStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);

                        imageStream.Write(buffer, 0, bytesRead);

                    } while (bytesRead != 0);
                }
            }
            else
            {
                Debug.Log("Failed to load online image");
            }

            Texture2D tex = new Texture2D(Size, Size);
            tex.LoadImage(imageStream.GetBuffer());
            return tex;
        }
    }
}
