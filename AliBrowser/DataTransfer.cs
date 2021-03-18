using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace AliBrowser
{
    public class DataTransfer
    {
        string fileName = "Setting.xml";

        public async void SaveSearchTerm(string searchTerm, string title, string url)
        {
            var doc = await DocumentLoad().AsAsyncOperation();  // Load XML File

            var history =  doc.GetElementsByTagName("history");

            XmlElement elSearchTerm = doc.CreateElement("searchterm");
            XmlElement elSiteName = doc.CreateElement("sitename");
            XmlElement elUrl = doc.CreateElement("url");

            var historyItem = history[0].AppendChild(doc.CreateElement("historyitem"));

            historyItem.AppendChild(elSearchTerm);
            historyItem.AppendChild(elSiteName);
            historyItem.AppendChild(elUrl);

            elSearchTerm.InnerText = searchTerm;
            elSiteName.InnerText = title;
            elUrl.InnerText = url;

            SaveDoc(doc);
        }

        private async Task<XmlDocument> DocumentLoad()
        {
            XmlDocument result = null;

            await Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                XmlDocument doc = await XmlDocument.LoadFromFileAsync(file);
                result = doc;
            });

            return result;
        }

        private async void SaveDoc(XmlDocument doc)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            await doc.SaveToFileAsync(file);
        }
    }
}
 