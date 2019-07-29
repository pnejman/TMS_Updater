using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TMS_Updater
{
    public class LanguageDictionary
    {
        public Dictionary<string, string> txt = new Dictionary<string, string>();

        public LanguageDictionary(string language)
        {
            try
            {
                XDocument translationsDocument = XDocument.Load($"languages/{language}.xml");
                foreach (var node in translationsDocument.Descendants())
                {
                    string a = node.Name.ToString();
                    if ((node is XElement) && (node.Name == "entry"))
                    {
                        this.txt.Add(node.Attribute("id").Value, node.Value);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
