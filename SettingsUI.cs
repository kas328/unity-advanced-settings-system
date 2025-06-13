using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("그래픽 설정 UI")]
    [SerializeField] private GraphicsQualitySelector graphicsQualitySelector;

    [Header("월드 내 설정 UI")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider emoticonVolumeSlider;
    [SerializeField] private Slider squareVideoVolumeSlider;
    [SerializeField] private Slider myVoiceVolumeSlider;
    [SerializeField] private ToggleSwitch hapticToggle;
    [SerializeField] private ToggleSwitch emotionCameraFixedToggle;
    [SerializeField] private ToggleSwitch autoPlayVideoToggle;

    [Header("환경설정 화면 UI")]
    [SerializeField] private Slider environmentBgmSlider;
    [SerializeField] private Slider environmentSfxSlider;
    
    [Header("서버 설정 UI")]
    [SerializeField] private ToggleSwitch marketingInfoConsentToggle;

    [Header("푸시 알림 설정 UI")]
    [SerializeField] private ToggleSwitch pushNotificationsEnabledToggle;
    [SerializeField] private ToggleSwitch nightTimePushEnabledToggle;
    [SerializeField] private ToggleSwitch newFriendRequestPushEnabledToggle;
    [SerializeField] private ToggleSwitch gatheringPushEnabledToggle;
    [SerializeField] private ToggleSwitch easterEggPushEnabledToggle;
    [SerializeField] private ToggleSwitch agitChatPushEnabledToggle;
    [SerializeField] private ToggleSwitch friendGiftPushEnabledToggle;
    [SerializeField] private ToggleSwitch agitInvitePushEnabledToggle;
    [SerializeField] private ToggleSwitch liveBroadcastPushEnabledToggle;

    #region Life Cycle
    private void Start()
    {
        InitializeUI();
        SetupEventListeners();
    }

    private void OnDestroy()
    {
        RemoveEventListeners();
    }
    #endregion

    #region Initialization
    private void InitializeUI()
    {
        // 그래픽 설정 초기화
        graphicsQualitySelector.SetQualityMode(SettingsManager.Instance.GraphicsQualityMode, false);

        // 월드 내 설정 초기화
        bgmSlider.value = SettingsManager.Instance.BGMVolume;
        sfxSlider.value = SettingsManager.Instance.SfxVolume;
        emoticonVolumeSlider.value = SettingsManager.Instance.EmoticonVolume;
        squareVideoVolumeSlider.value = SettingsManager.Instance.SquareVideoVolume;
        myVoiceVolumeSlider.value = SettingsManager.Instance.MyVoiceVolume;
        hapticToggle.SetState(SettingsManager.Instance.HapticEnabled, false);
        emotionCameraFixedToggle.SetState(SettingsManager.Instance.EmotionCameraFixed, false);
        autoPlayVideoToggle.SetState(SettingsManager.Instance.AutoPlayVideo, false);

        // 환경설정 화면 UI 초기화
        environmentBgmSlider.value = SettingsManager.Instance.BGMVolume;
        environmentSfxSlider.value = SettingsManager.Instance.SfxVolume;
        
        // 서버 설정 초기화
        marketingInfoConsentToggle.SetState(SettingsManager.Instance.MarketingInfoConsent, false);

        // 푸시 알림 설정 초기화
        pushNotificationsEnabledToggle.SetState(SettingsManager.Instance.PushNotificationsEnabled, false);
        nightTimePushEnabledToggle.SetState(SettingsManager.Instance.NightTimePushEnabled, false);
        newFriendRequestPushEnabledToggle.SetState(SettingsManager.Instance.NewFriendRequestPushEnabled, false);
        gatheringPushEnabledToggle.SetState(SettingsManager.Instance.GatheringPushEnabled, false);
        easterEggPushEnabledToggle.SetState(SettingsManager.Instance.EasterEggPushEnabled, false);
        agitChatPushEnabledToggle.SetState(SettingsManager.Instance.AgitChatPushEnabled, false);
        friendGiftPushEnabledToggle.SetState(SettingsManager.Instance.FriendGiftPushEnabled, false);
        agitInvitePushEnabledToggle.SetState(SettingsManager.Instance.AgitInvitePushEnabled, false);
        liveBroadcastPushEnabledToggle.SetState(SettingsManager.Instance.LiveBroadcastPushEnabled, false);
        
        UpdatePushNotificationSubTogglesState(SettingsManager.Instance.PushNotificationsEnabled);
    }

    private void SetupEventListeners()
    {
        // 그래픽 설정 이벤트
        graphicsQualitySelector.onQualityModeChanged.AddListener(OnGraphicsQualityModeChanged);

        // 월드 내 설정 이벤트
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        emoticonVolumeSlider.onValueChanged.AddListener(OnEmoticonVolumeSliderChanged);
        squareVideoVolumeSlider.onValueChanged.AddListener(OnSquareVideoVolumeSliderChanged);
        myVoiceVolumeSlider.onValueChanged.AddListener(OnMyVoiceVolumeSliderChanged);
        hapticToggle.onValueChanged.AddListener(OnHapticToggleChanged);
        emotionCameraFixedToggle.onValueChanged.AddListener(OnEmotionCameraFixedToggleChanged);
        autoPlayVideoToggle.onValueChanged.AddListener(OnAutoPlayVideoToggleChanged);

        // 환경설정 화면 이벤트
        environmentBgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        environmentSfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        
        // 서버 설정 이벤트
        marketingInfoConsentToggle.onValueChanged.AddListener(OnMarketingInfoConsentToggleChanged);

        // 푸시 알림 설정 이벤트
        pushNotificationsEnabledToggle.onValueChanged.AddListener(OnPushNotificationsEnabledToggleChanged);
        nightTimePushEnabledToggle.onValueChanged.AddListener(OnNightTimePushEnabledToggleChanged);
        newFriendRequestPushEnabledToggle.onValueChanged.AddListener(OnNewFriendRequestPushEnabledToggleChanged);
        gatheringPushEnabledToggle.onValueChanged.AddListener(OnGatheringPushEnabledToggleChanged);
        easterEggPushEnabledToggle.onValueChanged.AddListener(OnEasterEggPushEnabledToggleChanged);
        agitChatPushEnabledToggle.onValueChanged.AddListener(OnAgitChatPushEnabledToggleChanged);
        friendGiftPushEnabledToggle.onValueChanged.AddListener(OnFriendGiftPushEnabledToggleChanged);
        agitInvitePushEnabledToggle.onValueChanged.AddListener(OnAgitInvitePushEnabledToggleChanged);
        liveBroadcastPushEnabledToggle.onValueChanged.AddListener(OnLiveBroadcastPushEnabledToggleChanged);

        // SettingsManager 이벤트 구독
        SubscribeToSettingsEvents();
    }

    private void RemoveEventListeners()
    {
        // 그래픽 설정 이벤트 해제
        graphicsQualitySelector.onQualityModeChanged.RemoveListener(OnGraphicsQualityModeChanged);

        // 월드 내 설정 이벤트 해제
        bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
        emoticonVolumeSlider.onValueChanged.RemoveListener(OnEmoticonVolumeSliderChanged);
        squareVideoVolumeSlider.onValueChanged.RemoveListener(OnSquareVideoVolumeSliderChanged);
        myVoiceVolumeSlider.onValueChanged.RemoveListener(OnMyVoiceVolumeSliderChanged);
        hapticToggle.onValueChanged.RemoveListener(OnHapticToggleChanged);
        emotionCameraFixedToggle.onValueChanged.RemoveListener(OnEmotionCameraFixedToggleChanged);
        autoPlayVideoToggle.onValueChanged.RemoveListener(OnAutoPlayVideoToggleChanged);

        // 환경설정 화면 이벤트 해제
        environmentBgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        environmentSfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
        
        // 서버 설정 이벤트 해제
        marketingInfoConsentToggle.onValueChanged.RemoveListener(OnMarketingInfoConsentToggleChanged);

        // 푸시 알림 설정 이벤트 해제
        pushNotificationsEnabledToggle.onValueChanged.RemoveListener(OnPushNotificationsEnabledToggleChanged);
        nightTimePushEnabledToggle.onValueChanged.RemoveListener(OnNightTimePushEnabledToggleChanged);
        newFriendRequestPushEnabledToggle.onValueChanged.RemoveListener(OnNewFriendRequestPushEnabledToggleChanged);
        gatheringPushEnabledToggle.onValueChanged.RemoveListener(OnGatheringPushEnabledToggleChanged);
        easterEggPushEnabledToggle.onValueChanged.RemoveListener(OnEasterEggPushEnabledToggleChanged);
        agitChatPushEnabledToggle.onValueChanged.RemoveListener(OnAgitChatPushEnabledToggleChanged);
        friendGiftPushEnabledToggle.onValueChanged.RemoveListener(OnFriendGiftPushEnabledToggleChanged);
        agitInvitePushEnabledToggle.onValueChanged.RemoveListener(OnAgitInvitePushEnabledToggleChanged);
        liveBroadcastPushEnabledToggle.onValueChanged.RemoveListener(OnLiveBroadcastPushEnabledToggleChanged);

        // SettingsManager 이벤트 구독 해제
        UnsubscribeFromSettingsEvents();
    }

    private void SubscribeToSettingsEvents()
    {
        if (SettingsManager.Instance != null)
        {
            // 그래픽 설정 이벤트
            SettingsManager.Instance.OnGraphicsQualityModeChanged += UpdateGraphicsQualityMode;

            // 월드 내 설정 이벤트
            SettingsManager.Instance.OnBGMVolumeChanged += UpdateBGMSlider;
            SettingsManager.Instance.OnSfxVolumeChanged += UpdateSfxSlider;
            SettingsManager.Instance.OnEmoticonVolumeChanged += UpdateEmoticonVolumeSlider;
            SettingsManager.Instance.OnSquareVideoVolumeChanged += UpdateSquareVideoVolumeSlider;
            SettingsManager.Instance.OnMyVoiceVolumeChanged += UpdateMyVoiceVolumeSlider;
            SettingsManager.Instance.OnHapticEnabledChanged += UpdateHapticToggle;
            SettingsManager.Instance.OnEmotionCameraFixedChanged += UpdateEmotionCameraFixedToggle;
            SettingsManager.Instance.OnAutoPlayVideoChanged += UpdateAutoPlayVideoToggle;

            // 서버 설정 이벤트
            SettingsManager.Instance.OnMarketingInfoConsentChanged += UpdateMarketingInfoConsentToggle;

            // 푸시 알림 설정 이벤트
            SettingsManager.Instance.OnPushNotificationsEnabledChanged += UpdatePushNotificationsEnabledToggle;
            SettingsManager.Instance.OnNightTimePushEnabledChanged += UpdateNightTimePushEnabledToggle;
            SettingsManager.Instance.OnNewFriendRequestPushEnabledChanged += UpdateNewFriendRequestPushEnabledToggle;
            SettingsManager.Instance.OnGatheringPushEnabledChanged += UpdateGatheringPushEnabledToggle;
            SettingsManager.Instance.OnEasterEggPushEnabledChanged += UpdateEasterEggPushEnabledToggle;
            SettingsManager.Instance.OnRoomChatPushEnabledChanged += UpdateRoomChatPushEnabledToggle;
            SettingsManager.Instance.OnFriendGiftPushEnabledChanged += UpdateFriendGiftPushEnabledToggle;
            SettingsManager.Instance.OnRoomInvitePushEnabledChanged += UpdateRoomInvitePushEnabledToggle;
            SettingsManager.Instance.OnLiveBroadcastPushEnabledChanged += UpdateLiveBroadcastPushEnabledToggle;
        }
    }

    private void UnsubscribeFromSettingsEvents()
    {
        if (SettingsManager.Instance != null)
        {
            // 그래픽 설정 이벤트 해제
            SettingsManager.Instance.OnGraphicsQualityModeChanged -= UpdateGraphicsQualityMode;

            // 월드 내 설정 이벤트 해제
            SettingsManager.Instance.OnBGMVolumeChanged -= UpdateBGMSlider;
            SettingsManager.Instance.OnSfxVolumeChanged -= UpdateSfxSlider;
            SettingsManager.Instance.OnEmoticonVolumeChanged -= UpdateEmoticonVolumeSlider;
            SettingsManager.Instance.OnSquareVideoVolumeChanged -= UpdateSquareVideoVolumeSlider;
            SettingsManager.Instance.OnMyVoiceVolumeChanged -= UpdateMyVoiceVolumeSlider;
            SettingsManager.Instance.OnHapticEnabledChanged -= UpdateHapticToggle;
            SettingsManager.Instance.OnEmotionCameraFixedChanged -= UpdateEmotionCameraFixedToggle;
            SettingsManager.Instance.OnAutoPlayVideoChanged -= UpdateAutoPlayVideoToggle;

            // 서버 설정 이벤트 해제
            SettingsManager.Instance.OnMarketingInfoConsentChanged -= UpdateMarketingInfoConsentToggle;

            // 푸시 알림 설정 이벤트 해제
            SettingsManager.Instance.OnPushNotificationsEnabledChanged -= UpdatePushNotificationsEnabledToggle;
            SettingsManager.Instance.OnNightTimePushEnabledChanged -= UpdateNightTimePushEnabledToggle;
            SettingsManager.Instance.OnNewFriendRequestPushEnabledChanged -= UpdateNewFriendRequestPushEnabledToggle;
            SettingsManager.Instance.OnGatheringPushEnabledChanged -= UpdateGatheringPushEnabledToggle;
            SettingsManager.Instance.OnEasterEggPushEnabledChanged -= UpdateEasterEggPushEnabledToggle;
            SettingsManager.Instance.OnRoomChatPushEnabledChanged -= UpdateRoomChatPushEnabledToggle;
            SettingsManager.Instance.OnFriendGiftPushEnabledChanged -= UpdateFriendGiftPushEnabledToggle;
            SettingsManager.Instance.OnRoomInvitePushEnabledChanged -= UpdateRoomInvitePushEnabledToggle;
            SettingsManager.Instance.OnLiveBroadcastPushEnabledChanged -= UpdateLiveBroadcastPushEnabledToggle;
        }
    }
    #endregion

    #region 그래픽 설정 이벤트 핸들러 
    // UI -> SettingsManager
    public void OnGraphicsQualityModeChanged(int value)
    {
        SettingsManager.Instance.GraphicsQualityMode = value;
    } 
    
    // SettingsManager -> UI
    private void UpdateGraphicsQualityMode(int value)
    {
        if (graphicsQualitySelector != null)
            graphicsQualitySelector.SetQualityMode(value, true);
    }
    #endregion

    #region 월드 내 설정 이벤트 핸들러
    // UI -> SettingsManager
    public void OnBGMSliderChanged(float value)
    {
        SettingsManager.Instance.BGMVolume = value;
    }

    public void OnSfxSliderChanged(float value)
    {
        SettingsManager.Instance.SfxVolume = value;
    }

    public void OnEmoticonVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.EmoticonVolume = value;
    }

    public void OnSquareVideoVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.SquareVideoVolume = value;
    }

    public void OnMyVoiceVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.MyVoiceVolume = value;
    }

    public void OnHapticToggleChanged(bool value)
    {
        SettingsManager.Instance.HapticEnabled = value;
    }

    public void OnEmotionCameraFixedToggleChanged(bool value)
    {
        SettingsManager.Instance.EmotionCameraFixed = value;
    }

    public void OnAutoPlayVideoToggleChanged(bool value)
    {
        SettingsManager.Instance.AutoPlayVideo = value;
    }

    // SettingsManager -> UI
    private void UpdateBGMSlider(float value)
    {
        if (bgmSlider != null)
            bgmSlider.SetValueWithoutNotify(value);
        if (environmentBgmSlider != null)
            environmentBgmSlider.SetValueWithoutNotify(value);
    }

    private void UpdateSfxSlider(float value)
    {
        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(value);
        if (environmentSfxSlider != null)
            environmentSfxSlider.SetValueWithoutNotify(value);
    }

    private void UpdateEmoticonVolumeSlider(float value)
    {
        if (emoticonVolumeSlider != null)
            emoticonVolumeSlider.SetValueWithoutNotify(value);
    }

    private void UpdateSquareVideoVolumeSlider(float value)
    {
        if (squareVideoVolumeSlider != null)
            squareVideoVolumeSlider.SetValueWithoutNotify(value);
    }

    private void UpdateMyVoiceVolumeSlider(float value)
    {
        if (myVoiceVolumeSlider != null)
            myVoiceVolumeSlider.SetValueWithoutNotify(value);
    }

    private void UpdateHapticToggle(bool value)
    {
        if (hapticToggle != null)
            hapticToggle.SetState(value);
    }

    private void UpdateEmotionCameraFixedToggle(bool value)
    {
        if (emotionCameraFixedToggle != null)
            emotionCameraFixedToggle.SetState(value);
    }

    private void UpdateAutoPlayVideoToggle(bool value)
    {
        if (autoPlayVideoToggle != null)
            autoPlayVideoToggle.SetState(value);
    }

    #endregion

    #region 서버 설정 이벤트 핸들러
    // UI -> SettingsManager
    public void OnMarketingInfoConsentToggleChanged(bool value)
    {
        SettingsManager.Instance.MarketingInfoConsent = value;
    }

    // SettingsManager -> UI
    private void UpdateMarketingInfoConsentToggle(bool value)
    {
        if (marketingInfoConsentToggle != null)
            marketingInfoConsentToggle.SetState(value);
    }

    #endregion

    #region 푸시 알림 설정 이벤트 핸들러
    // UI -> SettingsManager
    public void OnPushNotificationsEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.PushNotificationsEnabled = value;
        // 푸시 알림 마스터 토글이 꺼지면 모든 하위 토글도 비활성화
        UpdatePushNotificationSubTogglesState(value);
        
        if (value)
        {
            // 모든 하위 알림 설정 켜기
            SettingsManager.Instance.NightTimePushEnabled = true;
            SettingsManager.Instance.NewFriendRequestPushEnabled = true;
            SettingsManager.Instance.GatheringPushEnabled = true;
            SettingsManager.Instance.EasterEggPushEnabled = true;
            SettingsManager.Instance.AgitChatPushEnabled = true;
            SettingsManager.Instance.FriendGiftPushEnabled = true;
            SettingsManager.Instance.AgitInvitePushEnabled = true;
            SettingsManager.Instance.LiveBroadcastPushEnabled = true;
            SettingsManager.Instance.MarketingInfoConsent = true;
        }
    }

    private void UpdatePushNotificationSubTogglesState(bool enabled)
    {
        // UI 상호작용 상태 변경
        nightTimePushEnabledToggle.GetComponent<Button>().interactable = enabled;
        newFriendRequestPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        gatheringPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        easterEggPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        agitChatPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        friendGiftPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        agitInvitePushEnabledToggle.GetComponent<Button>().interactable = enabled;
        liveBroadcastPushEnabledToggle.GetComponent<Button>().interactable = enabled;
        marketingInfoConsentToggle.GetComponent<Button>().interactable = enabled;

        // 마스터 토글이 꺼지면 모든 하위 토글 값도 비활성화
        if (!enabled)
        {
            // 불필요한 이벤트 발생을 방지하기 위해 조건문 추가
            if (SettingsManager.Instance.NightTimePushEnabled)
                SettingsManager.Instance.NightTimePushEnabled = false;

            if (SettingsManager.Instance.NewFriendRequestPushEnabled)
                SettingsManager.Instance.NewFriendRequestPushEnabled = false;

            if (SettingsManager.Instance.GatheringPushEnabled)
                SettingsManager.Instance.GatheringPushEnabled = false;

            if (SettingsManager.Instance.EasterEggPushEnabled)
                SettingsManager.Instance.EasterEggPushEnabled = false;

            if (SettingsManager.Instance.AgitChatPushEnabled)
                SettingsManager.Instance.AgitChatPushEnabled = false;

            if (SettingsManager.Instance.FriendGiftPushEnabled)
                SettingsManager.Instance.FriendGiftPushEnabled = false;

            if (SettingsManager.Instance.AgitInvitePushEnabled)
                SettingsManager.Instance.AgitInvitePushEnabled = false;

            if (SettingsManager.Instance.LiveBroadcastPushEnabled)
                SettingsManager.Instance.LiveBroadcastPushEnabled = false;

            if (SettingsManager.Instance.MarketingInfoConsent)
                SettingsManager.Instance.MarketingInfoConsent = false;
        }
    }

    public void OnNightTimePushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.NightTimePushEnabled = value;
    }

    public void OnNewFriendRequestPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.NewFriendRequestPushEnabled = value;
    }

    public void OnGatheringPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.GatheringPushEnabled = value;
    }

    public void OnEasterEggPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.EasterEggPushEnabled = value;
    }

    public void OnAgitChatPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.AgitChatPushEnabled = value;
    }

    public void OnFriendGiftPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.FriendGiftPushEnabled = value;
    }

    public void OnAgitInvitePushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.AgitInvitePushEnabled = value;
    }

    public void OnLiveBroadcastPushEnabledToggleChanged(bool value)
    {
        SettingsManager.Instance.LiveBroadcastPushEnabled = value;
    }

    // SettingsManager -> UI
    private void UpdatePushNotificationsEnabledToggle(bool value)
    {
        if (pushNotificationsEnabledToggle == null) return;
        pushNotificationsEnabledToggle.SetState(value, false);
        UpdatePushNotificationSubTogglesState(value);
    }

    private void UpdateNightTimePushEnabledToggle(bool value)
    {
        if (nightTimePushEnabledToggle != null)
            nightTimePushEnabledToggle.SetState(value);
    }

    private void UpdateNewFriendRequestPushEnabledToggle(bool value)
    {
        if (newFriendRequestPushEnabledToggle != null)
            newFriendRequestPushEnabledToggle.SetState(value);
    }

    private void UpdateGatheringPushEnabledToggle(bool value)
    {
        if (gatheringPushEnabledToggle != null)
            gatheringPushEnabledToggle.SetState(value);
    }

    private void UpdateEasterEggPushEnabledToggle(bool value)
    {
        if (easterEggPushEnabledToggle != null)
            easterEggPushEnabledToggle.SetState(value);
    }

    private void UpdateRoomChatPushEnabledToggle(bool value)
    {
        if (agitChatPushEnabledToggle != null)
            agitChatPushEnabledToggle.SetState(value);
    }

    private void UpdateFriendGiftPushEnabledToggle(bool value)
    {
        if (friendGiftPushEnabledToggle != null)
            friendGiftPushEnabledToggle.SetState(value);
    }

    private void UpdateRoomInvitePushEnabledToggle(bool value)
    {
        if (agitInvitePushEnabledToggle != null)
            agitInvitePushEnabledToggle.SetState(value);
    }

    private void UpdateLiveBroadcastPushEnabledToggle(bool value)
    {
        if (liveBroadcastPushEnabledToggle != null)
            liveBroadcastPushEnabledToggle.SetState(value);
    }

    #endregion

    // 설정을 수동으로 저장하는 메서드
    public void SaveAllSettings()
    {
        SettingsManager.Instance.SaveSettings();
    }
}