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
    class TagSettings {
        public string Tag { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        public TagSettings (string tag, string text = null, string color = null) {
            Tag = tag;
            Text = text == null ? tag : text;
            Color = color == null ? "#000000" : color;
        }
    }
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
                            { "backspace",      0x08 },
                            { "tab",            0x09 },
                            { "enter",          0x0D },
                            { "return",         0x0D },
                            { "pause",          0x13 },
                            { "caps",           0x14 },
                            { "escape",         0x1B },
                            { "space",          0x20 },
                            { "pageup",         0x21 },
                            { "pagedown",       0x22 },
                            { "end",            0x23 },
                            { "home",           0x24 },
                            { "leftarrow",      0x25 },
                            { "uparrow",        0x26 },
                            { "rightarrow",     0x27 },
                            { "downarrow",      0x28 },
                            { "printscreen",    0x2C },
                            { "insert",         0x2D },
                            { "delete",         0x2E },
                            { "0",              0x30 },
                            { "1",              0x31 },
                            { "2",              0x32 },
                            { "3",              0x33 },
                            { "4",              0x34 },
                            { "5",              0x35 },
                            { "6",              0x36 },
                            { "7",              0x37 },
                            { "8",              0x38 },
                            { "9",              0x39 },
                            { "a",              0x41 },
                            { "b",              0x42 },
                            { "c",              0x43 },
                            { "d",              0x44 },
                            { "e",              0x45 },
                            { "f",              0x46 },
                            { "g",              0x47 },
                            { "h",              0x48 },
                            { "i",              0x49 },
                            { "j",              0x4A },
                            { "k",              0x4B },
                            { "l",              0x4C },
                            { "m",              0x4D },
                            { "n",              0x4E },
                            { "o",              0x4F },
                            { "p",              0x50 },
                            { "q",              0x51 },
                            { "r",              0x52 },
                            { "s",              0x53 },
                            { "t",              0x54 },
                            { "u",              0x55 },
                            { "v",              0x56 },
                            { "w",              0x57 },
                            { "x",              0x58 },
                            { "y",              0x59 },
                            { "z",              0x5A },
                            { "win",            0x5B },
                            { "winleft",        0x5B },
                            { "winright",       0x5C },
                            { "num0",           0x60 },
                            { "num1",           0x61 },
                            { "num2",           0x62 },
                            { "num3",           0x63 },
                            { "num4",           0x64 },
                            { "num5",           0x65 },
                            { "num6",           0x66 },
                            { "num7",           0x67 },
                            { "num8",           0x68 },
                            { "num9",           0x69 },
                            { "*",              0x6A },
                            { "+",              0x6B },
                            { "-",              0x6D },
                            { ".",              0x6E },
                            { "/",              0x6F },
                            { "f1",             0x70 },
                            { "f2",             0x71 },
                            { "f3",             0x72 },
                            { "f4",             0x73 },
                            { "f5",             0x74 },
                            { "f6",             0x75 },
                            { "f7",             0x76 },
                            { "f8",             0x77 },
                            { "f9",             0x78 },
                            { "f10",            0x79 },
                            { "f11",            0x7A },
                            { "f12",            0x7B },
                            { "f13",            0x7C },
                            { "f14",            0x7D },
                            { "f15",            0x7E },
                            { "f16",            0x7F },
                            { "f17",            0x80 },
                            { "f18",            0x81 },
                            { "f19",            0x82 },
                            { "f20",            0x83 },
                            { "f21",            0x84 },
                            { "f22",            0x85 },
                            { "f23",            0x86 },
                            { "f24",            0x87 } } },
            { "hideonclick", new Dictionary<string, object>() {
                            { "true",   true },
                            { "false",  false } } },
            { "width", null },
            { "height", null },
            { "tooglevisibility", new Dictionary<string, object>() {
                            { "true",   true },
                            { "false",  false } } },
            { "movetocursor", new Dictionary<string, object>() {
                            { "true",   true },
                            { "false",  false } } },
        };
        private Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            { "modifier",           0x00 },
            { "key",                0x79 },
            { "hideonclick",        true },
            { "width",              256 },
            { "height",             256 },
            { "tooglevisibility",    true },
            { "movetocursor",    true },
        };

        private Dictionary<string, object> settings = null;


        public List<TagSettings> Tags { get; set; }

        public int hotkey_mod { get { return (int)settings["modifier"]; } }
        public int hotkey_key { get { return (int)settings["key"]; } }
        public bool hideOnClick { get { return (bool)settings["hideonclick"]; } }
        public bool toogleVisibility { get { return (bool)settings["tooglevisibility"]; } }
        public bool moveToCursor { get { return (bool)settings["movetocursor"]; } }

        public int width { get { return int.Parse(settings["width"].ToString()); } }
        public int height { get { return int.Parse(settings["height"].ToString()); } }


        public SettingsLoader(string path = "settings.ini")
        {
            Tags = new List<TagSettings>();

            // Copying defaults to settings
            settings = new Dictionary<string, object>();
            foreach (var def in defaults)
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
                var defaultStoreValue = opts == null ? defaults[sKey].ToString() : opts.First(x => x.Value.Equals(sVal)).Key;

                // Option was not found in settings
                if (!data["settings"].ContainsKey(sKey)) {
                    data["settings"].AddKey(sKey, defaultStoreValue);
                    continue;
                }

                // Current data value
                var dVal = opts == null ? data["settings"][sKey] : data["settings"][sKey].ToLower();
                dVal = dVal.Trim();

                // Option has invalid value
                if (opts != null && !opts.ContainsKey(dVal)) {
                    data["settings"][sKey] = defaultStoreValue;
                    continue;
                }

                // Actual changing value from loaded data
                settings[sKey] = opts == null ? dVal : opts[dVal];
            }


            // Get tags from settings file
            foreach(var section in data.Sections) {
                var sname = section.SectionName.ToLower();

                // Filter all non-tag data
                if (!sname.StartsWith("tag:"))
                    continue;

                // Section name without leading 'tag:'
                string sectionName = sname.Substring("tag:".Length);

                // Check if keys are present
                if (!data[sname].ContainsKey("tag"))
                    data[sname].AddKey("tag", sectionName);
                if (!data[sname].ContainsKey("text"))
                    data[sname].AddKey("text", data[sname]["tag"]);
                if (!data[sname].ContainsKey("color"))
                    data[sname].AddKey("tag", "#ffffff");

                Tags.Add(new TagSettings(data[sname]["tag"],
                                         data[sname]["text"],
                                         data[sname]["color"]));
            }


            // Save generated data
            fileParser.WriteFile(path, data);
        }
    }
}
