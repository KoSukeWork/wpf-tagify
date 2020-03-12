Tagify - is a WPF application, which allows to quickly rename selected in Windows Explorer files according to selected tag.

The source code for the main app is located in `Tags`.
All available hotkey combinations can be discovered from _Tags/SettingsLoader.cs_.

## Installation guide
1. Clone repository: `git clone https://github.com/ilyko96/wpf-tagify.git`
2. Open with Visual Studio
3. Install with NuGet package `ini-parser`
4. Add 2 additional references from COM category:
   - _Microsoft Internet Controls_
   - _Microsoft Shell Controls an Automation_
5. Build solution

## Getting started
* Run Tags.exe file once.
* It will generate settings file _settings.ini_ with one test tag.
* Open _settings.ini_ and add your tags at the end of this file.
* You can first start with the following test tags or directly add what you need:

```
[tag:frisbee]
tag = frisbee
text = Frisbee
color = #FFB3FFA0

[tag:crocodile]
tag = croco
text = Crocodile
color = #FFECF78F

[tag:tower]
tag = eifel
text = Eifel Tower
color = #FFB0A0FF
```