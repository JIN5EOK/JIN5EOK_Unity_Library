#if USE_LOCALIZATION // 로컬라이제이션 임포트시에만 사용
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Jin5eok
{
    public static class LocalizationHelper
    {
        public static void SetStringReferenceAndRefresh(this LocalizeStringEvent localizedStringEvent, LocalizedString localizedString)
        {
            localizedStringEvent.StringReference = localizedString;
            localizedStringEvent.RefreshString();
        }
        
        // smart string argument 추가, 넘버링 기반일 경우 사용
        public static void SetArgumentAndRefresh(this LocalizedString localizedString, params object[] arguments)
        {
            localizedString.Arguments = arguments;
            localizedString.RefreshString();
        }

        // smart string argument 추가, 넘버링 기반이 아닌 Key 기반일 경우 사용
        public static void SetKeyVariablePairAndRefresh(this LocalizedString localizedString, params (string key, IVariable value)[] arguments)
        {
            foreach (var (key, value) in arguments)
            {
                localizedString[key] = value;
            }
            
            localizedString.RefreshString();
        }
        
        // 문자열 코드 -> 로케일 반환
        public static Locale GetLocaleByCode(string localeCode)
        {
            if (string.IsNullOrEmpty(localeCode)) return null;

            return LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(locale => locale.Identifier.Code == localeCode);
        }
    }
}
#endif