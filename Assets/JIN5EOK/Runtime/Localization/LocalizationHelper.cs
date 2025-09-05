#if USE_LOCALIZATION // 로컬라이제이션 임포트시에만 사용
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Jin5eok
{
        
    public static class LocalizationHelper
    {
        // Value를 인자로 받는 생성자가 없어 확장 메서드로 대체함 (생성 + 값 지정을 한줄로 쓰기 위해)
        public static Variable<T> SetValue<T>(this Variable<T> variable, T value)
        {
            variable.Value = value;
            return variable;
        }
        
        public static void SetStringReferenceAndRefresh(this LocalizeStringEvent localizedStringEvent, LocalizedString localizedString)
        {
            localizedStringEvent.StringReference = localizedString;
            localizedStringEvent.RefreshString();
        }

        // smart string argument 추가, 넘버링 기반일 경우 사용
        public static void AddArgumentAndRefresh(this LocalizedString localizedString, params object[] arguments)
        {
            if (localizedString.Arguments == null)
            {
                localizedString.Arguments = arguments;
            }
            else
            {
                foreach (var argument in arguments)
                {
                    localizedString.Arguments.Add(argument);    
                }    
            }
            
            localizedString.RefreshString();
        }

        // smart string argument 추가, 넘버링 기반이 아닌 Key 기반일 경우 사용
        public static void AddKeyVariablePairAndRefresh(this LocalizedString localizedString, params (string key, IVariable value)[] arguments)
        {
            foreach (var argument in arguments)
            {
                localizedString.Add(argument.key, argument.value);
            }
            
            localizedString.RefreshString();
        }
        
        // 문자열 코드 -> 로케일 반환
        public static Locale GetLocaleByCode(string localeCode)
        {
            var locales = LocalizationSettings.AvailableLocales.Locales;
            foreach (var locale in locales)
            {
                if (locale.Identifier.Code == localeCode)
                {
                    return locale;
                }
            }
            return null;
        }
    }
}
#endif