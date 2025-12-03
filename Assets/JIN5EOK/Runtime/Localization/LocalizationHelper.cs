#if USE_LOCALIZATION // 로컬라이제이션 임포트시에만 사용
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Jin5eok
{
    /// <summary>
    /// Unity Localization 패키지를 사용한 로컬라이제이션 작업을 간편하게 수행하기 위한 헬퍼 클래스입니다.
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// LocalizeStringEvent에 문자열 레퍼런스를 설정하고 새로고침합니다.
        /// </summary>
        /// <param name="localizedStringEvent">대상 LocalizeStringEvent</param>
        /// <param name="localizedString">설정할 LocalizedString</param>
        public static void SetStringReferenceAndRefresh(this LocalizeStringEvent localizedStringEvent, LocalizedString localizedString)
        {
            localizedStringEvent.StringReference = localizedString;
            localizedStringEvent.RefreshString();
        }
        
        /// <summary>
        /// Smart String 인자를 추가하고 새로고침합니다. 넘버링 기반일 경우 사용합니다.
        /// </summary>
        /// <param name="localizedString">대상 LocalizedString</param>
        /// <param name="arguments">추가할 인자들</param>
        public static void SetArgumentAndRefresh(this LocalizedString localizedString, params object[] arguments)
        {
            localizedString.Arguments = arguments;
            localizedString.RefreshString();
        }

        /// <summary>
        /// Smart String 인자를 Key-Value 쌍으로 추가하고 새로고침합니다. 넘버링 기반이 아닌 Key 기반일 경우 사용합니다.
        /// </summary>
        /// <param name="localizedString">대상 LocalizedString</param>
        /// <param name="arguments">추가할 Key-Value 쌍들</param>
        public static void SetKeyVariablePairAndRefresh(this LocalizedString localizedString, params (string key, IVariable value)[] arguments)
        {
            foreach (var (key, value) in arguments)
            {
                localizedString[key] = value;
            }
            
            localizedString.RefreshString();
        }
        
        /// <summary>
        /// 문자열 코드로 로케일을 찾아 반환합니다.
        /// </summary>
        /// <param name="localeCode">로케일 코드</param>
        /// <returns>해당하는 Locale, 없으면 null</returns>
        public static Locale GetLocaleByCode(string localeCode)
        {
            if (string.IsNullOrEmpty(localeCode)) return null;

            return LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(locale => locale.Identifier.Code == localeCode);
        }
    }
}
#endif