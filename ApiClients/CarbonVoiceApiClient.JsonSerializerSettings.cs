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
                
                
            )
        );
    }
}

