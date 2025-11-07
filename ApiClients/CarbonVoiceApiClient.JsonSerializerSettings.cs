using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Loudspeaker.ApiClients;

// Partial class implementations to configure JSON serialization settings
// for all controller clients to use Newtonsoft.Json with StringEnumConverter

partial class AdminControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AIPromptControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AIResponseControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AnswersControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MessageControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
            )
        );
    }
}

partial class UserControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ContactInfoControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ChannelControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AuthControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AttachmentsControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class CarbonLinkControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class WorkspaceControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SettingsControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class PublicControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class FavoriteControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GateControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class HealthControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class HeardStatusControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class HomeControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class LabelControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class LinkControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class LogControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MagiclinkControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MessageMetadataControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class NotificationControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class WellKnownControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class OAuth2IntegrationControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class OpengraphControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class PaymentPlanControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AppStoreControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StripeControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class PlayStoreControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class PlaylistControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class PushControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ReactionControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SearchControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StreamControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StatisticsControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class TokenControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class VoicemailControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ActionItemControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UsersSCIMControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class LanguageControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class InboxNotificationControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MessageForwardControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MessageShareLinkControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class OpenAiChatBotTestControllerClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

// Additional client classes (non-controller clients)
partial class GetRecentMessagesV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
               
            )
        );
    }
}

partial class IndexChannelClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessageStatsClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GenerateTextModelClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetWelcomeMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartChannelMessageV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartPrerecordedMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartLabeledMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartWelcomeMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class TranslateMessagesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class TranslateMessagesV4Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetStreamKeyV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessagesByIdClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessagesByIdV4Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UpsertMessageSummaryClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetNotifiedMessagesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessagesBySequenceIdClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetNotifiedMessagesV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UpdateMessageNameClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetPrerecordedMessagesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class CreatePrerecordedMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartVoiceMemoMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class StartStoredMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetVoicememoMessagesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetVoicememoMessagesV4Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class DeleteMessagesByIdClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessageInteractionsClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SendPrerecordedMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ReplyWithPrerecordedMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SendExistingMessageClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SendExistingMessageAsReplyClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UpdateTranscriptClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessageByIdV4Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class FindUnverifiedMessagesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AddDubbedAudioClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UserProfileV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMyContactIdsV2Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class TrainUserVoiceV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class CreateDerivedConversationV2Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetSignedUrlV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetSignedUrlForDownloadV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UpdateWorkspaceRoleClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetWorkspaceV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetWorkspacesV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetWorkspaceLogoClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AddWorkspaceLogoClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class CreateWorkspaceClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class UpdateWorkspaceClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AddDomainToWorkspaceClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class RemoveDomainToWorkspaceClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class AddPhoneNumberClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SetVanityNameClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetSuggestedWorkspacesClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class JoinWorkspaceClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class RedeemGateTokenV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MagiclinkV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetUrlInfoV2Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class SearchV2Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class MyClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class IntoClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class FromClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class FilterClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class CreateClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class ByClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

partial class GetMessageShareLinkV3Client
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(
            new Newtonsoft.Json.Converters.StringEnumConverter(
                namingStrategy: null,
                allowIntegerValues: false
            )
        );
    }
}

