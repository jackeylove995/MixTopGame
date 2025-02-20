using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace ZLuaFramework
{
    public class LocalizationManager
    {
        static string playerPrefKey = "Localization_CurrentLanguage";
        static string tableName = "Common";

        public static string GetString(string key)
        {
            return LocalizationSettings.StringDatabase.GetTableEntry(tableName, key, LocalizationSettings.SelectedLocale).Entry.Value;           
        }

        public static void ChangeLanguage(string languageCode)
        {
            ChangeLanguageInternal(new LocaleIdentifier(languageCode));
        }
        public static void ChangeLanguage(SystemLanguage language)
        {
            ChangeLanguageInternal(new LocaleIdentifier(language));            
        }

        static void ChangeLanguageInternal(LocaleIdentifier localeIdentifier)
        {
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (locale.Identifier == localeIdentifier)
                {
                    PlayerPrefs.SetString(playerPrefKey, locale.Identifier.Code);
                    LocalizationSettings.SelectedLocale = locale;                                     
                }
            }
        }

        public static string GetCurrentUsingLanguage()
        {
            return LocalizationSettings.SelectedLocale.LocaleName;
        }       
    }

}
