using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Loudspeaker.Services;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.ApiClients;

/// <summary>
/// HTTP message handler that automatically adds the pxtoken header to all requests
/// </summary>
public class PxtokenHandler : DelegatingHandler
{
    private readonly CarbonVoiceAuthService _authService;

    public PxtokenHandler(CarbonVoiceAuthService authService)
    {
        _authService = authService;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var pxtoken = _authService.GetPxtoken();
        if (!string.IsNullOrEmpty(pxtoken))
        {
            request.Headers.Add("pxtoken", pxtoken);
        }
        else
        {
            Log("[PxtokenHandler] WARNING: No pxtoken available for request");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

/// <summary>
/// Factory class for creating authenticated CarbonVoice API clients
/// </summary>
public class AuthenticatedCarbonVoiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AuthenticatedCarbonVoiceClient(CarbonVoiceAuthService authService, string baseUrl)
    {
        _baseUrl = baseUrl;
        var handler = new PxtokenHandler(authService);
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    /// <summary>
    /// Initializes instance-level JsonSerializerSettings for a client so it can be configured independently.
    /// This allows runtime configuration like: client.JsonSerializerSettings.Converters.Add(...)
    /// </summary>
    private T InitializeClientSettings<T>(T client) where T : class
    {
        var clientType = typeof(T);
        var instanceSettingsField = clientType.GetField("_instanceSettings", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (instanceSettingsField != null)
        {
            // Get the static settings to use as a template
            var staticSettingsField = clientType.GetField("_settings", BindingFlags.NonPublic | BindingFlags.Static);
            if (staticSettingsField != null)
            {
                var staticSettings = staticSettingsField.GetValue(null) as Lazy<JsonSerializerSettings>;
                if (staticSettings != null)
                {
                    // Create a new instance settings based on the static settings
                    // Copy all important properties to ensure consistency
                    var staticSettingsValue = staticSettings.Value;
                    var instanceSettings = new JsonSerializerSettings
                    {
                        // Copy converters (this is what the user wants to modify)
                        Converters = new System.Collections.Generic.List<JsonConverter>(staticSettingsValue.Converters),
                        // Copy other important settings
                        ContractResolver = staticSettingsValue.ContractResolver,
                        DateFormatHandling = staticSettingsValue.DateFormatHandling,
                        DateTimeZoneHandling = staticSettingsValue.DateTimeZoneHandling,
                        DefaultValueHandling = staticSettingsValue.DefaultValueHandling,
                        Formatting = staticSettingsValue.Formatting,
                        MaxDepth = staticSettingsValue.MaxDepth,
                        MissingMemberHandling = staticSettingsValue.MissingMemberHandling,
                        NullValueHandling = staticSettingsValue.NullValueHandling,
                        ObjectCreationHandling = staticSettingsValue.ObjectCreationHandling,
                        PreserveReferencesHandling = staticSettingsValue.PreserveReferencesHandling,
                        ReferenceLoopHandling = staticSettingsValue.ReferenceLoopHandling,
                        StringEscapeHandling = staticSettingsValue.StringEscapeHandling,
                        TypeNameHandling = staticSettingsValue.TypeNameHandling,
                        CheckAdditionalContent = staticSettingsValue.CheckAdditionalContent,
                        ConstructorHandling = staticSettingsValue.ConstructorHandling,
                        Context = staticSettingsValue.Context,
                        Culture = staticSettingsValue.Culture,
                        DateFormatString = staticSettingsValue.DateFormatString,
                        DateParseHandling = staticSettingsValue.DateParseHandling,
                        FloatFormatHandling = staticSettingsValue.FloatFormatHandling,
                        FloatParseHandling = staticSettingsValue.FloatParseHandling,
                        MetadataPropertyHandling = staticSettingsValue.MetadataPropertyHandling,
                        TraceWriter = staticSettingsValue.TraceWriter
                    };
                    
                    instanceSettingsField.SetValue(client, instanceSettings);
                }
            }
        }
        
        return client;
    }

    /// <summary>
    /// Gets an authenticated AdminControllerClient
    /// </summary>
    public AdminControllerClient GetAdminClient()
    {
        return InitializeClientSettings(new AdminControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AuthControllerClient
    /// </summary>
    public AuthControllerClient GetAuthClient()
    {
        return InitializeClientSettings(new AuthControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AIPromptControllerClient
    /// </summary>
    public AIPromptControllerClient GetAIPromptClient()
    {
        return InitializeClientSettings(new AIPromptControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AIResponseControllerClient
    /// </summary>
    public AIResponseControllerClient GetAIResponseClient()
    {
        return InitializeClientSettings(new AIResponseControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated ChannelControllerClient
    /// </summary>
    public ChannelControllerClient GetChannelClient()
    {
        return InitializeClientSettings(new ChannelControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated MessageControllerClient
    /// </summary>
    public MessageControllerClient GetMessageClient()
    {
        return InitializeClientSettings(new MessageControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated NotificationControllerClient
    /// </summary>
    public NotificationControllerClient GetNotificationClient()
    {
        return InitializeClientSettings(new NotificationControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated SettingsControllerClient
    /// </summary>
    public SettingsControllerClient GetSettingsClient()
    {
        return InitializeClientSettings(new SettingsControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated UserControllerClient
    /// </summary>
    public UserControllerClient GetUserClient()
    {
        return InitializeClientSettings(new UserControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated WorkspaceControllerClient
    /// </summary>
    public WorkspaceControllerClient GetWorkspaceClient()
    {
        return InitializeClientSettings(new WorkspaceControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated OAuth2IntegrationControllerClient
    /// </summary>
    public OAuth2IntegrationControllerClient GetOAuth2IntegrationClient()
    {
        return InitializeClientSettings(new OAuth2IntegrationControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated WellKnownControllerClient
    /// </summary>
    public WellKnownControllerClient GetWellKnownClient()
    {
        return InitializeClientSettings(new WellKnownControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated MagiclinkControllerClient
    /// </summary>
    public MagiclinkControllerClient GetMagiclinkClient()
    {
        return InitializeClientSettings(new MagiclinkControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated TokenControllerClient
    /// </summary>
    public TokenControllerClient GetTokenClient()
    {
        return InitializeClientSettings(new TokenControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated SearchControllerClient
    /// </summary>
    public SearchControllerClient GetSearchClient()
    {
        return InitializeClientSettings(new SearchControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AnswersControllerClient
    /// </summary>
    public AnswersControllerClient GetAnswersClient()
    {
        return InitializeClientSettings(new AnswersControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated ContactInfoControllerClient
    /// </summary>
    public ContactInfoControllerClient GetContactInfoClient()
    {
        return InitializeClientSettings(new ContactInfoControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AttachmentsControllerClient
    /// </summary>
    public AttachmentsControllerClient GetAttachmentsClient()
    {
        return InitializeClientSettings(new AttachmentsControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated CarbonLinkControllerClient
    /// </summary>
    public CarbonLinkControllerClient GetCarbonLinkClient()
    {
        return InitializeClientSettings(new CarbonLinkControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated PublicControllerClient
    /// </summary>
    public PublicControllerClient GetPublicClient()
    {
        return InitializeClientSettings(new PublicControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated FavoriteControllerClient
    /// </summary>
    public FavoriteControllerClient GetFavoriteClient()
    {
        return InitializeClientSettings(new FavoriteControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated GateControllerClient
    /// </summary>
    public GateControllerClient GetGateClient()
    {
        return InitializeClientSettings(new GateControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated HealthControllerClient
    /// </summary>
    public HealthControllerClient GetHealthClient()
    {
        return InitializeClientSettings(new HealthControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated HeardStatusControllerClient
    /// </summary>
    public HeardStatusControllerClient GetHeardStatusClient()
    {
        return InitializeClientSettings(new HeardStatusControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated HomeControllerClient
    /// </summary>
    public HomeControllerClient GetHomeClient()
    {
        return InitializeClientSettings(new HomeControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated LabelControllerClient
    /// </summary>
    public LabelControllerClient GetLabelClient()
    {
        return InitializeClientSettings(new LabelControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated LinkControllerClient
    /// </summary>
    public LinkControllerClient GetLinkClient()
    {
        return InitializeClientSettings(new LinkControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated LogControllerClient
    /// </summary>
    public LogControllerClient GetLogClient()
    {
        return InitializeClientSettings(new LogControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated MessageMetadataControllerClient
    /// </summary>
    public MessageMetadataControllerClient GetMessageMetadataClient()
    {
        return InitializeClientSettings(new MessageMetadataControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated OpengraphControllerClient
    /// </summary>
    public OpengraphControllerClient GetOpengraphClient()
    {
        return InitializeClientSettings(new OpengraphControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated PaymentPlanControllerClient
    /// </summary>
    public PaymentPlanControllerClient GetPaymentPlanClient()
    {
        return InitializeClientSettings(new PaymentPlanControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated AppStoreControllerClient
    /// </summary>
    public AppStoreControllerClient GetAppStoreClient()
    {
        return InitializeClientSettings(new AppStoreControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated StripeControllerClient
    /// </summary>
    public StripeControllerClient GetStripeClient()
    {
        return InitializeClientSettings(new StripeControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated PlayStoreControllerClient
    /// </summary>
    public PlayStoreControllerClient GetPlayStoreClient()
    {
        return InitializeClientSettings(new PlayStoreControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated PlaylistControllerClient
    /// </summary>
    public PlaylistControllerClient GetPlaylistClient()
    {
        return InitializeClientSettings(new PlaylistControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated PushControllerClient
    /// </summary>
    public PushControllerClient GetPushClient()
    {
        return InitializeClientSettings(new PushControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated ReactionControllerClient
    /// </summary>
    public ReactionControllerClient GetReactionClient()
    {
        return InitializeClientSettings(new ReactionControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated StreamControllerClient
    /// </summary>
    public StreamControllerClient GetStreamClient()
    {
        return InitializeClientSettings(new StreamControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated StatisticsControllerClient
    /// </summary>
    public StatisticsControllerClient GetStatisticsClient()
    {
        return InitializeClientSettings(new StatisticsControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated VoicemailControllerClient
    /// </summary>
    public VoicemailControllerClient GetVoicemailClient()
    {
        return InitializeClientSettings(new VoicemailControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated ActionItemControllerClient
    /// </summary>
    public ActionItemControllerClient GetActionItemClient()
    {
        return InitializeClientSettings(new ActionItemControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated UsersSCIMControllerClient
    /// </summary>
    public UsersSCIMControllerClient GetUsersSCIMClient()
    {
        return InitializeClientSettings(new UsersSCIMControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated LanguageControllerClient
    /// </summary>
    public LanguageControllerClient GetLanguageClient()
    {
        return InitializeClientSettings(new LanguageControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated InboxNotificationControllerClient
    /// </summary>
    public InboxNotificationControllerClient GetInboxNotificationClient()
    {
        return InitializeClientSettings(new InboxNotificationControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated MessageForwardControllerClient
    /// </summary>
    public MessageForwardControllerClient GetMessageForwardClient()
    {
        return InitializeClientSettings(new MessageForwardControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated MessageShareLinkControllerClient
    /// </summary>
    public MessageShareLinkControllerClient GetMessageShareLinkClient()
    {
        return InitializeClientSettings(new MessageShareLinkControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated OpenAiChatBotTestControllerClient
    /// </summary>
    public OpenAiChatBotTestControllerClient GetOpenAiChatBotTestClient()
    {
        return InitializeClientSettings(new OpenAiChatBotTestControllerClient(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets an authenticated GetRecentMessagesV3Client
    /// </summary>
    public GetRecentMessagesV3Client GetRecentMessagesV3Client()
    {
        return InitializeClientSettings(new GetRecentMessagesV3Client(_httpClient)
        {
            BaseUrl = _baseUrl
        });
    }

    /// <summary>
    /// Gets the underlying HttpClient (useful for creating custom clients)
    /// </summary>
    public HttpClient GetHttpClient()
    {
        return _httpClient;
    }

    /// <summary>
    /// Gets the base URL
    /// </summary>
    public string GetBaseUrl()
    {
        return _baseUrl;
    }

    /// <summary>
    /// Disposes the underlying HttpClient
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

