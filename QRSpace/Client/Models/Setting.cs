using System.Collections.Generic;

namespace QRSpace.Client.Models
{
    public class Setting
    {
        private Dictionary<string,string> Settings { get; } = 
            new()
            {
                {"",""}
            };

        public bool TryAdd(string key, string value)
        {
            return Settings.TryAdd(key, value);
        }
        
        /// <summary>
        /// Get the setting value by the key
        /// </summary>
        /// <param name="key">The key of the setting</param>
        public string this[string key]
        {
            get
            {
                Settings.TryGetValue(key, out var result);
                return result;
            }
            set
            {
                if (key != null)
                {
                    Settings.TryAdd(key, value);
                }
            }
        }
    }
}