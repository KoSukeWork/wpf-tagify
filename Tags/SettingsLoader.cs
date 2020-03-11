using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using IniParser;
using IniParser.Model;

namespace Tags
{
    class SettingsLoader
    {
        private Dictionary<string, Dictionary<string, object>> options = new Dictionary<string, Dictionary<string, object>>()
        {
            { "modifier", new Dictionary<string, object>() {
                            { "none",   0x00 },
                            { "alt",    0x01 },
                            { "ctrl",   0x02 },
                            { "shift",  0x04 },
                            { "win",    0x08 } } },
            { "key", new Dictionary<string, object>() {
                            { "a",      0x41 },
                            { "b",      0x42 },
                            { "c",      0x43 },
                            { "d",      0x44 },
                            { "f10",    0x79 } } }
        };
        private Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            { "modifier",   0x00 },
            { "key",        0x79 },
        };

        private Dictionary<string, object> settings = null;

        public int hotkey_mod { get { return (int)defaults["modifier"]; } }
        public int hotkey_key { get { return (int)defaults["key"]; } }



        public SettingsLoader(string path = "settings.ini")
        {
            // Copying defaults to settings
            settings = new Dictionary<string, object>();
            foreach(var def in defaults)
                settings.Add(def.Key, def.Value);

            // Loading settings file
            if (!File.Exists(path))
                File.WriteAllText(path, "");
            var fileParser = new FileIniDataParser();
            IniData data = fileParser.ReadFile(path);

            // Initialize new ini with defaults
            if (!data.Sections.ContainsSection("settings"))
                data.Sections.AddSection("settings");
            
            // Synchronize options and defaults
            foreach(var setVal in defaults) {
                // Current default settings
                var sKey = setVal.Key.ToLower();
                var sVal = setVal.Value;

                // Current options. Can be null
                var opts = options[sKey];

                // Store value from defaults. (value in data is a key in opts)
                var defaultStoreValue = opts == null ? "" : opts.First(x => x.Value.Equals(sVal)).Key;

                // Option was not found in settings
                if(!data["settings"].ContainsKey(sKey)) {
                    data["settings"].AddKey(sKey, defaultStoreValue);
                    continue;
                }

                // Current data value
                var dVal = opts == null ? data["settings"][sKey] : data["settings"][sKey].ToLower();

                // Option has invalid value
                if(opts != null && !opts.ContainsKey(dVal)) {
                    data["settings"][sKey] = defaultStoreValue;
                    continue;
                }

                // Actual changing value from loaded data
                settings[sKey] = opts == null ? "" : opts[dVal];
            }

            // Save generated data
            fileParser.WriteFile(path, data);
        }
    }
}
