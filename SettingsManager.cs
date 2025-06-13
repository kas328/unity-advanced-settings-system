using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    #region 싱글톤
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null) Debug.Log("SettingsManager 인스턴스가 없습니다.");
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }
    #endregion

    #region 도우미 메서드
    // 음소거 상태 변경 시 볼륨을 처리하는 도우미 메서드
    private void HandleMuteStateChanged(bool muted, ref float volume, ref float lastVolume, Action<float> volumeChangedEvent, string volumeKey, string lastVolumeKey)
    {
        if (muted)
        {
            // 현재 볼륨이 0보다 크면 마지막 볼륨으로 저장
            if (volume > 0f)
            {
                lastVolume = volume;
                PlayerPrefs.SetFloat(lastVolumeKey, volume);
            }
        
            // 볼륨을 0으로 설정 (이벤트가 중복 발생하지 않도록 직접 값 변경)
            volume = 0f;
            volumeChangedEvent?.Invoke(0f);
            PlayerPrefs.SetFloat(volumeKey, 0f);
        }
        else if (volume <= 0f)
        {
            // 음소거 해제 시 마지막 볼륨으로 복원
            float newVolume = lastVolume > 0f ? lastVolume : 0.5f;
            volume = newVolume;
            volumeChangedEvent?.Invoke(newVolume);
            PlayerPrefs.SetFloat(volumeKey, newVolume);
        }
    }

    // 볼륨 변경 시 음소거 상태를 처리하는 도우미 메서드
    private void HandleVolumeChanged(float value, ref bool muted, Action<bool> mutedChangedEvent, string mutedKey, ref float lastVolume, string lastVolumeKey)
    {
        // 볼륨과 음소거 상태 동기화
        if (value <= 0f && !muted)
        {
            muted = true;
            mutedChangedEvent?.Invoke(true);
            PlayerPrefs.SetInt(mutedKey, 1);
        }
        else if (value > 0f && muted)
        {
            muted = false;
            mutedChangedEvent?.Invoke(false);
            PlayerPrefs.SetInt(mutedKey, 0);
        }
    
        // 볼륨이 0보다 크면 마지막 볼륨 값 저장
        if (value > 0f)
        {
            lastVolume = value;
            PlayerPrefs.SetFloat(lastVolumeKey, value);
        }
    }
    #endregion

    #region 오디오 마지막 볼륨 설정
    // 각 오디오 타입의 음소거 이전 마지막 볼륨 값
    private float _lastBGMVolume = 0.5f;
    private float _lastSfxVolume = 0.5f;
    private float _lastEmoticonVolume = 0.5f;
    private float _lastSquareVideoVolume = 0.5f;
    private float _lastMyVoiceVolume = 0.5f;

    public float LastBGMVolume
    {
        get => _lastBGMVolume;
        set
        {
            if (value > 0) {
                _lastBGMVolume = value;
                PlayerPrefs.SetFloat("LastBGMVolume", value);
                PlayerPrefs.Save();
            }
        }
    }

    public float LastSfxVolume
    {
        get => _lastSfxVolume;
        set
        {
            if (value > 0) {
                _lastSfxVolume = value;
                PlayerPrefs.SetFloat("LastSfxVolume", value);
                PlayerPrefs.Save();
            }
        }
    }

    public float LastEmoticonVolume
    {
        get => _lastEmoticonVolume;
        set
        {
            if (value > 0) {
                _lastEmoticonVolume = value;
                PlayerPrefs.SetFloat("LastEmoticonVolume", value);
                PlayerPrefs.Save();
            }
        }
    }

    public float LastSquareVideoVolume
    {
        get => _lastSquareVideoVolume;
        set
        {
            if (value > 0) {
                _lastSquareVideoVolume = value;
                PlayerPrefs.SetFloat("LastSquareVideoVolume", value);
                PlayerPrefs.Save();
            }
        }
    }

    public float LastMyVoiceVolume
    {
        get => _lastMyVoiceVolume;
        set
        {
            if (value > 0) {
                _lastMyVoiceVolume = value;
                PlayerPrefs.SetFloat("LastMyVoiceVolume", value);
                PlayerPrefs.Save();
            }
        }
    }
    #endregion
    
    #region 그래픽 설정
    // 0: 성능 우선, 1: 균형잡힌, 2: 최고 품질
    private int _graphicsQualityMode = 1;
    public int GraphicsQualityMode
    {
        get => _graphicsQualityMode;
        set
        {
            _graphicsQualityMode = value;
            OnGraphicsQualityModeChanged?.Invoke(value);
            PlayerPrefs.SetInt("GraphicsQualityMode", value);
            PlayerPrefs.Save();
            
            // Unity 품질 설정 변경
            QualitySettings.SetQualityLevel(value, true);
        }
    }
    public event Action<int> OnGraphicsQualityModeChanged;
    
    #endregion

    #region 월드 내 설정
    // 배경음악 설정
    private float _bgmVolume = 1f;
    public float BGMVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = value;
            OnBGMVolumeChanged?.Invoke(value);
            PlayerPrefs.SetFloat("BGMVolume", value);
            
            HandleVolumeChanged(value, ref _bgmMuted, OnBGMMutedChanged, "BGMMuted", 
                               ref _lastBGMVolume, "LastBGMVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<float> OnBGMVolumeChanged;
    
    // 배경음악 음소거 상태
    private bool _bgmMuted = false;
    public bool BGMMuted
    {
        get => _bgmMuted;
        set
        {
            _bgmMuted = value;
            OnBGMMutedChanged?.Invoke(value);
            PlayerPrefs.SetInt("BGMMuted", value ? 1 : 0);
            
            HandleMuteStateChanged(value, ref _bgmVolume, ref _lastBGMVolume, 
                                  OnBGMVolumeChanged, "BGMVolume", "LastBGMVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnBGMMutedChanged;

    // 효과음 설정
    private float _sfxVolume = 1f;
    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            OnSfxVolumeChanged?.Invoke(value);
            PlayerPrefs.SetFloat("SFXVolume", value);
            
            HandleVolumeChanged(value, ref _sfxMuted, OnSfxMutedChanged, "SFXMuted", 
                               ref _lastSfxVolume, "LastSfxVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<float> OnSfxVolumeChanged;

    // 효과음 음소거 상태
    private bool _sfxMuted = false;
    public bool SfxMuted
    {
        get => _sfxMuted;
        set
        {
            _sfxMuted = value;
            OnSfxMutedChanged?.Invoke(value);
            PlayerPrefs.SetInt("SFXMuted", value ? 1 : 0);
            
            HandleMuteStateChanged(value, ref _sfxVolume, ref _lastSfxVolume, 
                                  OnSfxVolumeChanged, "SFXVolume", "LastSfxVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnSfxMutedChanged;
    
    // 이모티콘 사운드 설정
    private float _emoticonVolume = 1f;
    public float EmoticonVolume
    {
        get => _emoticonVolume;
        set
        {
            _emoticonVolume = value;
            OnEmoticonVolumeChanged?.Invoke(value);
            PlayerPrefs.SetFloat("EmoticonVolume", value);
            
            HandleVolumeChanged(value, ref _emoticonMuted, OnEmoticonMutedChanged, "EmoticonMuted", 
                               ref _lastEmoticonVolume, "LastEmoticonVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<float> OnEmoticonVolumeChanged;

    // 이모티콘 사운드 음소거 상태
    private bool _emoticonMuted = false;
    public bool EmoticonMuted
    {
        get => _emoticonMuted;
        set
        {
            _emoticonMuted = value;
            OnEmoticonMutedChanged?.Invoke(value);
            PlayerPrefs.SetInt("EmoticonMuted", value ? 1 : 0);
            
            HandleMuteStateChanged(value, ref _emoticonVolume, ref _lastEmoticonVolume, 
                                  OnEmoticonVolumeChanged, "EmoticonVolume", "LastEmoticonVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnEmoticonMutedChanged;
    
    // 광장 영상 사운드 설정
    private float _squareVideoVolume = 1f;
    public float SquareVideoVolume
    {
        get => _squareVideoVolume;
        set
        {
            _squareVideoVolume = value;
            OnSquareVideoVolumeChanged?.Invoke(value);
            PlayerPrefs.SetFloat("SquareVideoVolume", value);
            
            HandleVolumeChanged(value, ref _squareVideoMuted, OnSquareVideoMutedChanged, "SquareVideoMuted", 
                               ref _lastSquareVideoVolume, "LastSquareVideoVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<float> OnSquareVideoVolumeChanged;

    // 광장 영상 사운드 음소거 상태
    private bool _squareVideoMuted = false;
    public bool SquareVideoMuted
    {
        get => _squareVideoMuted;
        set
        {
            _squareVideoMuted = value;
            OnSquareVideoMutedChanged?.Invoke(value);
            PlayerPrefs.SetInt("SquareVideoMuted", value ? 1 : 0);
            
            HandleMuteStateChanged(value, ref _squareVideoVolume, ref _lastSquareVideoVolume, 
                                  OnSquareVideoVolumeChanged, "SquareVideoVolume", "LastSquareVideoVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnSquareVideoMutedChanged;
    
    // 내 보이스 크기 설정
    private float _myVoiceVolume = 1f;
    public float MyVoiceVolume
    {
        get => _myVoiceVolume;
        set
        {
            _myVoiceVolume = value;
            OnMyVoiceVolumeChanged?.Invoke(value);
            PlayerPrefs.SetFloat("MyVoiceVolume", value);
            
            HandleVolumeChanged(value, ref _myVoiceMuted, OnMyVoiceMutedChanged, "MyVoiceMuted", 
                               ref _lastMyVoiceVolume, "LastMyVoiceVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<float> OnMyVoiceVolumeChanged;
    
    // 내 보이스 음소거 상태
    private bool _myVoiceMuted = false;
    public bool MyVoiceMuted
    {
        get => _myVoiceMuted;
        set
        {
            _myVoiceMuted = value;
            OnMyVoiceMutedChanged?.Invoke(value);
            PlayerPrefs.SetInt("MyVoiceMuted", value ? 1 : 0);
            
            HandleMuteStateChanged(value, ref _myVoiceVolume, ref _lastMyVoiceVolume, 
                                  OnMyVoiceVolumeChanged, "MyVoiceVolume", "LastMyVoiceVolume");
            
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnMyVoiceMutedChanged;

    // 햅틱 진동 설정
    private bool _hapticEnabled = true;
    public bool HapticEnabled
    {
        get => _hapticEnabled;
        set
        {
            _hapticEnabled = value;
            OnHapticEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("HapticEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnHapticEnabledChanged;

    // 이모티콘 재생시 카메라 시점 고정
    private bool _emotionCameraFixed = true;
    public bool EmotionCameraFixed
    {
        get => _emotionCameraFixed;
        set
        {
            _emotionCameraFixed = value;
            OnEmotionCameraFixedChanged?.Invoke(value);
            PlayerPrefs.SetInt("EmotionCameraFixed", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnEmotionCameraFixedChanged;

    // 영상 자동 재생
    private bool _autoPlayVideo = true;
    public bool AutoPlayVideo
    {
        get => _autoPlayVideo;
        set
        {
            _autoPlayVideo = value;
            OnAutoPlayVideoChanged?.Invoke(value);
            PlayerPrefs.SetInt("AutoPlayVideo", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnAutoPlayVideoChanged;

    #endregion

    #region 서버 설정
    // 마케팅 정보 수신동의
    private bool _marketingInfoConsent = false;
    public bool MarketingInfoConsent
    {
        get => _marketingInfoConsent;
        set
        {
            _marketingInfoConsent = value;
            OnMarketingInfoConsentChanged?.Invoke(value);
            PlayerPrefs.SetInt("MarketingInfoConsent", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnMarketingInfoConsentChanged;
    #endregion

    #region 푸시 알림 설정
    // 푸시 알림 사용
    private bool _pushNotificationsEnabled = true;
    public bool PushNotificationsEnabled
    {
        get => _pushNotificationsEnabled;
        set
        {
            _pushNotificationsEnabled = value;
            OnPushNotificationsEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("PushNotificationsEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnPushNotificationsEnabledChanged;

    // 야간 시간 푸시 알림 수신
    private bool _nightTimePushEnabled = false;
    public bool NightTimePushEnabled
    {
        get => _nightTimePushEnabled;
        set
        {
            _nightTimePushEnabled = value;
            OnNightTimePushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("NightTimePushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnNightTimePushEnabledChanged;

    // 새로운 친구 신청
    private bool _newFriendRequestPushEnabled = true;
    public bool NewFriendRequestPushEnabled
    {
        get => _newFriendRequestPushEnabled;
        set
        {
            _newFriendRequestPushEnabled = value;
            OnNewFriendRequestPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("NewFriendRequestPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnNewFriendRequestPushEnabledChanged;

    // 소집 알림
    private bool _gatheringPushEnabled = true;
    public bool GatheringPushEnabled
    {
        get => _gatheringPushEnabled;
        set
        {
            _gatheringPushEnabled = value;
            OnGatheringPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("GatheringPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnGatheringPushEnabledChanged;

    // 누군가 내 아지트에 이스터에그를 숨겼을 때
    private bool _easterEggPushEnabled = true;
    public bool EasterEggPushEnabled
    {
        get => _easterEggPushEnabled;
        set
        {
            _easterEggPushEnabled = value;
            OnEasterEggPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("EasterEggPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnEasterEggPushEnabledChanged;

    // 아지트 내 채팅
    private bool _agitChatPushEnabled = true;
    public bool AgitChatPushEnabled
    {
        get => _agitChatPushEnabled;
        set
        {
            _agitChatPushEnabled = value;
            OnRoomChatPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("RoomChatPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnRoomChatPushEnabledChanged;

    // 친구가 선물을 보냈을 때 알림 설정
    private bool _friendGiftPushEnabled = true;
    public bool FriendGiftPushEnabled
    {
        get => _friendGiftPushEnabled;
        set
        {
            _friendGiftPushEnabled = value;
            OnFriendGiftPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("FriendGiftPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnFriendGiftPushEnabledChanged;

    // 아지트에 초대받았을 때 알림 설정
    private bool _agitInvitePushEnabled = true;
    public bool AgitInvitePushEnabled
    {
        get => _agitInvitePushEnabled;
        set
        {
            _agitInvitePushEnabled = value;
            OnRoomInvitePushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("RoomInvitePushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnRoomInvitePushEnabledChanged;

    // 내 아지트에서 노래/라이브 방송 시작했을 때 알림 설정
    private bool _liveBroadcastPushEnabled = true;
    public bool LiveBroadcastPushEnabled
    {
        get => _liveBroadcastPushEnabled;
        set
        {
            _liveBroadcastPushEnabled = value;
            OnLiveBroadcastPushEnabledChanged?.Invoke(value);
            PlayerPrefs.SetInt("LiveBroadcastPushEnabled", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public event Action<bool> OnLiveBroadcastPushEnabledChanged;
    #endregion
    
    #region 설정 저장 및 로드
    public void SaveSettings()
    {
        // 그래픽 설정
        PlayerPrefs.SetInt("GraphicsQualityMode", _graphicsQualityMode);
        
        // 월드 내 설정
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetInt("BGMMuted", _bgmMuted ? 1 : 0);
        PlayerPrefs.SetFloat("LastBGMVolume", _lastBGMVolume);
        
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetInt("SFXMuted", _sfxMuted ? 1 : 0);
        PlayerPrefs.SetFloat("LastSfxVolume", _lastSfxVolume);
        
        PlayerPrefs.SetFloat("EmoticonVolume", _emoticonVolume);
        PlayerPrefs.SetInt("EmoticonMuted", _emoticonMuted ? 1 : 0);
        PlayerPrefs.SetFloat("LastEmoticonVolume", _lastEmoticonVolume);
        
        PlayerPrefs.SetFloat("SquareVideoVolume", _squareVideoVolume);
        PlayerPrefs.SetInt("SquareVideoMuted", _squareVideoMuted ? 1 : 0);
        PlayerPrefs.SetFloat("LastSquareVideoVolume", _lastSquareVideoVolume);
        
        PlayerPrefs.SetFloat("MyVoiceVolume", _myVoiceVolume);
        PlayerPrefs.SetInt("MyVoiceMuted", _myVoiceMuted ? 1 : 0);
        PlayerPrefs.SetFloat("LastMyVoiceVolume", _lastMyVoiceVolume);
        
        PlayerPrefs.SetInt("HapticEnabled", _hapticEnabled ? 1 : 0);
        PlayerPrefs.SetInt("EmotionCameraFixed", _emotionCameraFixed ? 1 : 0);
        PlayerPrefs.SetInt("AutoPlayVideo", _autoPlayVideo ? 1 : 0);

        // 서버 설정
        PlayerPrefs.SetInt("MarketingInfoConsent", _marketingInfoConsent ? 1 : 0);

        // 푸시 알림 설정
        PlayerPrefs.SetInt("PushNotificationsEnabled", _pushNotificationsEnabled ? 1 : 0);
        PlayerPrefs.SetInt("NightTimePushEnabled", _nightTimePushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("NewFriendRequestPushEnabled", _newFriendRequestPushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("GatheringPushEnabled", _gatheringPushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("EasterEggPushEnabled", _easterEggPushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("RoomChatPushEnabled", _agitChatPushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("FriendGiftPushEnabled", _friendGiftPushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("RoomInvitePushEnabled", _agitInvitePushEnabled ? 1 : 0);
        PlayerPrefs.SetInt("LiveBroadcastPushEnabled", _liveBroadcastPushEnabled ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        // 마지막 볼륨 값 로드 (먼저 로드해야 함)
        _lastBGMVolume = PlayerPrefs.GetFloat("LastBGMVolume", 0.5f);
        _lastSfxVolume = PlayerPrefs.GetFloat("LastSfxVolume", 0.5f);
        _lastEmoticonVolume = PlayerPrefs.GetFloat("LastEmoticonVolume", 0.5f);
        _lastSquareVideoVolume = PlayerPrefs.GetFloat("LastSquareVideoVolume", 0.5f);
        _lastMyVoiceVolume = PlayerPrefs.GetFloat("LastMyVoiceVolume", 0.5f);
        
        // 그래픽 설정
        GraphicsQualityMode = PlayerPrefs.GetInt("GraphicsQualityMode", 1);
        
        // 음소거 상태 먼저 로드
        _bgmMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        _sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        _emoticonMuted = PlayerPrefs.GetInt("EmoticonMuted", 0) == 1;
        _squareVideoMuted = PlayerPrefs.GetInt("SquareVideoMuted", 0) == 1;
        _myVoiceMuted = PlayerPrefs.GetInt("MyVoiceMuted", 0) == 1;
        
        // 볼륨 로드
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        EmoticonVolume = PlayerPrefs.GetFloat("EmoticonVolume", 1f);
        SquareVideoVolume = PlayerPrefs.GetFloat("SquareVideoVolume", 1f);
        MyVoiceVolume = PlayerPrefs.GetFloat("MyVoiceVolume", 1f);
        
        // 음소거 상태와 볼륨이 일치하도록 확인
        if (_bgmMuted && _bgmVolume > 0f)
            BGMVolume = 0f;
        else if (!_bgmMuted && _bgmVolume <= 0f)
            BGMMuted = true;
            
        if (_sfxMuted && _sfxVolume > 0f)
            SfxVolume = 0f;
        else if (!_sfxMuted && _sfxVolume <= 0f)
            SfxMuted = true;
            
        if (_emoticonMuted && _emoticonVolume > 0f)
            EmoticonVolume = 0f;
        else if (!_emoticonMuted && _emoticonVolume <= 0f)
            EmoticonMuted = true;
            
        if (_squareVideoMuted && _squareVideoVolume > 0f)
            SquareVideoVolume = 0f;
        else if (!_squareVideoMuted && _squareVideoVolume <= 0f)
            SquareVideoMuted = true;
            
        if (_myVoiceMuted && _myVoiceVolume > 0f)
            MyVoiceVolume = 0f;
        else if (!_myVoiceMuted && _myVoiceVolume <= 0f)
            MyVoiceMuted = true;
            
        // 기타 설정 로드
        HapticEnabled = PlayerPrefs.GetInt("HapticEnabled", 1) == 1;
        EmotionCameraFixed = PlayerPrefs.GetInt("EmotionCameraFixed", 1) == 1;
        AutoPlayVideo = PlayerPrefs.GetInt("AutoPlayVideo", 1) == 1;

        // 서버 설정
        MarketingInfoConsent = PlayerPrefs.GetInt("MarketingInfoConsent", 0) == 1;

        // 푸시 알림 설정
        PushNotificationsEnabled = PlayerPrefs.GetInt("PushNotificationsEnabled", 1) == 1;
        NightTimePushEnabled = PlayerPrefs.GetInt("NightTimePushEnabled", 0) == 1;
        NewFriendRequestPushEnabled = PlayerPrefs.GetInt("NewFriendRequestPushEnabled", 1) == 1;
        GatheringPushEnabled = PlayerPrefs.GetInt("GatheringPushEnabled", 1) == 1;
        EasterEggPushEnabled = PlayerPrefs.GetInt("EasterEggPushEnabled", 1) == 1;
        AgitChatPushEnabled = PlayerPrefs.GetInt("RoomChatPushEnabled", 1) == 1;
        FriendGiftPushEnabled = PlayerPrefs.GetInt("FriendGiftPushEnabled", 1) == 1;
        AgitInvitePushEnabled = PlayerPrefs.GetInt("RoomInvitePushEnabled", 1) == 1;
        LiveBroadcastPushEnabled = PlayerPrefs.GetInt("LiveBroadcastPushEnabled", 1) == 1;
    }
    #endregion

    #region ClearMethod
    public void ClearAllEvents()
    {
        // 그래픽 설정 이벤트 정리
        OnGraphicsQualityModeChanged = null;
    
        // 월드 내 설정 이벤트 정리
        OnBGMVolumeChanged = null;
        OnBGMMutedChanged = null;
        OnSfxVolumeChanged = null;
        OnSfxMutedChanged = null;
        OnEmoticonVolumeChanged = null;
        OnEmoticonMutedChanged = null;
        OnSquareVideoVolumeChanged = null;
        OnSquareVideoMutedChanged = null;
        OnMyVoiceVolumeChanged = null;
        OnMyVoiceMutedChanged = null;
        OnHapticEnabledChanged = null;
        OnEmotionCameraFixedChanged = null;
        OnAutoPlayVideoChanged = null;
    
        // 서버 설정 이벤트 정리
        OnMarketingInfoConsentChanged = null;
    
        // 푸시 알림 설정 이벤트 정리
        OnPushNotificationsEnabledChanged = null;
        OnNightTimePushEnabledChanged = null;
        OnNewFriendRequestPushEnabledChanged = null;
        OnGatheringPushEnabledChanged = null;
        OnEasterEggPushEnabledChanged = null;
        OnRoomChatPushEnabledChanged = null;
        OnFriendGiftPushEnabledChanged = null;
        OnRoomInvitePushEnabledChanged = null;
        OnLiveBroadcastPushEnabledChanged = null;
    
        Debug.Log("SettingsManager의 모든 이벤트가 정리되었습니다.");
    }
    #endregion
}